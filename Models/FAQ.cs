﻿namespace API.Models
{
    public class FAQ
    {
        public Guid id { get; set; }
        public int orden { get; set; }
        public string pregunta { get; set; }
        public string? respuesta { get; set; }
    }
}