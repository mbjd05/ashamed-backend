using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AshamedApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class final : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MqttMessages_Snapshots_SnapshotDtoId",
                table: "MqttMessages");

            migrationBuilder.DropIndex(
                name: "IX_MqttMessages_SnapshotDtoId",
                table: "MqttMessages");

            migrationBuilder.DropColumn(
                name: "MqttMessageId",
                table: "Snapshots");

            migrationBuilder.DropColumn(
                name: "SnapshotDtoId",
                table: "MqttMessages");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MqttMessageId",
                table: "Snapshots",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SnapshotDtoId",
                table: "MqttMessages",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MqttMessages_SnapshotDtoId",
                table: "MqttMessages",
                column: "SnapshotDtoId");

            migrationBuilder.AddForeignKey(
                name: "FK_MqttMessages_Snapshots_SnapshotDtoId",
                table: "MqttMessages",
                column: "SnapshotDtoId",
                principalTable: "Snapshots",
                principalColumn: "Id");
        }
    }
}
