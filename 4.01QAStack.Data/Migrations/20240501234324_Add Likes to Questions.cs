using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _4._01QAStack.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddLikestoQuestions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Likes",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Likes",
                table: "Questions");
        }
    }
}
