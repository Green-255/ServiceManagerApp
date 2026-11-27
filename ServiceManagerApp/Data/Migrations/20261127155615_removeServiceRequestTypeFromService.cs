using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceManagerApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class removeServiceRequestTypeFromService : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ServiceRequestType",
                table: "Services");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ServiceRequestType",
                table: "Services",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
