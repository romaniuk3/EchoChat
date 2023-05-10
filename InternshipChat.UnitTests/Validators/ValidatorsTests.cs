using FluentValidation.Internal;
using InternshipChat.BLL.Validators;
using InternshipChat.Shared.DTO;
using InternshipChat.Shared.DTO.UserDtos;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipChat.UnitTests.Validators
{
    [TestFixture]
    public class ValidatorsTests
    {
        [TestCase("test@mail.com", "P@ssword1", true)]
        [TestCase("test.com", "P@ssword1", false)]
        [TestCase("test@mail.com", "P@1", false)]
        [TestCase("test@mail.com", "P@ss", false)]
        [TestCase("test@mail.com", "P@ssword", false)]
        [TestCase("test@mail.com", "Pssword12", false)]
        [TestCase("test@mail.com", "P@sswor1", true)]
        public void LoginValidatorTest(string email, string password, bool isValid)
        {
            var validator = new LoginDtoValidator();

            var loginDto = new LoginDto
            {
                Email = email,
                Password = password,
            };

            var validatorResult = validator.Validate(loginDto);

            Assert.AreEqual(isValid, validatorResult.IsValid);
        }

        [TestCase("test@mail.com", "test", "test", "P@ssword1", "2021-02-22", false)]
        public void RegisterValidatorTest(string email, string firstName, string lastName, string password, 
            string birthdate, bool isValid) 
        { 
            var validator = new RegisterDTOValidator();

            var registerDto = new RegisterUserDTO
            {
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                Password = password,
                Birthdate = DateTime.Parse(birthdate)
            };

            var validatorResult = validator.Validate(registerDto);

            Assert.AreEqual(isValid, validatorResult.IsValid);
        }
    }
}
