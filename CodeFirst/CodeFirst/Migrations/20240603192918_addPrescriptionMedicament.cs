using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeFirst.Migrations
{
    /// <inheritdoc />
    public partial class addPrescriptionMedicament : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Medicaments_Prescriptions_PrescriptionId",
                table: "Medicaments");

            migrationBuilder.DropIndex(
                name: "IX_Medicaments_PrescriptionId",
                table: "Medicaments");

            migrationBuilder.DropColumn(
                name: "PrescriptionId",
                table: "Medicaments");

            migrationBuilder.CreateTable(
                name: "PrescriptionMedicaments",
                columns: table => new
                {
                    PrescriptionId = table.Column<int>(type: "int", nullable: false),
                    MedicamentId = table.Column<int>(type: "int", nullable: false),
                    Dose = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrescriptionMedicaments", x => new { x.PrescriptionId, x.MedicamentId });
                    table.ForeignKey(
                        name: "FK_PrescriptionMedicaments_Medicaments_MedicamentId",
                        column: x => x.MedicamentId,
                        principalTable: "Medicaments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PrescriptionMedicaments_Prescriptions_PrescriptionId",
                        column: x => x.PrescriptionId,
                        principalTable: "Prescriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PrescriptionMedicaments_MedicamentId",
                table: "PrescriptionMedicaments",
                column: "MedicamentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PrescriptionMedicaments");

            migrationBuilder.AddColumn<int>(
                name: "PrescriptionId",
                table: "Medicaments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Medicaments_PrescriptionId",
                table: "Medicaments",
                column: "PrescriptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Medicaments_Prescriptions_PrescriptionId",
                table: "Medicaments",
                column: "PrescriptionId",
                principalTable: "Prescriptions",
                principalColumn: "Id");
        }
    }
}
