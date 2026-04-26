using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

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
                name: "EmployeeEducations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Qualification = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Specialization = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GraduationYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeEducations", x => x.Id);
                    table.CheckConstraint("CH_EmployeeEducation_Qualification", "[Qualification] IN ('Secondary','SecondarySpecial','Bachelor','Master','Specialist','PhD')");
                    table.CheckConstraint("CH_EmployeeEducation_Specialization", "[Specialization] IN ('SoftwareEngineering','InformationSystems','AppliedInformatics','CyberSecurity','Economics','Accounting','Finance','Management','HumanResources','Marketing','Law','PublicAdministration','Logistics','CivilEngineering','ElectricalEngineering')");
                });

            migrationBuilder.CreateTable(
                name: "EmployeePassports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Serial = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Number = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GivenBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeePassports", x => x.Id);
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
                    Login = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.CheckConstraint("CH_User_Role", "[Role] IN ('Admin','User')");
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
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PassportId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EducationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.CheckConstraint("CH_Employee_BirthDate", "[BirthDate] <= GETDATE()");
                    table.CheckConstraint("CH_Employee_Gender", "[Gender] IN ('Male','Female')");
                    table.CheckConstraint("CH_Employee_Inn", "LEN([Inn]) IN (12, 14)");
                    table.CheckConstraint("CH_Employee_PhoneNumber", "LEN([PhoneNumber]) BETWEEN 10 AND 16");
                    table.ForeignKey(
                        name: "FK_Employees_EmployeeEducations_EducationId",
                        column: x => x.EducationId,
                        principalTable: "EmployeeEducations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Employees_EmployeePassports_PassportId",
                        column: x => x.PassportId,
                        principalTable: "EmployeePassports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.CreateIndex(
                name: "IX_Departments_Title",
                table: "Departments",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeePassports_Number",
                table: "EmployeePassports",
                column: "Number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeePassports_Serial",
                table: "EmployeePassports",
                column: "Serial",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_EducationId",
                table: "Employees",
                column: "EducationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Inn",
                table: "Employees",
                column: "Inn",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_PassportId",
                table: "Employees",
                column: "PassportId",
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

            migrationBuilder.CreateIndex(
                name: "IX_Users_Login",
                table: "Users",
                column: "Login",
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

            migrationBuilder.DropTable(
                name: "EmployeeEducations");

            migrationBuilder.DropTable(
                name: "EmployeePassports");
        }
    }
}
