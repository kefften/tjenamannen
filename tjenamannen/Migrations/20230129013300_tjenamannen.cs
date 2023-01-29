using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace tjenamannen.Migrations
{
    /// <inheritdoc />
    public partial class tjenamannen : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ordklasser",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ordklasser", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "Words",
                columns: table => new
                {
                    WordId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OrdklassName = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Words", x => x.WordId);
                    table.ForeignKey(
                        name: "FK_Words_Ordklasser_OrdklassName",
                        column: x => x.OrdklassName,
                        principalTable: "Ordklasser",
                        principalColumn: "Name");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Words_OrdklassName",
                table: "Words",
                column: "OrdklassName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Words");

            migrationBuilder.DropTable(
                name: "Ordklasser");
        }
    }
}
