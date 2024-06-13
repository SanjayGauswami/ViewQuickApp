using System.ComponentModel.DataAnnotations;

namespace ViewQuickApp.Server.Core.Dtos.Auth
{
    public class LoginDto
    {
        [Required(ErrorMessage = "UserName is Require..")]
        public string UserName { get; set; }


        [Required(ErrorMessage = "Password is Require..")]
        public string Password { get; set; }
    }
}
