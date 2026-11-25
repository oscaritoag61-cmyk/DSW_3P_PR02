using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DSW_3P_PR02.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Personajes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Videojuego = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Clase = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Nivel = table.Column<int>(type: "int", nullable: false),
                    Poder = table.Column<int>(type: "int", nullable: false),
                    Disponible = table.Column<bool>(type: "bit", nullable: false),
                    ImagenUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Personajes", x => x.Id);
                    table.CheckConstraint("CK_Personajes_Nivel", "[Nivel] BETWEEN 1 AND 100");
                    table.CheckConstraint("CK_Personajes_Poder", "[Poder] > 0");
                });

            migrationBuilder.InsertData(
                table: "Personajes",
                columns: new[] { "Id", "Clase", "Disponible", "ImagenUrl", "Nivel", "Nombre", "Poder", "Videojuego" },
                values: new object[,]
                {
                    { 1, "Guerrero", true, "https://placehold.co/400x400/png?text=Kratos", 90, "Kratos", 9500, "God of War" },
                    { 2, "Brujo", true, "https://placehold.co/400x400/png?text=Geralt", 75, "Geralt de Rivia", 8200, "The Witcher 3" },
                    { 3, "Maga", true, "https://placehold.co/400x400/png?text=Jaina", 60, "Jaina Proudmoore", 7000, "World of Warcraft" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Personajes_Videojuego",
                table: "Personajes",
                column: "Videojuego");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Personajes");
        }
    }
}
