using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using WeatherWebApp.Context;

namespace WeatherWebApp.ViewModels
{
    public class UserRegistrationViewModel
    {
        [Required,EmailAddress, Display(Name = "Email")]
        [UniqueEmail(ErrorMessage = "This e-mail is already registered")]

        public string Email { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Required ]
        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword")]
        [Compare("Password", ErrorMessage = "Password aren't same")]
        public string ConfirmPassword { get; set; }
        public class UniqueEmailAttribute : ValidationAttribute
        {
            public override bool IsValid(object value)
            {
                WeatherContext db = new WeatherContext();
                var userWithTheSameEmail = db.Users.SingleOrDefault(
                    u => u.Email == (string)value);
                return userWithTheSameEmail == null;
            }
        }
    }
}