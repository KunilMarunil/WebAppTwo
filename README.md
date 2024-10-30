# User Management Web Application with Audit Trail

## Description

This web application is a user management system built with **ASP.NET Core MVC** and **SQL Server** as the database. The application provides essential features such as **user registration, login, user management**, and an **audit trail** that logs user actions, including additions, updates, and deletions, with detailed information on data changes.

## Features

1. **User Authentication**
   - Login and logout functionality with cookie-based authentication.

2. **User Registration**
   - Allows new users to register with a username, email, and password.

3. **User Management**
   - View, edit, and manage users in the system. Access to this feature is restricted to logged-in users.

4. **Audit Trail**
   - Logs and tracks user actions such as additions and edits.
   - Provides information on old and new data for edits, giving a complete history of changes.

## Prerequisites

- **.NET SDK 8**: Make sure you have .NET SDK 8 installed.
- **SQL Server**: Ensure SQL Server is installed and accessible. The application connects to a local SQL Server instance by default.

## Installation

### 1. Clone the Repository

Clone the repository to your local machine:

```bash
git clone https://github.com/yourusername/YourRepo.git
cd YourRepo
```bash

### 2. Set Up the Database

Execute all the commands in Database.sql

### 3. Update appsettings.json

In the root directory of the project, open appsettings.json and update the ConnectionStrings section with your database connection string.

### 4. Install Dependencies

Open the project in Visual Studio or any IDE that supports .NET and run the following command in the Package Manager Console.

dotnet restore

### 5. Run the Application