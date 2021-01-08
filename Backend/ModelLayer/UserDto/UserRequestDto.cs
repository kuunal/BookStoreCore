using ModelLayer.CustomValidators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ModelLayer.UserDto
{
    public class UserRequestDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [RegularExpression(@"^\+[0-9]{12}$"
            ,ErrorMessage = "Phone number should be in +91[Phone Number] format")]
        public string PhoneNumber { get; set; }

        [RegularExpression(@"^(?=.*[0-9])(?=.*[A-Z])(?=[a-zA-Z0-9]*[^a-zA-Z0-9][a-zA-Z0-9]*$).{8,}"
        , ErrorMessage = "Password should contain atleast one Uppercase, lowercase, special and digit and minimum length should be 8")]
        public string Password { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9]+[\\.\\-\\+\\_]?[a-zA-Z0-9]+@[a-zA-Z0-9]+[.]?[a-zA-Z]{2,4}[\\.]?([a-z]{2,4})?$",
        ErrorMessage = "Please provide valid email id")]
        public string Email { get; set; }
        
        [RoleValidator]
        public string Role { get; set; }
    }
}
