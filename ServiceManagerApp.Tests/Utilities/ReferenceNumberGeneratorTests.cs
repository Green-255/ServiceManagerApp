using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using ServiceManagerApp.Utilities;

namespace ServiceManagerApp.Tests.Utilities
{
    public class ReferenceNumberGeneratorTests
    {
        [Theory]
        [InlineData("REQ", 12346578, "2026", "REQ-2026-12346578")]
        [InlineData("WIT", 7, "2027", "WIT-2027-000007")]
        [InlineData("WIT", 70, "Plumber", "WIT-Plumber-000070")]
        public void Generate_ValidArguments_ReturnsCorrectReference(
            string tag, int id, string middleTag, string expected)
        {
            // Act
            string actual = ReferenceNumberGenerator.Generate(tag, id, middleTag);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("WO", -42, "2026")]
        [InlineData("WIT", 0, "Plumber")]
        public void Generate_NegativeId_ThrowsException(
            string tag, int id, string? middleTag)
        {
            Assert.Throws<Exception>(
                () => ReferenceNumberGenerator.Generate(tag, id, middleTag)
            );
        }


        [Fact]
        public void GenerateServiceReferenceNumber_ValidArguments_ReturnsExpectedReference()
        {
            string middleTag = DateTime.UtcNow.Year.ToString();
            string actual = ReferenceNumberGenerator.GenerateServiceReferenceNumber(30);

            Assert.Equal($"WO-{middleTag}-000030", actual);
        }


        [Fact]
        public void GenerateServiceRequestReferenceNumber_ValidArguments_ReturnsExpectedReference()
        {
            string middleTag = DateTime.UtcNow.Year.ToString();
            string actual = ReferenceNumberGenerator.GenerateServiceRequestReferenceNumber(50);

            Assert.Equal($"REQ-{middleTag}-000050", actual);
        }



        [Fact]
        public void GenerateWorkerReferenceNumber_ValidArguments_ReturnsExpectedReference()
        {
            string actual = ReferenceNumberGenerator.GenerateWorkerReferenceNumber(70, "Plumber");

            Assert.Equal("WIT-Plumber-000070", actual);
        }

    }
    // why was this class generated as `internal`?
}
