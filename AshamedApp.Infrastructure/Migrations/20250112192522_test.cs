using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AshamedApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MessageIds",
                table: "Snapshots");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MqttMessages_Snapshots_SnapshotDtoId",
                table: "MqttMessages");

            migrationBuilder.DropIndex(
                name: "IX_MqttMessages_SnapshotDtoId",
                table: "MqttMessages");

            migrationBuilder.DropColumn(
                name: "SnapshotDtoId",
                table: "MqttMessages");

            migrationBuilder.AddColumn<string>(
                name: "MessageIds",
                table: "Snapshots",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
