# Hospital Management System

## Description
This application allows you to manage:
- Patients (registration, editing, listing, and duplicate validation).
- Doctors (registration, editing, listing, filtering by specialty, and duplicate validation).
- Medical appointments (scheduling, canceling, marking as attended, listing by patient and doctor).
- Sending confirmation emails to patients and tracking email delivery status.

Data persistence is done using **Lists, Dictionaries, and Entity Framework Core**. Error handling is implemented with **try-catch** and business validations are applied.

---

## Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Visual Studio 2022 / JetBrains Rider
- Database (MySQL/PostgreSQL/SQL Server depending on your setup)
- Configured SMTP for email sending (Gmail with App Passwords can be used)

---

## Installation

1. Clone the repository:
```bash
git clone <https://github.com/DenisV2112/HospitalSanVicente.git>
cd <HospitalSanVicente>
```
2. Restore NuGet packages:
```bash
dotnet restore
```

---

## Create the Database and Apply Migrations
```bash
dotnet ef database update
```

---

## Configure Connection String in `appsettings.json`
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=HospitalDB;User=root;Password=yourPassword;"
  },
  "SMTP": {
    "Host": "smtp.gmail.com",
    "Port": 587,
    "Email": "youremail@gmail.com",
    "Password": "yourAppPassword"
  }
}
```

---

## Run the Project
```bash
dotnet run
```

---

## Access the Application
Open your browser and go to:
```
http://localhost:5125
```

---

## Usage

* Register patients and doctors.
* Schedule appointments by assigning a patient, doctor, date, and time.
* The system will validate:
  - No duplicate documents.
  - Doctor/patient does not have another appointment at the same time.
* Cancel or mark appointments as attended.
* Automatically send confirmation emails to patients.
* View email sending history.

---

## Error Handling and Validations

* Duplicate document validation.
* Appointment time conflict checks.
* Exception handling with clear messages.
* Ensures data integrity.

---

## Technologies

* C# / .NET 8
* ASP.NET Core MVC
* Entity Framework Core
* LINQ
* Lists and Dictionaries
* SMTP for email sending