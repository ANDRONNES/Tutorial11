﻿namespace Tutorial11.Exceptions;

public class ConflictException : Exception
{
    public ConflictException(string message):base(message){}
}