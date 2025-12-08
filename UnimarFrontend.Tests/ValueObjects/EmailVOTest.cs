using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnimarFrontend.backend.ValueObjects;

namespace UnimarFrontend.Tests.ValueObjects
{

    public class EmailVOTest
    {
        [Fact]
        public void Should_Create_Email_When_Valid()
        {
            // Arrange
            var emailString = "usuario@teste.com";

            // Act
            var email = new EmailVO(emailString);

            // Assert
            Assert.Equal(email.Value , emailString.ToLower());
        }

    }
}
