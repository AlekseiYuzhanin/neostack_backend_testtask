using Microsoft.AspNetCore.Mvc;

public interface IPersonService
{
    Task<ActionResult> DeletePersonById(long id);
    Task<IEnumerable<PersonWithSkillsDto>> GetPersons();
    Task<PersonWithSkillsDto> GetPersonById(long id);
    Task<PersonWithSkillsDto> UpdatePersonById(long id, PersonWithSkillsDto updatedPerson);
    Task<ActionResult<Person>> AddPersonWithSkills([FromBody] PersonWithSkillsDto personDto);
}