# Task Manager

## Overview

This is a simple Task Manager API developed using ASP.NET Core (.NET 8.0) and PostgreSQL. The application allows users to manage tasks and associate them with individuals, providing functionalities to create, read, update, and delete both tasks and people.

## Features

- **CRUD Operations for People**: Create, read, update, and delete users.
- **CRUD Operations for Tasks**: Create, read, update, and delete tasks.
- **Task Assignment**: Assign tasks to individuals and remove assignments.
- **Status Tracking**: Track the status of tasks (Pending, In Progress, Completed).
- **Data Persistence**: Uses PostgreSQL for data storage.
- **Unit Testing**: Includes unit tests for controllers using xUnit.

## Technologies Used

- **Backend**: C#, ASP.NET Core 8.0;
- **Database**: PostgreSQL;
- **Testing**: xUnit, Moq;
- **Dependency Injection**: Built-in support in ASP.NET Core;
- **Entity Framework Core**: For database interactions.

## Getting Started

### Prerequisites

- [.NET SDK 8.0](https://dotnet.microsoft.com/download)
- [PostgreSQL](https://www.postgresql.org/download/)
- [Docker](https://www.docker.com/get-started)
- [Docker Compose](https://docs.docker.com/compose/install/)

### Installation

1. Clone the repository:

   ```bash
   git clone <repository_url>
   cd TaskManager
    ```

2. Create a PostgreSQL database. You can use the provided Docker setup below or create a database manually.

### Docker Setup

1. Create a `.env` file in the root directory with the following environment variables:

   ```bash
   POSTGRES_USER=postgres
   POSTGRES_PASSWORD=postgres
   POSTGRES_DB=taskmanager
   ```

2. Run the following command to start the PostgreSQL database:

   ```bash
    docker-compose up -d
    ```

3. Run the following command to start the application:

    ```bash
    dotnet run
    ```

4. Run the following command to run the unit tests:

    ```bash
    dotnet test
    ```

5. Run the following command to run the application:

    ```bash
    dotnet run
    ```

## API Endpoints

### People
```bash
GET /api/person: Get all people.
GET /api/person/{id}: Get a person by ID.
POST /api/person: Create a new person.
PUT /api/person/{id}: Update a person.
DELETE /api/person/{id}: Delete a person.
```

### Tasks

```bash
GET /api/task: Get all tasks.
GET /api/task/{id}: Get a task by ID.
POST /api/task: Create a new task.
PUT /api/task/{id}: Update a task.
DELETE /api/task/{id}: Delete a task.
POST /api/task/{taskId}/person/{personId}: Assign a person to a task.
DELETE /api/task/{taskId}/person: Remove a person from a task.
```

## Conclusion

This Task Manager application provides a solid foundation for managing tasks and users. You can expand its functionality by adding features such as authentication, advanced search, and notifications. Feel free to contribute to the project or use it as a reference for your own applications.

## License
This project is licensed under the MIT License.