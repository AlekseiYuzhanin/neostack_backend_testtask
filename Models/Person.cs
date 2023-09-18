using System.ComponentModel.DataAnnotations;

public class Person
    {
        public Person()
        {
            Skills = new HashSet<Skill>();
        }
        [Key]
        public long Id { get; set; }
        [Required]
        [MaxLength(50)]
        [MinLength(10)]
        public string Name { get; set; }
        [Required]
        [MaxLength(50)]
        [MinLength(10)]
        public string DisplayName { get; set; }
        public virtual ICollection<Skill> Skills { get; set; }
}