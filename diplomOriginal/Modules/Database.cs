using diplomOriginal.Controllers;
using diplomOriginal.Models;
using diplomOriginal.Modules;
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

    public static async Task<bool> AddAccountToDB(LoginViewModel account)
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

    public static async Task<Person?> GetPerson(string email, string password)
    {
        Person? result = null;

        using (var connection = new MySqlConnection(_connection))
        {
            await connection.OpenAsync();

            using var command = new MySqlCommand($"SELECT email,password,role FROM user WHERE email=\"{email}\" " +
                $"AND password=\"{Encryption.EncodePasswordToBase64(password)}\";", connection);
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                string? inputEmail = reader.GetValue(0)?.ToString() ?? null;
                string? inputRole = reader.GetValue(2)?.ToString() ?? null;

                await Console.Out.WriteLineAsync($"{email}|{inputRole}");

                if (string.IsNullOrEmpty(email))
                {
                    return null;
                }

                if (Enum.TryParse<Role>(inputRole ?? "USER", out var r_res))
                {
                    result = new Person(email, r_res);
                }
            }
        }

        return result;
    }
}