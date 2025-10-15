using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalSanVicente.Migrations
{
    /// <inheritdoc />
    public partial class AddDoctorDocumentToAppointments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Doctors_doctor_document",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Patients_patient_document",
                table: "Appointments");

            migrationBuilder.RenameColumn(
                name: "mail",
                table: "Doctors",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "patient_document",
                table: "Appointments",
                newName: "PatientId");

            migrationBuilder.RenameColumn(
                name: "doctor_document",
                table: "Appointments",
                newName: "DoctorId");

            migrationBuilder.RenameIndex(
                name: "IX_Appointments_patient_document",
                table: "Appointments",
                newName: "IX_Appointments_PatientId");

            migrationBuilder.RenameIndex(
                name: "IX_Appointments_doctor_document",
                table: "Appointments",
                newName: "IX_Appointments_DoctorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Doctors_DoctorId",
                table: "Appointments",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "document",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Patients_PatientId",
                table: "Appointments",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "document",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Doctors_DoctorId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Patients_PatientId",
                table: "Appointments");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "Doctors",
                newName: "mail");

            migrationBuilder.RenameColumn(
                name: "PatientId",
                table: "Appointments",
                newName: "patient_document");

            migrationBuilder.RenameColumn(
                name: "DoctorId",
                table: "Appointments",
                newName: "doctor_document");

            migrationBuilder.RenameIndex(
                name: "IX_Appointments_PatientId",
                table: "Appointments",
                newName: "IX_Appointments_patient_document");

            migrationBuilder.RenameIndex(
                name: "IX_Appointments_DoctorId",
                table: "Appointments",
                newName: "IX_Appointments_doctor_document");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Doctors_doctor_document",
                table: "Appointments",
                column: "doctor_document",
                principalTable: "Doctors",
                principalColumn: "document",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Patients_patient_document",
                table: "Appointments",
                column: "patient_document",
                principalTable: "Patients",
                principalColumn: "document",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
