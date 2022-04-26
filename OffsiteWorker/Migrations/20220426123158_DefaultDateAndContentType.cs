using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OffsiteWorker.Migrations
{
    public partial class DefaultDateAndContentType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "ObjectStorageFiles",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "ObjectStorageFiles",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "ObjectStorageFiles");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "ObjectStorageFiles",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "now()");
        }
    }
}
