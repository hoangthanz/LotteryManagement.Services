using Microsoft.EntityFrameworkCore.Migrations;

namespace LotteryManagement.Data.EF.Migrations
{
    public partial class initialize_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BankCardId",
                table: "Transactions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BankCardId",
                table: "Transactions");
        }
    }
}
