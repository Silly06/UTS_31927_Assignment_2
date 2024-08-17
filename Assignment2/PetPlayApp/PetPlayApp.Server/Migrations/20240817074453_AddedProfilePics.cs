using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetPlayApp.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddedProfilePics : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ProfilePictureData",
                table: "Users",
                type: "BLOB",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfilePictureData",
                table: "Users");
        }
    }
}
