using Microsoft.EntityFrameworkCore.Migrations;

namespace Internet_banking.Infrastructure.Persistence.Migrations
{
    public partial class DeleteElementsOfTableTrasational : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CountProduct",
                table: "Transactional");

            migrationBuilder.DropColumn(
                name: "UserActives",
                table: "Transactional");

            migrationBuilder.DropColumn(
                name: "UserInactives",
                table: "Transactional");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CountProduct",
                table: "Transactional",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserActives",
                table: "Transactional",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserInactives",
                table: "Transactional",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
