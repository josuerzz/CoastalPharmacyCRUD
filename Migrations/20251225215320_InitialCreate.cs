using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoastalPharmacyCRUD.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CDL_Identifiers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    Set = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ElementNumber = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Use = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CDL_Identifiers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SYS_Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    Email = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    RoleIdentifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SYS_Users_CDL_Identifiers_RoleIdentifierId",
                        column: x => x.RoleIdentifierId,
                        principalTable: "CDL_Identifiers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OBJ_Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    CreateUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OBJ_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OBJ_Products_CDL_Identifiers_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "CDL_Identifiers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OBJ_Products_CDL_Identifiers_SubCategoryId",
                        column: x => x.SubCategoryId,
                        principalTable: "CDL_Identifiers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OBJ_Products_SYS_Users_CreateUserId",
                        column: x => x.CreateUserId,
                        principalTable: "SYS_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SYS_Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    NumberTransaction = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProcessId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EntityName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SYS_Transactions_CDL_Identifiers_ProcessId",
                        column: x => x.ProcessId,
                        principalTable: "CDL_Identifiers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SYS_Transactions_SYS_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "SYS_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CDL_Identifiers_Code",
                table: "CDL_Identifiers",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OBJ_Products_CategoryId",
                table: "OBJ_Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_OBJ_Products_CreateUserId",
                table: "OBJ_Products",
                column: "CreateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_OBJ_Products_SubCategoryId",
                table: "OBJ_Products",
                column: "SubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_SYS_Transactions_ProcessId",
                table: "SYS_Transactions",
                column: "ProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_SYS_Transactions_UserId",
                table: "SYS_Transactions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SYS_Users_Email",
                table: "SYS_Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SYS_Users_RoleIdentifierId",
                table: "SYS_Users",
                column: "RoleIdentifierId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OBJ_Products");

            migrationBuilder.DropTable(
                name: "SYS_Transactions");

            migrationBuilder.DropTable(
                name: "SYS_Users");

            migrationBuilder.DropTable(
                name: "CDL_Identifiers");
        }
    }
}
