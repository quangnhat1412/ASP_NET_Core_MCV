using Microsoft.EntityFrameworkCore.Migrations;

namespace Demo_Etity_Frameword_Core.Migrations
{
    public partial class CreateTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: false),
                    CategoryID = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_products_categories_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "categories",
                columns: new[] { "Id", "DisplayOrder", "Name" },
                values: new object[,]
                {
                    { 1, 1, "Điện thoại" },
                    { 2, 2, "Máy tính bảng" },
                    { 3, 3, "Laptop" },
                    { 4, 4, "Phụ Kiện" }
                });

            migrationBuilder.InsertData(
                table: "products",
                columns: new[] { "Id", "CategoryID", "Description", "ImageUrl", "Name", "Price" },
                values: new object[,]
                {
                    { 1, 1, null, null, "IPhone 14Pro", 900.0 },
                    { 4, 1, null, null, "IPhone 13Pro", 700.0 },
                    { 6, 1, null, null, "IPhone 12Pro", 600.0 },
                    { 5, 2, null, null, "IPad Pro M1", 650.0 },
                    { 2, 3, null, null, "Macbook Air M1", 1500.0 },
                    { 3, 4, null, null, "Apple Watch Utra 14", 500.0 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_products_CategoryID",
                table: "products",
                column: "CategoryID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "products");

            migrationBuilder.DropTable(
                name: "categories");
        }
    }
}
