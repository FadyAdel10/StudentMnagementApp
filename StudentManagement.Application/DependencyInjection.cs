using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StudentSkillManagement.Application.Abstractions.Persistence;
using StudentSkillManagement.Application.Features.Skills.Commands.ChangeSkillStatus;
using StudentSkillManagement.Application.Features.Skills.Commands.CreateSkill;
using StudentSkillManagement.Application.Features.Skills.Commands.UpdateSkill;
using StudentSkillManagement.Application.Features.Skills.Queries.GetAllSkills;
using StudentSkillManagement.Application.Features.Skills.Queries.GetSkillById;
using StudentSkillManagement.Application.Features.Students.Commands.ChangeStudentStatus;
using StudentSkillManagement.Application.Features.Students.Commands.CreateStudent;
using StudentSkillManagement.Application.Features.Students.Commands.UpdateStudentProfile;
using StudentSkillManagement.Application.Features.Students.Queries.GetAllStudents;
using StudentSkillManagement.Application.Features.Students.Queries.GetStudentById;
using StudentSkillManagement.Application.Features.StudentSkills.Commands.AssignSkillToStudent;
using StudentSkillManagement.Application.Features.StudentSkills.Commands.DeleteStudentSkill;
using StudentSkillManagement.Application.Features.StudentSkills.Commands.UpdateProficiencyLevel;
using StudentSkillManagement.Application.Features.StudentSkills.Queries.GetAllStudentSkills;
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

            services.AddScoped<GetAllSkillsQueryHandler>();
            services.AddScoped<CreateSkillCommandHandler>();
            services.AddScoped<GetSkillByIdQueryHandler>();
            services.AddScoped<UpdateSkillCommandHandler>();
            services.AddScoped<ChangeSkillStatusCommandHandler>();

            services.AddScoped<AssignSkillToStudentCommandHandler>();
            services.AddScoped<GetAllStudentSkillsQueryHandler>();
            services.AddScoped<UpdateProficiencyLevelCommandHandler>();
            services.AddScoped<DeleteStudentSkillCommandHandler>();

            return services;
        }
    }
}
