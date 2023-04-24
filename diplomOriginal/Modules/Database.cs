using diplomOriginal.Controllers;
using diplomOriginal.Models;
using diplomOriginal.Modules;
using diplomOriginal.Login;
using MySqlConnector;

namespace diplomOriginal.Modules
{
    public class Database
    {
    }
}
public static class Database
{
    private static string _connection = string.Empty;
    public static void InitConnection(string connection)
    {
        _connection = connection;
    }

    public static async Task<bool> DoesAccountExist(string email)
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

            return false;
        }
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
                using (var connection = new MySqlConnection(_connection))
                {
                    await connection.OpenAsync();

                    using var command = new MySqlCommand($"UPDATE user SET username=\"{newName}\"" +
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

    public static async Task<bool> AddAccountToDB(LoginViewModel account)
    {
        if (!DoesAccountExist(account.Email).Result)
        {
            try
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

                    return true;
                }

            }
            catch (Exception e)
            {
                await Console.Out.WriteLineAsync($"Error occured: {e.Message}");
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

                await Console.Out.WriteLineAsync($"{email}|{inputRole}");

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

        return result;
    }
}