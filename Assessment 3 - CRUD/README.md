Event Registration CRUD Application
Description

This project is a web application featuring an ASP.NET Core Web API with an SQLite3 database and a Blazor frontend. The Web API handles data management and interactions, while the Blazor frontend provides a modern user interface for accessing and managing the data. The project includes an SQLite3 database for lightweight, file-based data storage.
Features

    ASP.NET Core Web API for backend functionality.
    SQLite3 database for data storage and management.
    Blazor frontend for a rich, interactive user experience.
    Configurable base URL for Blazor in Program.cs.

Technologies Used

    ASP.NET Core: For building the Web API.
    SQLite3: For the database.
    Blazor: For the frontend user interface.
    LINQ: For querying data.
    Entity Framework: For ORM and database management.
    HTML: For structuring the content.
    Bootstrap: For responsive design and components.
    Tailwind CSS: For utility-first styling.
    Custom CSS: For additional styling and design tweaks.

Prerequisites

    .NET 8 or later
    SQLite3

Setup and Installation
Clone the Repository

bash

git clone https://github.com/TukelloMathole/AAT.git
cd AAT

Backend Setup (ASP.NET Core Web API)

    Navigate to the Web API directory:

    bash

cd Assessment 3 - CRUD/EventRegistrationApi

Install dependencies:

bash

dotnet restore

Apply database migrations:

SQLite3 database migrations are managed using Entity Framework Core. Apply migrations with:

bash

dotnet ef database update

Run the Web API:

bash

    dotnet run

    The Web API will be available at https://localhost:7018 by default.

Frontend Setup (Blazor)

    Navigate to the Blazor directory:

    bash

cd Assessment 3 - CRUD/EventRegistrationApp

Install dependencies:

bash

dotnet restore

Configure Base URL:

The base URL for API calls in the Blazor application is configured in Program.cs. Ensure it matches the URL of your running Web API.

Run the Blazor application:

bash

dotnet run

The Blazor application will be available at https://localhost:7161/ by default.

Admin Section Login

To access the admin section of the application:

    Email: admin@event.co.za
    Password: password

Configuration

    Web API: Configuration settings are located in appsettings.json. The database connection string for SQLite3 is set here.

    Blazor: Base URL for the API is configured in Program.cs:

    csharp

    builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7018/") });

Usage

    Access the Blazor frontend at https://localhost:7161/.
    Interact with the Web API through the Blazor UI to perform CRUD operations.

Database

    SQLite3 Database: The project uses SQLite3 for data storage. The database file is included in the project directory and managed through Entity Framework Core migrations.


For any inquiries, please contact matholecristian@gmail.com.
