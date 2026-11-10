using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudentSkillManagement.Domain.Entities.Skill;
using StudentSkillManagement.Domain.Entities.Student;
using StudentSkillManagement.Domain.Entities.StudentSkill;
using StudentSkillManagement.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentSkillManagement.Infrastructure.Persistence
{
    public sealed class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Student Configuration
            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(s => s.Id);

                entity.Property(s => s.FirstName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(s => s.LastName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(s => s.Email)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            // Skill Configuration
            modelBuilder.Entity<Skill>(entity =>
            {
                entity.HasKey(s => s.Id);

                entity.Property(s => s.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            // StudentSkill Configuration
            modelBuilder.Entity<StudentSkill>(entity =>
            {
                entity.HasKey(ss => new { ss.StudentId, ss.SkillId });

                entity.HasOne(ss => ss.Student)
                    .WithMany(s => s.StudentSkills)
                    .HasForeignKey(ss => ss.StudentId);

                entity.HasOne(ss => ss.Skill)
                    .WithMany(s => s.StudentSkills)
                    .HasForeignKey(ss => ss.SkillId);
            });
        }

        public DbSet<Student> Students => Set<Student>();
        public DbSet<Skill> Skills => Set<Skill>();
        public DbSet<StudentSkill> StudentSkills => Set<StudentSkill>();
    }
}
