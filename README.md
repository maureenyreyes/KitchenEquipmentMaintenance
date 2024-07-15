# KitchenEquipmentMaintenance
Project Overview
This project utilizes Windows Presentation Foundation (WPF) for the user interface and Entity Framework 6 (EF6) for data access, following the Model-View-ViewModel (MVVM) design pattern.

Database Schema Initialization
To initialize the database schema:

Open Package Manager Console in Visual Studio.
Run Enable-Migrations to enable Code First Migrations.
Run Add-Migration Initial to scaffold the initial migration for creating the database schema based on the current model.

Notes
Ensure your connection string in app.config or web.config points to your desired database server and credentials.
Modify the entities and their relationships in the model classes (DbContext) to match your application's requirements.
