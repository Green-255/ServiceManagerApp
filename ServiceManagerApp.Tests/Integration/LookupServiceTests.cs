using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.Sqlite;
using ServiceManagerApp.Data;
using ServiceManagerApp.Models.Entities;
using ServiceManagerApp.Models.Enums;
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


        [Fact]
        public async Task PopulateJobRoleDropDownAsync_ExistingJobRoles_ReturnsItems()
        {
            // Arrange
            await using var connection = new SqliteConnection("Data Source=:memory:");

            await connection.OpenAsync();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;

            await using var context = new ApplicationDbContext(options);

            await context.Database.EnsureCreatedAsync();

            context.JobRoles.AddRange(
                new JobRole { Name = "Plumber" },
                new JobRole { Name = "Electrician" },
                new JobRole { Name = "Jonas" },
                new JobRole { Name = "Andrella" }
            );

            await context.SaveChangesAsync();

            var lookupService = new LookupService(context);

            // Act
            var result = await lookupService.PopulateJobRoleDropDownAsync();

            // Assert
            Assert.Equal(4, result.Count());

            Assert.Contains(result,
                item => item.Text == "Plumber");
            Assert.Contains(result,
                item => item.Text == "Electrician");
            Assert.Contains(result,
                item => item.Text == "Jonas");
            Assert.Contains(result,
                item => item.Text == "Andrella");
        }


        [Fact]
        public async Task PopulateServicesFromRequests_EveryTypeJobRoles_Returns()
        {
            // Arrange
            await using var connection = new SqliteConnection("Data Source=:memory:");

            await connection.OpenAsync();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;

            await using var context = new ApplicationDbContext(options);

            await context.Database.EnsureCreatedAsync();

            context.Services.AddRange(
                new Service
                {
                    Status = ServiceStatus.Scheduled,
                },
                new Service
                {
                    Status = ServiceStatus.InProgress,
                },
                new Service
                {
                    Status = ServiceStatus.Draft,
                },
                new Service
                {
                    Status = ServiceStatus.Completed,
                },
                new Service
                {
                    Status = ServiceStatus.Cancelled,
                },
                new Service
                {
                    Status = ServiceStatus.NeedsReview,
                },
                new Service
                {
                    Status = ServiceStatus.Draft,
                }
            );

            await context.SaveChangesAsync();

            var lookupService = new LookupService( context );

            // Act
            var result = await lookupService.PopulateServicesFromRequests();

            // Assert
            Assert.Equal(3, result.Count());

            Assert.Contains(result,
                service => service.Status == ServiceStatus.Draft);
            Assert.Contains(result,
                service => service.Status == ServiceStatus.Draft);
            Assert.Contains(result,
                service => service.Status == ServiceStatus.NeedsReview);
        }
    }
}
