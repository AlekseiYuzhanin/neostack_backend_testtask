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
    public async Task<ActionResult<List<PersonWithSkillsDto>>> GetPersons()
    {
        var persons = await _personService.GetPersons();
        return Ok(persons);
    }
}