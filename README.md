Company Management System
Initial Admin Credentials
When you run the application for the first time, an admin user is created automatically. Use the following credentials to log in as an admin:

Email: admin@company.com
Password: Admin@123
Please change the password after logging in for the first time to ensure security.

Application Architecture and Design Patterns
This application is designed using the following architectural patterns and technologies:

3-Tier Architecture: The application is divided into the Presentation, Business Logic, and Data Access layers to separate concerns and improve maintainability.
Repository Pattern: This pattern is used to abstract the data access layer, providing a cleaner interface and making the data access code more testable.
Unit of Work Pattern: This pattern is used to manage transactions and ensure that multiple repository operations are treated as a single unit of work.
Specification Design Pattern: This pattern is used to encapsulate complex query logic in a reusable and testable manner.
Identity Package: ASP.NET Core Identity is used for authentication and authorization.
AutoMapper: This library is used to map objects between different layers, reducing boilerplate code and improving readability.
Dependency Injection (DI): ASP.NET Core's built-in dependency injection is used to manage the application's dependencies, improving testability and maintainability.
