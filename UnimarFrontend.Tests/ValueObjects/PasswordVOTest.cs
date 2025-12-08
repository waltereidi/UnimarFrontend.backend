using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnimarFrontend.backend.ValueObjects;

namespace UnimarFrontend.Tests.ValueObjects
{

    public class PasswordVOTest
    {
        [Fact]
        public void Should_Create_Password_Hash()
        {
            // Arrange
            string plain = "senha123";

            // Act
            var password = new PasswordVO(plain);

            // Assert
            Assert.False(String.IsNullOrEmpty(password.Hash));
        }

    }
}
