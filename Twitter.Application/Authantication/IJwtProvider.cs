﻿namespace Twitter.Application.Authantication;

public interface IJwtProvider
{
    (string token, int expiresIn) GenerateToken(User user);
}