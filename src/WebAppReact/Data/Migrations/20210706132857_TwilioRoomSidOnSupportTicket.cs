using Microsoft.EntityFrameworkCore.Migrations;

namespace WebAppReact.Data.Migrations
{
    public partial class TwilioRoomSidOnSupportTicket : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TwilioRoomSid",
                table: "SupportTickets",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TwilioRoomSid",
                table: "SupportTickets");
        }
    }
}
