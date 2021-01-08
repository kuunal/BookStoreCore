using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ModelLayer.CustomValidators
{ 
    public class Roles
    {
        static public List<string> roles = new List<string>
        {
            "admin",
            "user"
        };
    }

    public class RoleValidator : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (Roles.roles.Contains(value.ToString()))
            {
                return ValidationResult.Success;
            }
            return new ValidationResult("Please provide valid role");
        }
    }
}
