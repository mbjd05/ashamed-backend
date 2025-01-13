using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AshamedApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class snapshots : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Payload",
                table: "MqttMessages",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<int>(
                name: "SnapshotDtoId",
                table: "MqttMessages",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Snapshots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Snapshots", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MqttMessages_SnapshotDtoId",
                table: "MqttMessages",
                column: "SnapshotDtoId");

            migrationBuilder.AddForeignKey(
                name: "FK_MqttMessages_Snapshots_SnapshotDtoId",
                table: "MqttMessages",
                column: "SnapshotDtoId",
                principalTable: "Snapshots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MqttMessages_Snapshots_SnapshotDtoId",
                table: "MqttMessages");

            migrationBuilder.DropTable(
                name: "Snapshots");

            migrationBuilder.DropIndex(
                name: "IX_MqttMessages_SnapshotDtoId",
                table: "MqttMessages");

            migrationBuilder.DropColumn(
                name: "SnapshotDtoId",
                table: "MqttMessages");

            migrationBuilder.AlterColumn<string>(
                name: "Payload",
                table: "MqttMessages",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);
        }
    }
}
