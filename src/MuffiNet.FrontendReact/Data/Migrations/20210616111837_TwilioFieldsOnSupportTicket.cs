using Microsoft.EntityFrameworkCore.Migrations;

namespace MuffiNet.FrontendReact.Data.Migrations
{
    public partial class TwilioFieldsOnSupportTicket : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TwilioToken",
                table: "SupportTickets",
                newName: "TwilioVideoGrantForTechnicianToken");

            migrationBuilder.AddColumn<string>(
                name: "TwilioRoomName",
                table: "SupportTickets",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TwilioVideoGrantForCustomerToken",
                table: "SupportTickets",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TwilioRoomName",
                table: "SupportTickets");

            migrationBuilder.DropColumn(
                name: "TwilioVideoGrantForCustomerToken",
                table: "SupportTickets");

            migrationBuilder.RenameColumn(
                name: "TwilioVideoGrantForTechnicianToken",
                table: "SupportTickets",
                newName: "TwilioToken");
        }
    }
}
