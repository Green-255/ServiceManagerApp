using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceManagerApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AuthorizationForWorkerAndServiceRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Workers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerUserId",
                table: "ServiceRequests",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Workers_ApplicationUserId",
                table: "Workers",
                column: "ApplicationUserId",
                unique: true,
                filter: "[ApplicationUserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequests_CustomerUserId",
                table: "ServiceRequests",
                column: "CustomerUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceRequests_AspNetUsers_CustomerUserId",
                table: "ServiceRequests",
                column: "CustomerUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Workers_AspNetUsers_ApplicationUserId",
                table: "Workers",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceRequests_AspNetUsers_CustomerUserId",
                table: "ServiceRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_Workers_AspNetUsers_ApplicationUserId",
                table: "Workers");

            migrationBuilder.DropIndex(
                name: "IX_Workers_ApplicationUserId",
                table: "Workers");

            migrationBuilder.DropIndex(
                name: "IX_ServiceRequests_CustomerUserId",
                table: "ServiceRequests");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "CustomerUserId",
                table: "ServiceRequests");
        }
    }
}
