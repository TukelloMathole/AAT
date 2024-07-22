Overview

This project contains Assessment 1,2,3. Below is an overview of each directory:

Directory Structure

Assessment 1 - SQL

This directory focuses on optimizing SQL code for handling large volumes of data. The provided SQL code has been reviewed and updated with a focus on improving performance and efficiency. The updates include optimizations for processing large datasets and are accompanied by explanations detailing why the changes were made.

Key Tasks Completed:

    Reviewed and updated the provided SQL code.
    Implemented optimizations for better performance with large datasets.
    Provided explanations for all changes, focusing on efficiency.

Assessment 2 - Threading

Last Updated: Jul 18, 2024

This assessment involves creating a multi-threaded application where a thread picks random odd numbers and adds them to a global list. The following sections detail the task and provide a sample solution.
Task

Objective: Create a thread that generates random odd numbers and adds them to a global list.

Requirements:

    Implement a thread that continuously generates random odd numbers.
    Add each generated odd number to a global list.
    Ensure thread safety when accessing the global list.


Assessment 3 - CRUD

Last Updated: Jul 22, 2024

This assessment involves creating a small CRUD (Create, Read, Update, Delete) application using the following technology stack:

    ASP.NET Core: For building the Web API.
    SQLite3: For the database.
    Blazor: For the frontend user interface.
    LINQ: For querying data.
    Entity Framework: For ORM and database management.
    HTML: For structuring the content.
    Bootstrap: For responsive design and components.
    Tailwind CSS: For utility-first styling.
    Custom CSS: For additional styling and design tweaks.

Features

    Landing Page: Displays a list of top events available for booking.
    Event Management:
        Create: Add new events.
        Read: View details of events.
        Update: Modify existing events.
        Delete: Remove events.
    User Registration:
        Register for events with available seats.
        Prevent multiple registrations by the same user for an event.
        Assign a unique reference number to each registration.
    Event Search and Filtering: Search and filter events based on various criteria.
    Asynchronous API/Business Logic: Utilizes async/await .NET patterns for efficient processing.

Admin Section Features

The admin section allows for comprehensive event management through the following CRUD operations:

    Create Event: Add new events to the system.
    Read Event: View details of existing events.
    Update Event: Modify details of existing events.
    Delete Event: Remove events from the system.

Implementation Details
Project Structure

    Backend API (ASP.NET Core):
        EventRegistrationApi: Handles CRUD operations and business logic. Uses SQLite3 for data storage and Entity Framework for ORM. LINQ is used for querying data.

    Blazor Frontend:
        EventRegistrationApp: Provides the user interface for managing and registering for events. Utilizes HTML for content structure, Bootstrap for responsive design, Tailwind CSS for utility-first styling, and custom CSS for additional design tweaks.

Data Storage

    SQLite3: A SQLite database file is included for persistent data storage.

ORM

    Entity Framework Core (EFCore): Used for database interactions and CRUD operations.

UI

    HTML: For structuring the content of pages.
    Bootstrap: Provides responsive design and UI components.
    Tailwind CSS: Used for utility-first styling, enhancing the design.
    Custom CSS: Applied for additional styling and tweaks.
