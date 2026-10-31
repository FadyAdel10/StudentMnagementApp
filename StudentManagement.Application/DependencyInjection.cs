using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StudentSkillManagement.Application.Abstractions.Persistence;
using StudentSkillManagement.Application.Features.Students.Commands.CreateStudent;
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

            return services;
        }
    }
}
