using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace myteammanager.Migrations
{
    /// <inheritdoc />
    public partial class changepasswordname : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PassWordHash",
                table: "TeamMembers",
                newName: "PasswordHash");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "TeamMembers",
                newName: "PassWordHash");
        }
    }
}
