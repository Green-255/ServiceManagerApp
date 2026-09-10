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

            migrationBuilder.AddForeignKey(
                name: "FK_JobRoles_Departments_DepartmentId",
                table: "JobRoles",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobRoles_Departments_DepartmentId",
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
