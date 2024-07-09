# NZ Walks

NZ Walks is an ASP.NET Core API project structured using an N-Tier architecture, comprising API, Data Access, Service, and Model layers. The project employs the Repository Pattern for streamlined data access operations, enabling CRUD (Create, Read, Update, Delete) functionality for entities such as Region, Walk, and Difficulty. Additionally, it features user authentication via Identity JWT 🔑 and integrates several third-party libraries, including AutoMapper, API Versioning, Serilog, and JWT Bearer.

## Features

- **Region Management:** Provides endpoints to add, edit, delete, and retrieve information about regions of New Zealand.
- **Walk Management:** Allows CRUD operations on walking trails.
- **Difficulty Management:** Used to define the difficulty level of a walking trail.
- **User Authentication:** Utilizes Identity JWT for user authentication and management.

## Architecture

The project follows an N-Tier architecture with the following layers:

- **API Layer:** Contains API controllers responsible for handling HTTP requests and responses.
- **Service Layer:** Implements business logic and interacts with the Data Access layer.
- **Data Access Layer:** Includes repositories implementing the Repository Pattern for data access operations.
- **Model Layer:** Contains domain models representing entities and DTOs (Data Transfer Objects) for data exchange.

## Technologies Used
- ASP.NET Core: The API is built using ASP.NET Core framework, which provides a powerful and flexible platform for building web applications and APIs.
- Entity Framework Core: Entity Framework Core is used as the Object-Relational Mapping (ORM) tool to interact with the database.
- Microsoft Identity: The API uses Microsoft Identity for user authentication and role-based authorization.
- AutoMapper: AutoMapper is used for object-to-object mapping between domain models and DTOs.
- Microsoft.AspNetCore.Mvc: The API uses ASP.NET Core's MVC framework for handling HTTP requests and responses.
- Microsoft.Extensions.Logging: Logging is done using the ILogger interface provided by ASP.NET Core.
- IWebHostEnvironment: It is used to get the web host environment information, such as content root path.
- IHttpContextAccessor: IHttpContextAccessor is used to access the HttpContext, which is used for getting the base URL to form image file paths.

## Third-Party Libraries
- AutoMapper
- Serilog
- JWT Bearer


