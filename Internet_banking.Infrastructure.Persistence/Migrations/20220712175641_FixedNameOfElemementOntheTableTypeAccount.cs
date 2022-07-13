using Microsoft.EntityFrameworkCore.Migrations;

namespace Internet_banking.Infrastructure.Persistence.Migrations
{
    public partial class FixedNameOfElemementOntheTableTypeAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Titlte",
                table: "typeAccount",
                newName: "Title");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "typeAccount",
                newName: "Titlte");
        }
    }
}
