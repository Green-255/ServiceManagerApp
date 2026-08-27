using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceManagerApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class Worker : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Workers_Services_ServiceId",
                table: "Workers");

            migrationBuilder.DropIndex(
                name: "IX_Workers_ServiceId",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "Occupation",
                table: "Workers");

            migrationBuilder.RenameColumn(
                name: "WorkSector",
                table: "Workers",
                newName: "AvailabilityStatus");

            migrationBuilder.RenameColumn(
                name: "ServiceId",
                table: "Workers",
                newName: "SkillLevel");

            migrationBuilder.RenameColumn(
                name: "ReferenceCode",
                table: "Workers",
                newName: "ReferenceNumber");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Workers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DepartamentId",
                table: "Workers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "JobRoleId",
                table: "Workers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SkillLevel",
                table: "Services",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Departament",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departament", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServiceWorker",
                columns: table => new
                {
                    ServicesId = table.Column<int>(type: "int", nullable: false),
                    WorkersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceWorker", x => new { x.ServicesId, x.WorkersId });
                    table.ForeignKey(
                        name: "FK_ServiceWorker_Services_ServicesId",
                        column: x => x.ServicesId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServiceWorker_Workers_WorkersId",
                        column: x => x.WorkersId,
                        principalTable: "Workers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JobRole",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descriptions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DepartamentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobRole_Departament_DepartamentId",
                        column: x => x.DepartamentId,
                        principalTable: "Departament",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Workers_DepartamentId",
                table: "Workers",
                column: "DepartamentId");

            migrationBuilder.CreateIndex(
                name: "IX_Workers_JobRoleId",
                table: "Workers",
                column: "JobRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_JobRole_DepartamentId",
                table: "JobRole",
                column: "DepartamentId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceWorker_WorkersId",
                table: "ServiceWorker",
                column: "WorkersId");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Workers_Departament_DepartamentId",
                table: "Workers");

            migrationBuilder.DropForeignKey(
                name: "FK_Workers_JobRole_JobRoleId",
                table: "Workers");

            migrationBuilder.DropTable(
                name: "JobRole");

            migrationBuilder.DropTable(
                name: "ServiceWorker");

            migrationBuilder.DropTable(
                name: "Departament");

            migrationBuilder.DropIndex(
                name: "IX_Workers_DepartamentId",
                table: "Workers");

            migrationBuilder.DropIndex(
                name: "IX_Workers_JobRoleId",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "DepartamentId",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "JobRoleId",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "SkillLevel",
                table: "Services");

            migrationBuilder.RenameColumn(
                name: "SkillLevel",
                table: "Workers",
                newName: "ServiceId");

            migrationBuilder.RenameColumn(
                name: "ReferenceNumber",
                table: "Workers",
                newName: "ReferenceCode");

            migrationBuilder.RenameColumn(
                name: "AvailabilityStatus",
                table: "Workers",
                newName: "WorkSector");

            migrationBuilder.AlterColumn<int>(
                name: "PhoneNumber",
                table: "Workers",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Occupation",
                table: "Workers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Workers_ServiceId",
                table: "Workers",
                column: "ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Workers_Services_ServiceId",
                table: "Workers",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id");
        }
    }
}
