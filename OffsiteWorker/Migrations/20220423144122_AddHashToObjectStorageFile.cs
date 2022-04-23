using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OffsiteWorker.Migrations
{
    public partial class AddHashToObjectStorageFile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Hash",
                table: "ObjectStorageFiles",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Hash",
                table: "ObjectStorageFiles");
        }
    }
}
