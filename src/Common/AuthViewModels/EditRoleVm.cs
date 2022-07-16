using System.ComponentModel.DataAnnotations;

namespace Common.AuthViewModels
{
    public class EditRoleVm
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string RoleName { get; set; }
    }
}
