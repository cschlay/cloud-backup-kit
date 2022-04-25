using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OffsiteWorker.Migrations
{
    public partial class AddStatusAndDownloadLink : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SignedDownloadUrl",
                table: "ObjectStorageFiles",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "ObjectStorageFiles",
                type: "varchar(10)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SignedDownloadUrl",
                table: "ObjectStorageFiles");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "ObjectStorageFiles");
        }
    }
}
