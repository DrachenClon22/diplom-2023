using diplomOriginal.Controllers;
using diplomOriginal.Models;
using diplomOriginal.Modules;
using diplomOriginal.Login;
using MySqlConnector;

public static class Database
{
    private static string _connection = string.Empty;
    public static void InitConnection(string connection)
    {
        _connection = connection;
    }

    // Returns false if string is probably injection, returns true is the string is safe
    public static bool CheckForSQLInjection(string input)
    {
        // SQL Injection cheat sheet
        string[] sqlCheckList = { "--", ";--",";", "/*","*/","@@",
            "char", "nchar", "varchar", "nvarchar", "alter", "begin", "cast", "create", "cursor", "declare", "delete", 
            "drop", "end", "exec", "execute", "fetch", "insert", "kill", "select", "sys", "sysobjects", "syscolumns", "table", "update"
        };

        // Remove any single quote
        string checkString = input.Replace("'", "");

        // Check result string for any injections, if any, return false, if none, return true
        foreach (var item in sqlCheckList)
        {
            if (checkString.Contains(item))
            {
                return false;
            }
        }

        return true;
    }

    public static async Task<bool> DoesAccountExist(string email)
    {
        if (CheckForSQLInjection(email))
        {
            using (var connection = new MySqlConnection(_connection))
            {
                await connection.OpenAsync();

                // Validate form to avoid sql injections
                using var command = new MySqlCommand($"SELECT email FROM user WHERE email=\"{email}\";", connection);
                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    return !string.IsNullOrEmpty(reader.GetValue(0)?.ToString() ?? string.Empty);
                }
            }
        }

        return false;
    }

    internal static async Task<bool> ChangeAccountPassword(LoginViewModel account, string newPassword)
    {
        if (DoesAccountExist(account.Email).Result)
        {
            try
            {
                using (var connection = new MySqlConnection(_connection))
                {
                    await connection.OpenAsync();

                    using var command = new MySqlCommand($"UPDATE user SET password=\"{Encryption.EncodePasswordToBase64(newPassword)}\"" +
                        $"WHERE email=\"{account.Email}\"");
                    command.Connection = connection;
                    await command.ExecuteNonQueryAsync();

                    return true;
                }

            }
            catch (Exception e)
            {
                await Console.Out.WriteLineAsync($"Error occured: {e.Message}");
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public static async Task<bool> ChangeAccountName(LoginViewModel account, string newName)
    {
        if (DoesAccountExist(account.Email).Result)
        {
            try
            {
                if (CheckForSQLInjection(newName))
                {
                    using (var connection = new MySqlConnection(_connection))
                    {
                        await connection.OpenAsync();

                        using var command = new MySqlCommand($"UPDATE user SET username=\"{newName}\"" +
                            $"WHERE email=\"{account.Email}\"");
                        command.Connection = connection;
                        await command.ExecuteNonQueryAsync();

                        return true;
                    }
                } else
                {
                    throw new Exception($"Probability of SQL Injection. {account.Email} tried to change name to {newName}");
                }
            }
            catch (Exception e)
            {
                await Console.Out.WriteLineAsync($"Error occured: {e.Message}");
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public static async Task<bool> AddAccountToDB(LoginViewModel account)
    {
        if (!DoesAccountExist(account.Email).Result)
        {
            try
            {
                if (CheckForSQLInjection(account.Email))
                {
                    // Remove later
                    if (account.Email.Equals("banned@gmail.com"))
                    {
                        throw new Exception("Test Exception");
                    }

                    using (var connection = new MySqlConnection(_connection))
                    {
                        await connection.OpenAsync();

                        using var command = new MySqlCommand($"INSERT INTO user (email, password, role) VALUES " +
                            $"(\"{account.Email}\"," +
                            $" \"{Encryption.EncodePasswordToBase64(account.Password)}\", \"{Role.USER}\");", connection);
                        command.Connection = connection;
                        await command.ExecuteNonQueryAsync();

                        ConsoleLogger.Log($"New user registered: {account.Email}");

                        return true;
                    }
                } else
                {
                    throw new Exception($"Probability of SQL Injection, raw input: {account.Email}");
                }

            }
            catch (Exception e)
            {
                ConsoleLogger.Log(LogStatus.ERROR, $"Error occured: {e.Message}");
                return false;
            }
        } else
        {
            return false;
        }
    }

    public static async Task<Person?> GetPerson(string email, string password)
    {
        Person? result = null;

        if (CheckForSQLInjection(email) && CheckForSQLInjection(password))
        {
            using (var connection = new MySqlConnection(_connection))
            {
                await connection.OpenAsync();

                using var command = new MySqlCommand($"SELECT email,password,username,role FROM user WHERE email=\"{email}\" " +
                    $"AND password=\"{Encryption.EncodePasswordToBase64(password)}\";", connection);
                command.Connection = connection;
                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    string? inputEmail = reader.GetValue(0)?.ToString() ?? null;
                    string? inputName = reader.GetValue(2)?.ToString() ?? "Unknown";
                    string? inputRole = reader.GetValue(3)?.ToString() ?? "USER";

                    ConsoleLogger.Log($"{email} logged in!");

                    if (string.IsNullOrEmpty(email))
                    {
                        return null;
                    }

                    if (Enum.TryParse<Role>(inputRole, out var r_res))
                    {
                        result = new Person(email, r_res, inputName);
                    }
                }
            }
        } else
        {
            ConsoleLogger.Log(LogStatus.WARNING, $"SQL Injection. Access denied. -> {email}");
        }

        return result;
    }
}