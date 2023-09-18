using System.ComponentModel.DataAnnotations;

public class Skill
    {
        [Key]
        public long Id { get; set; }
        [Required]
        [MaxLength(50)]
        [MinLength(10)]
        public string Name { get; set; }
        [Range(1,10)]
        public byte Level { get; set; }
        public long PersonId { get; set; }
        public virtual Person Person { get; set; }
    }