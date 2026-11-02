using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StudentSkillManagement.Application.Abstractions.Persistence;
using StudentSkillManagement.Application.Features.Students.Commands.ChangeStudentStatus;
using StudentSkillManagement.Application.Features.Students.Commands.CreateStudent;
using StudentSkillManagement.Application.Features.Students.Commands.UpdateStudentProfile;
using StudentSkillManagement.Application.Features.Students.Queries.GetAllStudents;
using StudentSkillManagement.Application.Features.Students.Queries.GetStudentById;
using System;
using System.Collections.Generic;
using System.Text;


namespace StudentSkillManagement.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services)
        {
            services.AddScoped<CreateStudentCommandHandler>();
            services.AddScoped<GetStudentByIdQueryHandler>();
            services.AddScoped<GetAllStudentsQueryHandler>();
            services.AddScoped<UpdateStudentProfileCommandHandler>();
            services.AddScoped<ChangeStudentStatusCommandHandler>();

            return services;
        }
    }
}
