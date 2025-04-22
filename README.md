Company M - Case Management System
A comprehensive Customer Relationship Management (CRM) system built for Company M's Customer Support department. This application allows support staff to efficiently track and manage customer queries from multiple channels.

📋 Features

Multi-channel Support: Track cases from Visit, AI, Call, WhatsApp, and Email channels
Case Management: Create, view, update, and delete support cases
Customer Database: Maintain customer information
Search & Filter: Find cases by customer name or channel
WhatsApp API: Dedicated RESTful API for WhatsApp team integration

🛠️ Technology Stack

Frontend: HTML, CSS, JavaScript
Backend: ASP.NET Core Web API (C#)
Database: Microsoft SQL Server
ORM: Entity Framework Core

📋 Requirements
To run this application, you need:

Visual Studio 2022 or Visual Studio Code with C# extension
.NET 8.0 SDK or later - Download here
SQL Server (LocalDB is sufficient for testing)
Entity Framework Core tools: Install using dotnet tool install --global dotnet-ef
A modern web browser (Chrome, Firefox, Edge)

🚀 Getting Started
Step 1: Clone the Repository
bashgit clone https://github.com/joenyo/CompanyM-CRM.git
cd CompanyM-CRM
Step 2: Set Up the Backend

Navigate to the Backend directory:
bashcd Backend

Restore dependencies:
bashdotnet restore

Update database connection string in appsettings.json if needed:
json"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=CompanyM_CRM;Trusted_Connection=True;MultipleActiveResultSets=true"
}

Create the database:
bashdotnet ef database update

Run the backend server:
bashdotnet run
The API will run at http://localhost:5163 (port may vary)



