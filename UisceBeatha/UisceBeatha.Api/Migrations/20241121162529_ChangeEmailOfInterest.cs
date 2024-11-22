using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UisceBeatha.Api.Migrations
{
    /// <inheritdoc />
    public partial class ChangeEmailOfInterest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EmailAddres",
                table: "Emails",
                newName: "EmailAddress");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EmailAddress",
                table: "Emails",
                newName: "EmailAddres");
        }
    }
}
