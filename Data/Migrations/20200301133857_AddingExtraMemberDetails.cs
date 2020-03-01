using Microsoft.EntityFrameworkCore.Migrations;

namespace UserRegProfile.Data.Migrations
{
    public partial class AddingExtraMemberDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "First_Name",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Last_Name",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Player1_First_Name",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Player1_Last_Name",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Player1_Team",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Player2_First_Name",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Player2_Last_Name",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Player2_Team",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "First_Name",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Last_Name",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Player1_First_Name",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Player1_Last_Name",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Player1_Team",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Player2_First_Name",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Player2_Last_Name",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Player2_Team",
                table: "AspNetUsers");
        }
    }
}
