using Microsoft.EntityFrameworkCore.Migrations;

namespace Firewatch.Infrastructure.Persistence.Migrations
{
    public partial class ConfgureForeignKeyOnTradeExecution : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "TradeExecutions");

            migrationBuilder.AddColumn<int>(
                name: "AccountId",
                table: "TradeExecutions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TradeExecutions_AccountId",
                table: "TradeExecutions",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_TradeExecutions_Accounts_AccountId",
                table: "TradeExecutions",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TradeExecutions_Accounts_AccountId",
                table: "TradeExecutions");

            migrationBuilder.DropIndex(
                name: "IX_TradeExecutions_AccountId",
                table: "TradeExecutions");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "TradeExecutions");

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "TradeExecutions",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
