using Microsoft.EntityFrameworkCore.Migrations;

namespace BiStatApp.Migrations
{
    public partial class AddHeartRateDuration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "Bouts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EndHeartRate",
                table: "Bouts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StartHeartRate",
                table: "Bouts",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Bouts");

            migrationBuilder.DropColumn(
                name: "EndHeartRate",
                table: "Bouts");

            migrationBuilder.DropColumn(
                name: "StartHeartRate",
                table: "Bouts");
        }
    }
}
