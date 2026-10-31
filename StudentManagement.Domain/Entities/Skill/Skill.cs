using System;
using System.Collections.Generic;
using System.Text;

namespace StudentSkillManagement.Domain.Entities.Skill
{
    public sealed class Skill
    {
        public Guid Id { get; private set; }

        public string Name { get; private set; } = string.Empty;

        public string? Description { get; private set; }


        public bool IsActive { get; private set; }

        private Skill()
        {
            // Reserved for ORM materialization.
        }

        private Skill(
            string name,
            string? description)
        {
            Id = Guid.NewGuid();

            SetName(Name);
            Description = string.IsNullOrWhiteSpace(description)? null : description.Trim();

            IsActive = true;
        }

        public static Skill Create(
            string Name,
            string Description)
        {
            return new Skill(Name, Description);
        }

        public void UpdateSkill(
            string name,
            string description)
        {
            EnsureActive();

            SetName(name);
            Description = description;
        }

        public void Deactivate()
        {
            IsActive = false;
        }

        public void Activate()
        {
            IsActive = true;
        }

        private void SetName(string name)
        {
            Name = NormalizeRequired(name, nameof(name));
        }
        //private void SetDescription(string description)
        //{
        //    Description = NormalizeRequired(description, nameof(description));
        //}

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
                    "An inactive skill cannot be updated");
            }
        }
    }
}
