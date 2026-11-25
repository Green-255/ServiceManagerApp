using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceManagerApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class addPriorityForService : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Priority",
                table: "Services",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Priority",
                table: "Services");
        }
    }
}
