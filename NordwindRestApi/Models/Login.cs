using System;
using System.Collections.Generic;

namespace NordwindRestApi.Models
{
    public partial class Login
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string UserPassword { get; set; } = null!;
    }
}
