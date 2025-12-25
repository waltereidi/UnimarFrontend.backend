using System;
using System.Collections.Generic;
using System.Text;
using UnimarFrontend.Dominio.ValueObjects;

namespace UnimarFrontend.Tests.ValueObjects
{
    public class FileNameWithExtensionTests
    {
        [Fact]
        public void Should_Replace_File_Extension_Correctly()
        {
            // Arrange
            var fileName = "document.pdf";
            var newExtension = "txt";

            // Act
            var vo = new FileNameWithExtension(fileName, newExtension);

            // Assert
            Assert.Equal("document.txt", vo.Value);
        }

        [Fact]
        public void Should_Handle_Extension_With_Dot()
        {
            var vo = new FileNameWithExtension("image.jpeg", ".png");

            Assert.Equal("image.png", vo.Value);
        }

        [Fact]
        public void Should_Add_Extension_When_File_Has_No_Extension()
        {
            var vo = new FileNameWithExtension("readme", "md");

            Assert.Equal("readme.md", vo.Value);
        }

        [Fact]
        public void Should_Throw_When_FileName_Is_Invalid()
        {
            Assert.Throws<ArgumentException>(() =>
                new FileNameWithExtension("", "txt"));
        }

        [Fact]
        public void Should_Throw_When_Extension_Is_Invalid()
        {
            Assert.Throws<ArgumentException>(() =>
                new FileNameWithExtension("file.txt", ""));
        }
    }
}