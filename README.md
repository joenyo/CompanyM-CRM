# Company M - Case Management System

A comprehensive Customer Relationship Management (CRM) system built for Company M's Customer Support department. This application allows support staff to efficiently track and manage customer queries from multiple channels.

## Features

- **Multi-channel Support:** Track cases from Visit, AI, Call, WhatsApp, and Email channels
- **Case Management:** Create, view, update, and delete support cases
- **Customer Database:** Maintain customer information
- **Search & Filter:** Find cases by customer name or channel
- **WhatsApp API:** Dedicated RESTful API for WhatsApp team integration

## Technology Stack

- **Frontend:** HTML, CSS, JavaScript
- **Backend:** ASP.NET Core Web API (C#)
- **Database:** Microsoft SQL Server
- **ORM:** Entity Framework Core

## Requirements

To run this application, you need:

- Visual Studio 2022 or Visual Studio Code with C# extension
- .NET 8.0 SDK or later - Download here
- SQL Server (LocalDB is sufficient for testing)
- Entity Framework Core tools: Install using `dotnet tool install --global dotnet-ef`
- A modern web browser (Chrome, Firefox, Edge)

## Getting Started

### Step 1: Clone the Repository

```bash
git clone https://github.com/joenyo/CompanyM-CRM.git
cd CompanyM-CRM
```

### Step 2: Set Up the Backend

1. Navigate to the Backend directory:

   ```bash
   cd Backend
   ```

2. Restore dependencies:

   ```bash
   dotnet restore
   ```

3. Update the database connection string in `appsettings.json` if needed:

   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=CompanyM_CRM;Trusted_Connection=True;"
   }
   ```

4. Create the database:

   ```bash
   dotnet ef database update
   ```

5. Run the backend server:

   ```bash
   dotnet run
   ```

   The API will run at `https://localhost:5163` (port may vary).

## API Documentation

### Cases API

| Endpoint | Method | Description |
| --- | --- | --- |
| `/api/Cases` | GET | Get all cases |
| `/api/Cases/{id}` | GET | Get case by ID |
| `/api/Cases/Search` | GET | Search cases by customer name/channel |
| `/api/Cases` | POST | Create a new case |
| `/api/Cases/{id}` | PUT | Update a case |
| `/api/Cases/{id}` | DELETE | Delete a case |

### WhatsApp API

The WhatsApp team has access to a dedicated API for managing their cases:

| Endpoint | Method | Description |
| --- | --- | --- |
| `/api/WhatsApp` | GET | Get all WhatsApp cases |
| `/api/WhatsApp/{id}` | GET | Get WhatsApp case by ID |
| `/api/WhatsApp` | POST | Create WhatsApp case |
| `/api/WhatsApp/{id}` | PUT | Update WhatsApp case |
| `/api/WhatsApp/{id}` | DELETE | Delete WhatsApp case |

## Testing the WhatsApp API

You can test the WhatsApp API using tools like Postman or cURL:

**Get all WhatsApp cases**

```http
GET http://localhost:5163/api/WhatsApp
```

**Create a new WhatsApp case**

```http
POST http://localhost:5163/api/WhatsApp
Content-Type: application/json

{
  "customerId": 1,
  "subject": "WhatsApp Query",
  "description": "Customer contacted via WhatsApp",
  "status": "OPEN"
}
```

## Notes

- The WhatsApp API ensures that the WhatsApp team can only access cases from the WhatsApp channel.
- This application does not include authentication/authorization in the current version.
- For production use, additional security measures should be implemented.
