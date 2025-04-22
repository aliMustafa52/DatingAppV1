﻿using System.ComponentModel.DataAnnotations;

namespace DatingApp.Api.Helpers
{
    public class CloudinarySettings
    {
        [Required]
        public string CloudName { get; init; } = string.Empty;

        [Required]
        public string ApiKey { get; init; } = string.Empty;

        [Required]
        public string ApiSecret { get; init; } = string.Empty;
    }
}
