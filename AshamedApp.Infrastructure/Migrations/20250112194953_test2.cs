using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AshamedApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class test2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MqttMessageId",
                table: "Snapshots",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SnapshotId",
                table: "MqttMessages",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MqttMessageId",
                table: "Snapshots");

            migrationBuilder.DropColumn(
                name: "SnapshotId",
                table: "MqttMessages");
        }
    }
}
