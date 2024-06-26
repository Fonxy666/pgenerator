﻿namespace PGenerator.Data.TokenStorageFolder;

public interface ITokenStorage
{
    Task SaveTokenAsync(string token);
    Task<string?> ReadTokenAsync();
}