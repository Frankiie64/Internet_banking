using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Internet_banking.Infrastructure.Persistence.Migrations
{
    public partial class ChangesOnTableTypeAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_products_typeAccount_IdTypeAcount",
                table: "products");

            migrationBuilder.RenameColumn(
                name: "IdTypeAcount",
                table: "products",
                newName: "IdAccount");

            migrationBuilder.RenameIndex(
                name: "IX_products_IdTypeAcount",
                table: "products",
                newName: "IX_products_IdAccount");

            migrationBuilder.AddColumn<DateTime>(
                name: "Creadted",
                table: "typeAccount",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "typeAccount",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "typeAccount",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "typeAccount",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_products_typeAccount_IdAccount",
                table: "products",
                column: "IdAccount",
                principalTable: "typeAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_products_typeAccount_IdAccount",
                table: "products");

            migrationBuilder.DropColumn(
                name: "Creadted",
                table: "typeAccount");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "typeAccount");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "typeAccount");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "typeAccount");

            migrationBuilder.RenameColumn(
                name: "IdAccount",
                table: "products",
                newName: "IdTypeAcount");

            migrationBuilder.RenameIndex(
                name: "IX_products_IdAccount",
                table: "products",
                newName: "IX_products_IdTypeAcount");

            migrationBuilder.AddForeignKey(
                name: "FK_products_typeAccount_IdTypeAcount",
                table: "products",
                column: "IdTypeAcount",
                principalTable: "typeAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
