using System.ComponentModel.DataAnnotations;

namespace Common.AuthViewModels
{
    public class RoleVm
    {
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
