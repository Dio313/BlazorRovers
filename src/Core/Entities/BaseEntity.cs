using System.ComponentModel.DataAnnotations;

namespace Core.Entities;

    public abstract class BaseEntity
    {
        [Key]
        [ScaffoldColumn(false)]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
    }

