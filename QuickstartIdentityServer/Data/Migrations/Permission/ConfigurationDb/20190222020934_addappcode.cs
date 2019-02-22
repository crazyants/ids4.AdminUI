using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace QuickstartIdentityServer.Data.Migrations.Permission.ConfigurationDb
{
    public partial class addappcode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Module_Code",
                table: "Module");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "RolePermissionMap",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "RoleAppAdmin",
                maxLength: 8,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Module",
                maxLength: 8,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AppCode",
                table: "Module",
                maxLength: 8,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RoleModuleMap",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AppCode = table.Column<string>(maxLength: 8, nullable: true),
                    ModuleId = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleModuleMap", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissionMap_Code",
                table: "RolePermissionMap",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissionMap_PermissionId",
                table: "RolePermissionMap",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissionMap_RoleId",
                table: "RolePermissionMap",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleAppAdmin_Code",
                table: "RoleAppAdmin",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_RoleAppAdmin_RoleId",
                table: "RoleAppAdmin",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Module_AppCode",
                table: "Module",
                column: "AppCode");

            migrationBuilder.CreateIndex(
                name: "IX_RoleModuleMap_AppCode",
                table: "RoleModuleMap",
                column: "AppCode");

            migrationBuilder.CreateIndex(
                name: "IX_RoleModuleMap_ModuleId",
                table: "RoleModuleMap",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleModuleMap_RoleId",
                table: "RoleModuleMap",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoleModuleMap");

            migrationBuilder.DropIndex(
                name: "IX_RolePermissionMap_Code",
                table: "RolePermissionMap");

            migrationBuilder.DropIndex(
                name: "IX_RolePermissionMap_PermissionId",
                table: "RolePermissionMap");

            migrationBuilder.DropIndex(
                name: "IX_RolePermissionMap_RoleId",
                table: "RolePermissionMap");

            migrationBuilder.DropIndex(
                name: "IX_RoleAppAdmin_Code",
                table: "RoleAppAdmin");

            migrationBuilder.DropIndex(
                name: "IX_RoleAppAdmin_RoleId",
                table: "RoleAppAdmin");

            migrationBuilder.DropIndex(
                name: "IX_Module_AppCode",
                table: "Module");

            migrationBuilder.DropColumn(
                name: "AppCode",
                table: "Module");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "RolePermissionMap",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "RoleAppAdmin",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 8,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Module",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 8,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Module_Code",
                table: "Module",
                column: "Code");
        }
    }
}
