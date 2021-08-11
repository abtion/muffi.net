using Microsoft.EntityFrameworkCore.Migrations;

namespace WebAppReact.Data.Migrations
{
    public partial class OssIdAddedOnSupportTicket : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OssId",
                table: "SupportTickets",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OssId",
                table: "SupportTickets");
        }
    }
}
