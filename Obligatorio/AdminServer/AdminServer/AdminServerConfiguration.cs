﻿using System;
namespace AdminServer
{
    public class AdminServerConfiguration
    {
            public string AdminServerIP { get; set; }
            public string AdminServerHttpPort { get; set; }
            public string AdminServerHttpsPort { get; set; }
            public string GrpcServerApiHttpPort { get; set; }
            public string GrpcServerApiHttpsPort { get; set; }
            public string GrpcServerIP { get; set; }
    }
}
