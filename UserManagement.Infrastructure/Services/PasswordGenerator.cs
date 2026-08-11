using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UserManagement.Application.Interfaces;

namespace UserManagement.Infrastructure.Services
{
    public class PasswordGenerator : IPasswordGenerator
    {
        public string GeneratePassword()
        {
            const string upperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lowerCase = "abcdefghijklmnopqrstuvwxyz";
            const string numbers = "0123456789";
            const string specialCharacters = "!@#$%^&*";

            const string allCharacters =
                upperCase + lowerCase + numbers + specialCharacters;

            var password = new char[12];

            password[0] = GetRandomCharacter(upperCase);
            password[1] = GetRandomCharacter(lowerCase);
            password[2] = GetRandomCharacter(numbers);
            password[3] = GetRandomCharacter(specialCharacters);

            for (int i = 4; i < password.Length; i++)
            {
                password[i] = GetRandomCharacter(allCharacters);
            }

            return new string(password);
        }

        private char GetRandomCharacter(string characters)
        {
            int index = RandomNumberGenerator.GetInt32(
                characters.Length);

            return characters[index];
        }
    }
}
