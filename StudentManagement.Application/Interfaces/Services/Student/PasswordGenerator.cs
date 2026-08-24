
using System.Security.Cryptography;

namespace StudentManagement.Application.Services.Student
{
    public static class PasswordGenerator
    {
        public static string GeneratePassword(
            int length = 10)
        {
            const string characters =
                "ABCDEFGHJKLMNPQRSTUVWXYZ" +
                "abcdefghijkmnopqrstuvwxyz" +
                "23456789@#$";

            var password = new char[length];

            for (int i = 0; i < length; i++)
            {
                password[i] =
                    characters[
                        RandomNumberGenerator
                            .GetInt32(characters.Length)];
            }

            return new string(password);
        }
    }
}

