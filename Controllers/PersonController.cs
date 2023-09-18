using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/v1/persons")]
[ApiController]
public class PersonController: ControllerBase{

    public readonly BaseContext _db;
    public PersonController(BaseContext db){
        _db = db;
    }
    [HttpPost]
    public IActionResult AddPersonWithSkills([FromBody] PersonWithSkillsDto personDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var person = new Person
        {
            Name = personDto.Name,
            DisplayName = personDto.DisplayName
        };

        foreach (var skillDto in personDto.Skills)
        {
            var skill = new Skill
            {
                Name = skillDto.Name,
                Level = skillDto.Level
            };

            person.Skills.Add(skill);
        }

        _db.Person.Add(person);
        _db.SaveChanges();

        
        return Ok("Person has been added");
    }

    [HttpGet]
    public ActionResult<IEnumerable<PersonWithSkillsDto>> GetPersons()
    {
        var persons = _db.Person
            .Include(p => p.Skills)
            .Select(p => new PersonWithSkillsDto
            {
                Name = p.Name,
                DisplayName = p.DisplayName,
                Skills = p.Skills.Select(s => new SkillDto
                {
                    Name = s.Name,
                    Level = s.Level
                }).ToList()
            })
            .ToList();

        return Ok(persons);
    }

    [HttpGet("{id}")]
    public ActionResult<PersonWithSkillsDto> GetPersonById(int id)
    {
        var person = _db.Person
            .Include(p => p.Skills)
            .Where(p => p.Id == id)
            .Select(p => new PersonWithSkillsDto
            {
                Name = p.Name,
                DisplayName = p.DisplayName,
                Skills = p.Skills.Select(s => new SkillDto
                {
                    Name = s.Name,
                    Level = s.Level
                }).ToList()
            })
            .FirstOrDefault();

        if (person == null)
        {
            return NotFound();
        }

        return Ok(person);
    }

    [HttpPut("{id}")]
public ActionResult<PersonWithSkillsDto> UpdatePersonById(int id, PersonWithSkillsDto updatedPerson)
{
    var person = _db.Person
        .Include(p => p.Skills)
        .FirstOrDefault(p => p.Id == id);

    if (person == null)
    {
        return NotFound();
    }

    person.Name = updatedPerson.Name;
    person.DisplayName = updatedPerson.DisplayName;

    foreach (var skillDto in updatedPerson.Skills)
    {
        var existingSkill = person.Skills.FirstOrDefault(s => s.Name == skillDto.Name);

        if (existingSkill != null)
        {
            existingSkill.Level = skillDto.Level;
        }
        else
        {
            var newSkill = new Skill
            {
                Name = skillDto.Name,
                Level = skillDto.Level
            };
            person.Skills.Add(newSkill);
        }
    }

        _db.SaveChanges();

        return Ok(updatedPerson);
    }

    [HttpDelete("{id}")]
    public ActionResult DeletePersonById(int id)
    {
        var person = _db.Person
            .FirstOrDefault(p => p.Id == id);

        if (person == null)
        {
            return NotFound();
        }

        _db.Person.Remove(person);
        _db.SaveChanges();

        return Ok();
}


}