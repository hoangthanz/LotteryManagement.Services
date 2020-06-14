using Microsoft.EntityFrameworkCore.Migrations;

namespace LotteryManagement.Data.EF.Migrations
{
    public partial class fix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentRate",
                table: "Bao_Lottos");

            migrationBuilder.DropColumn(
                name: "beginRate",
                table: "Bao_Lottos");

            migrationBuilder.DropColumn(
                name: "endRate",
                table: "Bao_Lottos");

            migrationBuilder.AddColumn<string>(
                name: "OwnerBankId",
                table: "Transactions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankCardId",
                table: "TransactionHistories",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OwnerBankId",
                table: "TransactionHistories",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OwnerBankId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "BankCardId",
                table: "TransactionHistories");

            migrationBuilder.DropColumn(
                name: "OwnerBankId",
                table: "TransactionHistories");

            migrationBuilder.AddColumn<double>(
                name: "CurrentRate",
                table: "Bao_Lottos",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "beginRate",
                table: "Bao_Lottos",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "endRate",
                table: "Bao_Lottos",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
