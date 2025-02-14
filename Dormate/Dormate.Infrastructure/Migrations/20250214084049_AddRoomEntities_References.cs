using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dormate.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRoomEntities_References : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FavouriteRoomId",
                table: "Users",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReviewId",
                table: "Users",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoomRegisterId",
                table: "Users",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FavouriteRoomId",
                table: "Rooms",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReviewId",
                table: "Rooms",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoomRegisterId",
                table: "Rooms",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FavouriteRooms",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoomId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavouriteRooms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoomId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rate = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoomRegisters",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoomId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomRegisters", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_FavouriteRoomId",
                table: "Users",
                column: "FavouriteRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ReviewId",
                table: "Users",
                column: "ReviewId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoomRegisterId",
                table: "Users",
                column: "RoomRegisterId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_FavouriteRoomId",
                table: "Rooms",
                column: "FavouriteRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_ReviewId",
                table: "Rooms",
                column: "ReviewId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_RoomRegisterId",
                table: "Rooms",
                column: "RoomRegisterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_FavouriteRooms_FavouriteRoomId",
                table: "Rooms",
                column: "FavouriteRoomId",
                principalTable: "FavouriteRooms",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Reviews_ReviewId",
                table: "Rooms",
                column: "ReviewId",
                principalTable: "Reviews",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_RoomRegisters_RoomRegisterId",
                table: "Rooms",
                column: "RoomRegisterId",
                principalTable: "RoomRegisters",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_FavouriteRooms_FavouriteRoomId",
                table: "Users",
                column: "FavouriteRoomId",
                principalTable: "FavouriteRooms",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Reviews_ReviewId",
                table: "Users",
                column: "ReviewId",
                principalTable: "Reviews",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_RoomRegisters_RoomRegisterId",
                table: "Users",
                column: "RoomRegisterId",
                principalTable: "RoomRegisters",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_FavouriteRooms_FavouriteRoomId",
                table: "Rooms");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Reviews_ReviewId",
                table: "Rooms");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_RoomRegisters_RoomRegisterId",
                table: "Rooms");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_FavouriteRooms_FavouriteRoomId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Reviews_ReviewId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_RoomRegisters_RoomRegisterId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "FavouriteRooms");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "RoomRegisters");

            migrationBuilder.DropIndex(
                name: "IX_Users_FavouriteRoomId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_ReviewId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_RoomRegisterId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_FavouriteRoomId",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_ReviewId",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_RoomRegisterId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "FavouriteRoomId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ReviewId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RoomRegisterId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FavouriteRoomId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "ReviewId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "RoomRegisterId",
                table: "Rooms");
        }
    }
}
