using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoastalPharmacyCRUD.Migrations
{
    /// <inheritdoc />
    public partial class AddParentIdToIdentifiers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ParentId",
                table: "CDL_Identifiers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CDL_Identifiers_ParentId",
                table: "CDL_Identifiers",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_CDL_Identifiers_CDL_Identifiers_ParentId",
                table: "CDL_Identifiers",
                column: "ParentId",
                principalTable: "CDL_Identifiers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CDL_Identifiers_CDL_Identifiers_ParentId",
                table: "CDL_Identifiers");

            migrationBuilder.DropIndex(
                name: "IX_CDL_Identifiers_ParentId",
                table: "CDL_Identifiers");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "CDL_Identifiers");
        }
    }
}
