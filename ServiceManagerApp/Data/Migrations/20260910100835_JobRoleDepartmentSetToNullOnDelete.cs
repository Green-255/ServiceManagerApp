using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceManagerApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class JobRoleDepartmentSetToNullOnDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobRoles_Departments_DepartmentId",
                table: "JobRoles");

            migrationBuilder.AddColumn<int>(
                name: "DepartmentId1",
                table: "JobRoles",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_JobRoles_DepartmentId1",
                table: "JobRoles",
                column: "DepartmentId1");

            migrationBuilder.AddForeignKey(
                name: "FK_JobRoles_Departments_DepartmentId",
                table: "JobRoles",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_JobRoles_Departments_DepartmentId1",
                table: "JobRoles",
                column: "DepartmentId1",
                principalTable: "Departments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobRoles_Departments_DepartmentId",
                table: "JobRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_JobRoles_Departments_DepartmentId1",
                table: "JobRoles");

            migrationBuilder.DropIndex(
                name: "IX_JobRoles_DepartmentId1",
                table: "JobRoles");

            migrationBuilder.DropColumn(
                name: "DepartmentId1",
                table: "JobRoles");

            migrationBuilder.AddForeignKey(
                name: "FK_JobRoles_Departments_DepartmentId",
                table: "JobRoles",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id");
        }
    }
}
