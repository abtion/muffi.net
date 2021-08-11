using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace MuffiNet.FrontendReact.Data.Migrations
{
    public partial class CallStartedTrackingOnSupportTicket : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Created",
                table: "SupportTickets",
                newName: "CreatedAt");

            migrationBuilder.AlterColumn<string>(
                name: "CustomerPhone",
                table: "SupportTickets",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CustomerName",
                table: "SupportTickets",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CustomerEmail",
                table: "SupportTickets",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CallEndedAt",
                table: "SupportTickets",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CallStartedAt",
                table: "SupportTickets",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "TechnicianUserId",
                table: "SupportTickets",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SupportTicket_CallEndedAt_CreatedAt",
                table: "SupportTickets",
                columns: new[] { "CallEndedAt", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_SupportTicket_CreatedAt_CallStartedAt_CallEndedAt",
                table: "SupportTickets",
                columns: new[] { "CreatedAt", "CallStartedAt", "CallEndedAt" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SupportTicket_CallEndedAt_CreatedAt",
                table: "SupportTickets");

            migrationBuilder.DropIndex(
                name: "IX_SupportTicket_CreatedAt_CallStartedAt_CallEndedAt",
                table: "SupportTickets");

            migrationBuilder.DropColumn(
                name: "CallEndedAt",
                table: "SupportTickets");

            migrationBuilder.DropColumn(
                name: "CallStartedAt",
                table: "SupportTickets");

            migrationBuilder.DropColumn(
                name: "TechnicianUserId",
                table: "SupportTickets");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "SupportTickets",
                newName: "Created");

            migrationBuilder.AlterColumn<string>(
                name: "CustomerPhone",
                table: "SupportTickets",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "CustomerName",
                table: "SupportTickets",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "CustomerEmail",
                table: "SupportTickets",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300);
        }
    }
}
