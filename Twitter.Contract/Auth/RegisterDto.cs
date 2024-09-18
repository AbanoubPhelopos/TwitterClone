﻿namespace Twitter.Contract.Auth;

public record RegisterDto(
    string FirstName,
    string LastName,
    string Email,
    string Password
    );
