using System.ComponentModel.DataAnnotations;

namespace ViewQuickApp.Server.Core.Dtos.Auth
{
    public class UpdateDto
    {
        [Required(ErrorMessage = "UserName is Required..")]
        public string UserName { get; set; }

        public RoleType NewRole { get; set; }

    }

    public enum RoleType
    {

        ADMIN,
        MANAGER,
        USER
    }
}
