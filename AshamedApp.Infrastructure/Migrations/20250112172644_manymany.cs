using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AshamedApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class manymany : Migration
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
                name: "SnapshotDtoId",
                table: "MqttMessages");

            migrationBuilder.AddColumn<string>(
                name: "MessageIds",
                table: "Snapshots",
                type: "TEXT",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.CreateTable(
                name: "SnapshotMqttMessages",
                columns: table => new
                {
                    SnapshotId = table.Column<int>(type: "INTEGER", nullable: false),
                    MqttMessageId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SnapshotMqttMessages", x => new { x.SnapshotId, x.MqttMessageId });
                    table.ForeignKey(
                        name: "FK_SnapshotMqttMessages_MqttMessages_MqttMessageId",
                        column: x => x.MqttMessageId,
                        principalTable: "MqttMessages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SnapshotMqttMessages_Snapshots_SnapshotId",
                        column: x => x.SnapshotId,
                        principalTable: "Snapshots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SnapshotMqttMessages_MqttMessageId",
                table: "SnapshotMqttMessages",
                column: "MqttMessageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SnapshotMqttMessages");

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
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
