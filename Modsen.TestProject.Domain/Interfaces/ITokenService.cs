﻿namespace Modsen.TestProject.Domain.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(string username, string role);
    }

}
