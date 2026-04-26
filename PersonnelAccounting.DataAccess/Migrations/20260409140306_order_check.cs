using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class order_check : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CH_Order_Date",
                table: "Orders");

            migrationBuilder.DropCheckConstraint(
                name: "CH_Order_StartDate_EndDate",
                table: "Orders");

            migrationBuilder.AddCheckConstraint(
                name: "CH_Order_Date",
                table: "Orders",
                sql: "CAST([Date] AS DATE) >= CAST(GETDATE() AS DATE)");

            migrationBuilder.AddCheckConstraint(
                name: "CH_Order_StartDate_EndDate",
                table: "Orders",
                sql: "[EndDate] >= [StartDate]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CH_Order_Date",
                table: "Orders");

            migrationBuilder.DropCheckConstraint(
                name: "CH_Order_StartDate_EndDate",
                table: "Orders");

            migrationBuilder.AddCheckConstraint(
                name: "CH_Order_Date",
                table: "Orders",
                sql: "[Date] >= GETDATE()");

            migrationBuilder.AddCheckConstraint(
                name: "CH_Order_StartDate_EndDate",
                table: "Orders",
                sql: "[StartDate] <= [EndDate]");
        }
    }
}
