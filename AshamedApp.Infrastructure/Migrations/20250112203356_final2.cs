using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AshamedApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class final2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SnapshotId",
                table: "MqttMessages");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SnapshotId",
                table: "MqttMessages",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
