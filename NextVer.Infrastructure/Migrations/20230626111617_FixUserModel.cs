using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NextVer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixUserModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RegistrationTokenGeneratedTime",
                table: "Users",
                newName: "ConfirmationTokenGeneratedTime");

            migrationBuilder.RenameColumn(
                name: "RegistrationToken",
                table: "Users",
                newName: "ConfirmationToken");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ConfirmationTokenGeneratedTime",
                table: "Users",
                newName: "RegistrationTokenGeneratedTime");

            migrationBuilder.RenameColumn(
                name: "ConfirmationToken",
                table: "Users",
                newName: "RegistrationToken");
        }
    }
}
