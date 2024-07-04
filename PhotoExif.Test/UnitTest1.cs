using Xunit;
using PhotoExif;
using System.Diagnostics.Contracts;

namespace PhotoExif.Test
{
    public class PhotoExifTest
    {
        [Fact]
        public void PhotoExif_FormatShutterSpeed_ReturnsConsoleErrorFileNotFound()
        {
            // Arrange
            var filePath = "";
            // Act
            using var consoleOutput = new StringWriter();
            Console.SetOut(consoleOutput);

            var result = Program.GetImagesFromPath(filePath);

            // Assert
            Assert.Contains("File not found", consoleOutput.ToString());
        }
    }
}