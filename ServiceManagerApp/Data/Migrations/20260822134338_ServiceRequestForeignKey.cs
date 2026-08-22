using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceManagerApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class ServiceRequestForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_ServiceRequests_ServiceRequestId",
                table: "Services");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_ServiceRequests_ServiceRequestId",
                table: "Services",
                column: "ServiceRequestId",
                principalTable: "ServiceRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_ServiceRequests_ServiceRequestId",
                table: "Services");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_ServiceRequests_ServiceRequestId",
                table: "Services",
                column: "ServiceRequestId",
                principalTable: "ServiceRequests",
                principalColumn: "Id");
        }
    }
}
