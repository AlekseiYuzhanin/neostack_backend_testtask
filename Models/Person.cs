using System.ComponentModel.DataAnnotations;

public class Person
    {
        public Person()
        {
            Skills = new HashSet<Skill>();
        }
        public long Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public virtual ICollection<Skill> Skills { get; set; }
    }