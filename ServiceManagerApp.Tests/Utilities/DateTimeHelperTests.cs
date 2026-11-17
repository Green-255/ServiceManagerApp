using Xunit;
using ServiceManagerApp.Services.Utilities;

namespace ServiceManagerApp.Tests.Utilities
{
    public class DateTimeHelperTests
    {
        [Theory]
        [InlineData(2, 30, 150)]
        [InlineData(0, 45, 45)]
        [InlineData(3, 0, 180)]
        [InlineData(null, 30, 30)]
        [InlineData(2, null, 120)]
        [InlineData(null, null, 0)]
        public void CalculateDuration_ValidInput_ReturnsExpectedMinutes(
            int? hours, int? minutes, int expectedMinutes)
        {
            // Arrange
            TimeSpan expected = TimeSpan.FromMinutes(expectedMinutes);

            // Act
            TimeSpan actual = DateTimeHelper.CalculateDuration(hours, minutes);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(-1, 0)]
        [InlineData(0, -1)]
        [InlineData(-1, -1)]
        public void CalculateDuration_NegativeTime_ThrowsException(
            int hours, int minutes)
        {
            // Assert
            Assert.Throws<Exception>(
                () => DateTimeHelper.CalculateDuration(hours, minutes)
            );
        }



        [Theory]
        [InlineData(2, 30, 150)]
        [InlineData(0, 0, 0)]
        [InlineData(-1, -1, 0)]
        public void GetMinutesFromTimeSpan_ValidInput_ReturnsExpectedMinutes(
            int hours, int minutes, int expectedMinutes)
        {
            TimeSpan timeSpan;
            int actual;

            if(hours < 0 || minutes < 0)
            {
                actual = DateTimeHelper.GetMinutesFromTimeSpan(null);
            }
            else
            {
                timeSpan = new TimeSpan(hours, minutes, 0);
                actual = DateTimeHelper.GetMinutesFromTimeSpan(timeSpan);
            }

            Assert.Equal(expectedMinutes, actual);
        }
    }
}
