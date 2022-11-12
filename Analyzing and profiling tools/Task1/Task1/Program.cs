﻿using System.Diagnostics;
using System.Security.Cryptography;

//62ms
var stopwatch = new Stopwatch();
var passwordText = "Hello, World";

stopwatch.Start();
var result = GeneratePasswordHashUsingSalt(passwordText,
    new byte[]
    {
        1, 2, 3, 4,
        1, 2, 3, 4,
        1, 2, 3, 4,
        1, 2, 3, 4
    });
stopwatch.Stop();
Console.WriteLine($"Elapsed Milliseconds: {stopwatch.ElapsedMilliseconds}ms");

Console.WriteLine(result);

string GeneratePasswordHashUsingSalt(string passwordText, byte[] salt)
{

    var iterate = 10000;
    var pbkdf2 = new Rfc2898DeriveBytes(passwordText, salt, iterate);
    byte[] hash = pbkdf2.GetBytes(20);

    byte[] hashBytes = new byte[36];

    Array.Copy(salt, 0, hashBytes, 0, 16);
    Array.Copy(hash, 0, hashBytes, 16, 20);

    var passwordHash = Convert.ToBase64String(hashBytes);

    return passwordHash;

}

