﻿namespace EPAM.RabbitMQ.Models
{
    public class RabbitMqOptions
    {
        public required string UserName { get; set; }

        public required string Password { get; set; }

        public int Port { get; set; }

        public required string VirtualHost { get; set; }
    }
}
