using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Authentication_2025.Migrations
{
    /// <inheritdoc />
    public partial class useroffruit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserOfFruit",
                table: "fruits",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserOfFruit",
                table: "fruits");
        }
    }
}
