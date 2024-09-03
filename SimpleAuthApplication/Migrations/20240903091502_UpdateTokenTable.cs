using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleAuthApplication.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTokenTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccessToken",
                table: "Tokens");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccessToken",
                table: "Tokens",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
