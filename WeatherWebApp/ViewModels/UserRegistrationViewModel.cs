using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeatherWebApp.ViewModels
{
    public class UserRegistrationViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}