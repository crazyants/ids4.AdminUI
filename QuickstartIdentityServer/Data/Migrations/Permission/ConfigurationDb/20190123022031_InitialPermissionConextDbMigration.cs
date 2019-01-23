using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace QuickstartIdentityServer.Data.Migrations.Permission.ConfigurationDb
{
    public partial class InitialPermissionConextDbMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "App",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Ts = table.Column<long>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreateId = table.Column<int>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    UpdateId = table.Column<int>(nullable: false),
                    UpdateTime = table.Column<DateTime>(nullable: false),
                    Code = table.Column<string>(maxLength: 8, nullable: true),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    Order = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_App", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Module",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Ts = table.Column<long>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreateId = table.Column<int>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    UpdateId = table.Column<int>(nullable: false),
                    UpdateTime = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(maxLength: 30, nullable: true),
                    Code = table.Column<string>(nullable: true),
                    Order = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Module", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Permission",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Ts = table.Column<long>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreateId = table.Column<int>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    UpdateId = table.Column<int>(nullable: false),
                    UpdateTime = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(maxLength: 30, nullable: true),
                    ControllerName = table.Column<string>(maxLength: 20, nullable: true),
                    ActionName = table.Column<string>(maxLength: 20, nullable: true),
                    Url = table.Column<string>(maxLength: 50, nullable: true),
                    ModuleId = table.Column<int>(nullable: false),
                    Order = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permission", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Ts = table.Column<long>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreateId = table.Column<int>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    UpdateId = table.Column<int>(nullable: false),
                    UpdateTime = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoleAppAdmin",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(nullable: true),
                    RoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleAppAdmin", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RolePermissionMap",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(nullable: true),
                    PermissionId = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissionMap", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Ts = table.Column<long>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreateId = table.Column<int>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    UpdateId = table.Column<int>(nullable: false),
                    UpdateTime = table.Column<DateTime>(nullable: false),
                    Account = table.Column<string>(maxLength: 20, nullable: true),
                    Name = table.Column<string>(maxLength: 20, nullable: true),
                    Pwd = table.Column<string>(maxLength: 32, nullable: true),
                    IsSystemAdmin = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserRoleMap",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoleMap", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRoleMap_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoleMap_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Module_Code",
                table: "Module",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_Permission_ModuleId",
                table: "Permission",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_User_Account",
                table: "User",
                column: "Account");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoleMap_RoleId",
                table: "UserRoleMap",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoleMap_UserId",
                table: "UserRoleMap",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "App");

            migrationBuilder.DropTable(
                name: "Module");

            migrationBuilder.DropTable(
                name: "Permission");

            migrationBuilder.DropTable(
                name: "RoleAppAdmin");

            migrationBuilder.DropTable(
                name: "RolePermissionMap");

            migrationBuilder.DropTable(
                name: "UserRoleMap");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
