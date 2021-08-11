using Microsoft.EntityFrameworkCore.Migrations;

namespace MuffiNet.FrontendReact.Data.Migrations
{
    public partial class AddSupportTicketBrand : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Brand",
                table: "SupportTickets",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Brand",
                table: "SupportTickets");
        }
    }
}
