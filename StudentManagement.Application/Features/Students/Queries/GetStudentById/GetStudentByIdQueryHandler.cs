using StudentSkillManagement.Application.Abstractions.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentSkillManagement.Application.Features.Students.Queries.GetStudentById
{
    // continue implementing the GetStudentByIdQueryHandler use case
    public sealed class GetStudentByIdQueryHandler
    {
        private readonly IStudentRepository _studentRepository;
        public GetStudentByIdQueryHandler(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

    }
}
