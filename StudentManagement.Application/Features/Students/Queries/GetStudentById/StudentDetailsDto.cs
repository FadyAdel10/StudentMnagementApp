using StudentSkillManagement.Application.Features.StudentSkills.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentSkillManagement.Application.Features.Students.Queries.GetStudentById
{
    public sealed record StudentDetailsDto
    (Guid Id,
    string FirstName,
    string LastName,
    string Email,
    bool IsActive,
    List<StudentSkillDetailsDto> Skills);
}
