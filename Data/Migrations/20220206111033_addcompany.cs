using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineStore.Data.Migrations
{
    public partial class addcompany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Colors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Colors_CompanyId",
                table: "Colors",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Colors_Companies_CompanyId",
                table: "Colors",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Colors_Companies_CompanyId",
                table: "Colors");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_Colors_CompanyId",
                table: "Colors");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Colors");
        }
    }
}
