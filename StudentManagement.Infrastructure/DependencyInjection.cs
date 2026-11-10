using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StudentSkillManagement.Application.Abstractions.Authentication;
using StudentSkillManagement.Application.Abstractions.Persistence;
using StudentSkillManagement.Infrastructure.Authentication;
using StudentSkillManagement.Infrastructure.Identity;
using StudentSkillManagement.Infrastructure.Persistence;
using StudentSkillManagement.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentSkillManagement.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));
            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddScoped<IJwtTokenService, JwtTokenService>();

            var connectionString =
                configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            services.AddIdentityCore<ApplicationUser>().AddRoles<IdentityRole<Guid>>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<ISkillRepository, SkillRepository>();
            services.AddScoped<IStudentSkillRepository, StudentSkillRepository>();

            return services;
        }
    }
}
