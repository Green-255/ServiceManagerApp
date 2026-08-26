using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceManagerApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixJobRoleEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobRole_Departament_DepartamentId",
                table: "JobRole");

            migrationBuilder.DropForeignKey(
                name: "FK_Workers_Departament_DepartamentId",
                table: "Workers");

            migrationBuilder.DropForeignKey(
                name: "FK_Workers_JobRole_JobRoleId",
                table: "Workers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobRole",
                table: "JobRole");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Departament",
                table: "Departament");

            migrationBuilder.RenameTable(
                name: "JobRole",
                newName: "JobRoles");

            migrationBuilder.RenameTable(
                name: "Departament",
                newName: "Departaments");

            migrationBuilder.RenameColumn(
                name: "Descriptions",
                table: "JobRoles",
                newName: "Description");

            migrationBuilder.RenameIndex(
                name: "IX_JobRole_DepartamentId",
                table: "JobRoles",
                newName: "IX_JobRoles_DepartamentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobRoles",
                table: "JobRoles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Departaments",
                table: "Departaments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_JobRoles_Departaments_DepartamentId",
                table: "JobRoles",
                column: "DepartamentId",
                principalTable: "Departaments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Workers_Departaments_DepartamentId",
                table: "Workers",
                column: "DepartamentId",
                principalTable: "Departaments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Workers_JobRoles_JobRoleId",
                table: "Workers",
                column: "JobRoleId",
                principalTable: "JobRoles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobRoles_Departaments_DepartamentId",
                table: "JobRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_Workers_Departaments_DepartamentId",
                table: "Workers");

            migrationBuilder.DropForeignKey(
                name: "FK_Workers_JobRoles_JobRoleId",
                table: "Workers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobRoles",
                table: "JobRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Departaments",
                table: "Departaments");

            migrationBuilder.RenameTable(
                name: "JobRoles",
                newName: "JobRole");

            migrationBuilder.RenameTable(
                name: "Departaments",
                newName: "Departament");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "JobRole",
                newName: "Descriptions");

            migrationBuilder.RenameIndex(
                name: "IX_JobRoles_DepartamentId",
                table: "JobRole",
                newName: "IX_JobRole_DepartamentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobRole",
                table: "JobRole",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Departament",
                table: "Departament",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_JobRole_Departament_DepartamentId",
                table: "JobRole",
                column: "DepartamentId",
                principalTable: "Departament",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Workers_Departament_DepartamentId",
                table: "Workers",
                column: "DepartamentId",
                principalTable: "Departament",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Workers_JobRole_JobRoleId",
                table: "Workers",
                column: "JobRoleId",
                principalTable: "JobRole",
                principalColumn: "Id");
        }
    }
}
