using Microsoft.AspNet.Identity.EntityFramework;

namespace WeatherWebApp.Models
{
    public class AppRole : IdentityRole
    {
        public AppRole() : base() { }
        public AppRole(string name) : base(name) { }
    }
}