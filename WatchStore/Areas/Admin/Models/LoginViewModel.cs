using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WatchStore.Areas.Admin.Models
{
    public class LoginViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Captcha { get; set; }

    }
}