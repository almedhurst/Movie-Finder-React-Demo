using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieFinder.Infrastructure.Data.Migrations
{
    public partial class AddedGivenNameToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GivenName",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GivenName",
                table: "AspNetUsers");
        }
    }
}
