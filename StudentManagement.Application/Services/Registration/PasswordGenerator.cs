using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;


namespace StudentManagement.Application.Services
    {
        public static class PasswordGenerator
        {
            public static string Generate(int length = 10)
            {
                const string chars =
                    "ABCDEFGHJKLMNPQRSTUVWXYZ" +
                    "abcdefghijkmnopqrstuvwxyz" +
                    "23456789" +
                    "@#$";

                var result = new char[length];

                for (int i = 0; i < length; i++)
                {
                    result[i] =
                        chars[RandomNumberGenerator.GetInt32(
                            chars.Length)];
                }

                return new string(result);
            }
        }
    }

