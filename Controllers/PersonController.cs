using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/v1/persons")]
[ApiController]
public class PersonController : ControllerBase
{
    private readonly IPersonService _personService;

public PersonController(IPersonService personService)
{
    _personService = personService;
}

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PersonWithSkillsDto>>> GetPersons()
    {
        var persons = await _personService.GetPersons();
        return Ok(persons);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetPersonById(long id){
        var person = await _personService.GetPersonById(id);
        return Ok(person);
    }

    [HttpPost]
    public async Task<ActionResult> AddPersonWithSkills([FromBody] PersonWithSkillsDto personDto){
        var options = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.Preserve
        };

        var person = await _personService.AddPersonWithSkills(personDto);
        return Ok(JsonSerializer.Serialize(person, options));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdatePersonById(long id, PersonWithSkillsDto personDto){
        var person = await _personService.UpdatePersonById(id,personDto);
        return Ok(person);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletePersonById(long id){
        var person = await _personService.DeletePersonById(id);
        return Ok(person);
    }

}