using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Surveys",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OwnerCode = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Surveys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AnwserModules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SurveyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnwserModules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnwserModules_Surveys_SurveyId",
                        column: x => x.SurveyId,
                        principalTable: "Surveys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Components",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Required = table.Column<bool>(type: "bit", nullable: false),
                    Question = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SurveyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Components", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Components_Surveys_SurveyId",
                        column: x => x.SurveyId,
                        principalTable: "Surveys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Anwsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnwserText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompId = table.Column<int>(type: "int", nullable: false),
                    AnwserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Anwsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Anwsers_AnwserModules_AnwserId",
                        column: x => x.AnwserId,
                        principalTable: "AnwserModules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Anwsers_Components_CompId",
                        column: x => x.CompId,
                        principalTable: "Components",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CompModules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompSingleId = table.Column<int>(type: "int", nullable: true),
                    CompMultiId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompModules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompModules_Components_CompMultiId",
                        column: x => x.CompMultiId,
                        principalTable: "Components",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CompModules_Components_CompSingleId",
                        column: x => x.CompSingleId,
                        principalTable: "Components",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnwserModules_SurveyId",
                table: "AnwserModules",
                column: "SurveyId");

            migrationBuilder.CreateIndex(
                name: "IX_Anwsers_AnwserId",
                table: "Anwsers",
                column: "AnwserId");

            migrationBuilder.CreateIndex(
                name: "IX_Anwsers_CompId",
                table: "Anwsers",
                column: "CompId");

            migrationBuilder.CreateIndex(
                name: "IX_CompModules_CompMultiId",
                table: "CompModules",
                column: "CompMultiId");

            migrationBuilder.CreateIndex(
                name: "IX_CompModules_CompSingleId",
                table: "CompModules",
                column: "CompSingleId");

            migrationBuilder.CreateIndex(
                name: "IX_Components_SurveyId",
                table: "Components",
                column: "SurveyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Anwsers");

            migrationBuilder.DropTable(
                name: "CompModules");

            migrationBuilder.DropTable(
                name: "AnwserModules");

            migrationBuilder.DropTable(
                name: "Components");

            migrationBuilder.DropTable(
                name: "Surveys");
        }
    }
}
