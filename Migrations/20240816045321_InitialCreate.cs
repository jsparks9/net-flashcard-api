using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quiz_API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "app_user",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    full_name = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    date_of_birth = table.Column<DateTime>(type: "date", nullable: true),
                    registration_date = table.Column<DateTime>(type: "datetime", nullable: false),
                    last_login = table.Column<DateTime>(type: "datetime", nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_app_user", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "quiz_card",
                columns: table => new
                {
                    card_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    quiz_text = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    answers = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    image = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_quiz_card", x => x.card_id);
                });

            migrationBuilder.CreateTable(
                name: "quiz_deck",
                columns: table => new
                {
                    deck_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    deck_name = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_quiz_deck", x => x.deck_id);
                    table.ForeignKey(
                        name: "FK_quiz_deck_app_user_user_id",
                        column: x => x.user_id,
                        principalTable: "app_user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_auth",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    user_pwd = table.Column<string>(type: "nvarchar(128)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_auth", x => x.user_id);
                    table.ForeignKey(
                        name: "FK_user_auth_app_user_user_id",
                        column: x => x.user_id,
                        principalTable: "app_user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "deck_cards",
                columns: table => new
                {
                    deck_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    card_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    order_index = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deck_cards", x => new { x.deck_id, x.card_id });
                    table.ForeignKey(
                        name: "FK_deck_cards_quiz_card_card_id",
                        column: x => x.card_id,
                        principalTable: "quiz_card",
                        principalColumn: "card_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_deck_cards_quiz_deck_deck_id",
                        column: x => x.deck_id,
                        principalTable: "quiz_deck",
                        principalColumn: "deck_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_deck_cards_card_id",
                table: "deck_cards",
                column: "card_id");

            migrationBuilder.CreateIndex(
                name: "IX_deck_cards_deck_id_order_index",
                table: "deck_cards",
                columns: new[] { "deck_id", "order_index" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_quiz_deck_user_id",
                table: "quiz_deck",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "deck_cards");

            migrationBuilder.DropTable(
                name: "user_auth");

            migrationBuilder.DropTable(
                name: "quiz_card");

            migrationBuilder.DropTable(
                name: "quiz_deck");

            migrationBuilder.DropTable(
                name: "app_user");
        }
    }
}
