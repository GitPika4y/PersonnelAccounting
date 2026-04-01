using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MiddleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Inn = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Passport = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.CheckConstraint("CH_Employee_BirthDate", "[BirthDate] <= GETDATE()");
                    table.CheckConstraint("CH_Employee_Inn", "LEN([Inn]) IN (12, 14)");
                    table.CheckConstraint("CH_Employee_Passport", "LEN([Passport]) = 11 AND [Passport] LIKE '[0-9][0-9][0-9][0-9]-[0-9][0-9][0-9][0-9][0-9][0-9]'");
                    table.CheckConstraint("CH_Employee_PhoneNumber", "LEN([PhoneNumber]) BETWEEN 10 AND 14");
                });

            migrationBuilder.CreateTable(
                name: "Positions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Salary = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Positions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Login = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.CheckConstraint("CH_User_Role", "[Role] IN ('Admin','User')");
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HireDepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    HirePositionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FireReason = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.CheckConstraint("CH_Order_Date", "[Date] >= GETDATE()");
                    table.CheckConstraint("CH_Order_Date_StartDate", "[Date] <= [StartDate]");
                    table.CheckConstraint("CH_Order_StartDate_EndDate", "[StartDate] <= [EndDate]");
                    table.CheckConstraint("CH_Order_Type", "[Type] IN ('Hire','Fire','StudyLeave','Vacation','BusinessTrip')");
                    table.ForeignKey(
                        name: "FK_Orders_Departments_HireDepartmentId",
                        column: x => x.HireDepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Orders_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_Positions_HirePositionId",
                        column: x => x.HirePositionId,
                        principalTable: "Positions",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Login", "Password", "Role", "Username" },
                values: new object[,]
                {
                    { new Guid("ed2f1ea9-3714-40db-a5b4-af4e099214d1"), "admin_", "$2a$11$OswhLeNo0PGwGSGnI0RwTONRZtZUfgw656L0CbJtjY0/L00pvpyea", "Admin", null },
                });

            migrationBuilder.CreateIndex(
                name: "IX_Departments_Title",
                table: "Departments",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Inn",
                table: "Employees",
                column: "Inn",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Passport",
                table: "Employees",
                column: "Passport",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_PhoneNumber",
                table: "Employees",
                column: "PhoneNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_EmployeeId",
                table: "Orders",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_HireDepartmentId",
                table: "Orders",
                column: "HireDepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_HirePositionId",
                table: "Orders",
                column: "HirePositionId");

            migrationBuilder.CreateIndex(
                name: "IX_Positions_Title",
                table: "Positions",
                column: "Title",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Positions");
        }
    }
}
