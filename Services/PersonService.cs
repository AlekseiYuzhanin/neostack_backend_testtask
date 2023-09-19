using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
public class PersonService : IPersonService
{
    private readonly BaseContext _db;
    public PersonService(BaseContext db)
    {
        _db = db;
    }

    private ActionResult StatusOk(){
        return new OkResult();
    }

    private ActionResult NotFound()
    {
        return new NotFoundResult();
    }


    private ActionResult BadRequest()
    {
        return new BadRequestResult();
    }


 public async Task<ActionResult> DeletePersonById(long id)
{       
        var person = await _db.Person.FirstOrDefaultAsync(p => p.Id == id);
        var lastPerson = await _db.Person.OrderBy(p => p.Id).LastOrDefaultAsync();
        
        if (person == null)
        {
            return NotFound();
        }

        if (person.Id <= 0 || person.Id > lastPerson.Id)
        {
            return BadRequest();
        }

        _db.Person.Remove(person);
        await _db.SaveChangesAsync();

        return StatusOk();
}

    public async Task<IEnumerable<PersonWithSkillsDto>> GetPersons()
    {
        var persons = await _db.Person
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
            .ToListAsync();

    return persons;
    }

    public async Task<PersonWithSkillsDto> GetPersonById(long id)
{
    var person = await _db.Person
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
        .FirstOrDefaultAsync();

    return person;
}

    public async Task<PersonWithSkillsDto> UpdatePersonById(long id, PersonWithSkillsDto updatedPerson)
    {
        var person = await _db.Person
            .Include(p => p.Skills)
            .FirstOrDefaultAsync(p => p.Id == id);

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

    await _db.SaveChangesAsync();

    return updatedPerson;
    }

    public async Task<ActionResult<Person>> AddPersonWithSkills([FromBody] PersonWithSkillsDto personDto)
    {
        
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
        await _db.SaveChangesAsync();

        return person;

    }

}
