using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using ServiceManagerApp.Services.Utilities;

namespace ServiceManagerApp.Tests.Utilities
{
    public class ReferenceNumberGeneratorTests
    {
        [Theory]
        [InlineData("REQ", 1, "2026", "REQ-2026-000001")]
        [InlineData("REQ", 12346578, "2026", "REQ-2026-12346578")]
        [InlineData("WO", 42, "2026", "WO-2026-000042")]
        [InlineData("REQ", 123456, "2025", "REQ-2025-123456")]
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

    }
    // why was this class generated as `internal`?
}
