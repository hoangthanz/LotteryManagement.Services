using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LotteryManagement.Data.EF.Migrations
{
    public partial class addbankcard : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OwnerBanks",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FullNameOwner = table.Column<string>(nullable: true),
                    Branch = table.Column<string>(nullable: true),
                    AccountNumber = table.Column<string>(nullable: true),
                    BankName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OwnerBanks", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OwnerBanks");
        }
    }
}
