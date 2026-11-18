using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.Sqlite;
using ServiceManagerApp.Data;
using ServiceManagerApp.Models.Entities;
using ServiceManagerApp.Services;
using Microsoft.EntityFrameworkCore;

namespace ServiceManagerApp.Tests.Integration
{
    public class LookupServiceTests
    {
        [Fact]
        public async Task PopulateDepartmentDropDownAsync_ExistingDepartments_ReturnsItems()
        {
            // Arrange
            await using var connection = new SqliteConnection("Data Source=:memory:");

            await connection.OpenAsync();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;

            await using var context = new ApplicationDbContext(options);

            await context.Database.EnsureCreatedAsync();

            context.Departments.AddRange(
                new Department { Name = "Plumbing" },
                new Department { Name = "Electrical" }
            );

            await context.SaveChangesAsync();

            var lookupService = new LookupService(context);

            // Act
            var result = await lookupService.PopulateDepartmentDropDownAsync();

            // Assert
            Assert.Equal(2, result.Count());

            Assert.Contains(result,
                item => item.Text == "Plumbing");
            Assert.Contains(result,
                item => item.Text == "Electrical");
        }
    }
}
