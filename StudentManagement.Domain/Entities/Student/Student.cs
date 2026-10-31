using System;
using System.Collections.Generic;
using System.Text;
using StudentSkillManagement.Domain.Entities.StudentSkill;

namespace StudentSkillManagement.Domain.Entities.Student
{
    public sealed class Student
    {
        public Guid Id { get; private set; }

        public string FirstName { get; private set; } = string.Empty;

        public string LastName { get; private set; } = string.Empty;

        public string Email { get; private set; } = string.Empty;

        public bool IsActive { get; private set; }
        public ICollection<StudentSkill.StudentSkill> StudentSkills { get; private set; }
        = new List<StudentSkill.StudentSkill>();


        private Student()
        {
            // Reserved for ORM materialization.
        }

        private Student(
            string firstName,
            string lastName,
            string email)
        {
            Id = Guid.NewGuid();

            SetName(firstName, lastName);
            SetEmail(email);

            IsActive = true;
        }

        public static Student Create(
            string firstName,
            string lastName,
            string email)
        {
            return new Student(firstName, lastName, email);
        }

        public void UpdateProfile(
            string firstName,
            string lastName,
            string email)
        {
            EnsureActive();

            SetName(firstName, lastName);
            SetEmail(email);
        }

        public void Deactivate()
        {
            IsActive = false;
        }

        public void Activate()
        {
            IsActive = true;
        }

        private void SetName(string firstName, string lastName)
        {
            FirstName = NormalizeRequired(firstName, nameof(firstName));
            LastName = NormalizeRequired(lastName, nameof(lastName));
        }

        private void SetEmail(string email)
        {
            var normalizedEmail = NormalizeRequired(email, nameof(email));

            if (!System.Net.Mail.MailAddress.TryCreate(
                    normalizedEmail, out var address)
                || !string.Equals(
                    address.Address,
                    normalizedEmail,
                    StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException(
                    "Email address is invalid.",
                    nameof(email));
            }

            Email = normalizedEmail;
        }

        private static string NormalizeRequired(
            string value,
            string parameterName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(
                    "Value cannot be empty.",
                    parameterName);
            }

            return value.Trim();
        }

        private void EnsureActive()
        {
            if (!IsActive)
            {
                throw new InvalidOperationException(
                    "An inactive student cannot update their profile.");
            }
        }
    }
}
