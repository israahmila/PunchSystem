﻿namespace PunchSystem.DTOs
{
    public class ResetPasswordRequest
    {
        public string Username { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}
