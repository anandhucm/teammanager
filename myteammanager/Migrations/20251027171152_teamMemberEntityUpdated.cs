using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace myteammanager.Migrations
{
    /// <inheritdoc />
    public partial class teamMemberEntityUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "PassWordHash",
                table: "TeamMembers",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: Array.Empty<byte>());

            migrationBuilder.AddColumn<byte[]>(
                name: "PasswordSalt",
                table: "TeamMembers",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: Array.Empty<byte>());
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PassWordHash",
                table: "TeamMembers");

            migrationBuilder.DropColumn(
                name: "PasswordSalt",
                table: "TeamMembers");
        }
    }
}
