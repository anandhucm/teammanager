using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace myteammanager.Migrations
{
    /// <inheritdoc />
    public partial class removedMemberId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MemberId",
                table: "Photos");
        }

        /// <inheritdoc />
        /// 
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MemberId",
                table: "Photos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
