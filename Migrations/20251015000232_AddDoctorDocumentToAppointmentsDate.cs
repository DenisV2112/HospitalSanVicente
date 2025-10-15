using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalSanVicente.Migrations
{
    /// <inheritdoc />
    public partial class AddDoctorDocumentToAppointmentsDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "date",
                table: "Appointments",
                newName: "AppointmentDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AppointmentDate",
                table: "Appointments",
                newName: "date");
        }
    }
}
