
### [BookShop deployed to Azure](https://bookshopmvc-fgdmc9aqeyeucsc6.polandcentral-01.azurewebsites.net)

## Backend Architecture & Application Logic Overview

This section provides a high-level overview of the backend logic and implemented services in the BookShop application. It is intended as a guide for reviewers and for documentation purposes.

### Core Features Implemented

- **Book and Author Management**: Creation, validation, listing, and deletion of books and authors. Books are linked to authors (many-to-one). The author’s BookCount is incremented when a new book is added.
- **Ratings System**: Users can rate both books and authors. Ratings are managed via dedicated repositories and services.
- **Shopping Cart**: Registered and anonymous users can manage their shopping cart, add/remove items, and see their cart contents.
- **Search**: Search functionality for books and authors.
- **Authentication & Authorization**: Standard user authentication with role-based access control.
- **Background Services**: Scheduled cleanup (archiving/removal) of outdated ratings and other entities.
- **Infrastructure**: Use of repository and service patterns, dependency injection, and unit of work for data access.
- **Pagination**: Listing endpoints support pagination via an extension method.

---

### Main Backend Services

**Domain and Application Services**

These are registered via dependency injection and implement the core business logic:

- `UserService`: Manages user operations and authentication.
- `AuthorDomainService` & `BookDomainService`: Encapsulate business rules for authors and books.
- `CartService`: Handles all cart operations (creation, add/remove items, marking notifications as shown).
- `PolicyRoleService`: Manages authorization policies and user roles.
- `EmailSender`: Sending email notifications.

**Repositories**

Concrete implementations for data persistence:

- `UserRepository`
- `AuthorRepository`
- `BookRepository`
- `BookRatingRepository` & `AuthorRatingRepository`
- `CartRepository`

All repositories inherit from a `GenericRepository` base class, providing common methods for add, update, and save operations.

---


### UML Diagram

Below is a simplified UML class diagram showing the key backend components and their relationships:

```
+---------------------+
|    UserService      |
+---------------------+
           |
           v
+---------------------+       +---------------------+
|   UserRepository    |------>|    ShopDbContext    |
+---------------------+       +---------------------+
           |
           v
+---------------------+
|     CartService     |
+---------------------+
           |
           v
+---------------------+       +-------------------+
|   CartRepository    |<----->|   CartEntity      |
+---------------------+       +-------------------+
           |                          |
           v                          v
+---------------------+       +-------------------+
| BookRepository      |------>| BookEntity        |
+---------------------+       +-------------------+
           |
           v
+---------------------+
|BookRatingRepository |
+---------------------+
```

_Note: This is a conceptual overview. Actual class relationships are more detailed in the codebase._

---

### Shopping Cart Logic

- **Cart Creation**: When a user (authenticated or guest) interacts with the cart for the first time, a new cart is created and linked to their user ID.
- **Adding Items**: When adding a book to the cart:
  - The system validates the user and book exist.
  - If the cart does not exist, it’s created.
  - If the book is already in the cart, it increases the quantity; otherwise, a new cart item is created.
  - The book’s available quantity is decreased.
- **Removing Items**: When removing a book from the cart:
  - If the item exists, its quantity is decreased or removed.
  - The book’s available quantity is increased accordingly.
- **Persistence**: All cart and item changes are persisted to the database via the repository and context.

---

### Ratings Logic

- **Book & Author Ratings**: Each rating entity is linked to both a user and either a book or author. Ratings can be fetched or created via their repositories.

---

### Background Services

- **ArchiveEntitiesCleanupService**: Runs in the background to remove or archive old ratings and other obsolete entities.

---

### Code Quality & Extensibility

- Uses dependency injection for all services and repositories.
- Follows separation of concerns and clean architecture principles.
- Employs unit of work and generic repository patterns for consistent data access.
- Extensible for new features (such as domain events, more background tasks, etc.).

---

### Useful Commands

- **Create Migration**:
  ```bash
  dotnet ef migrations add <MigrationName> --project BookShop.Infrastructure --startup-project BookShop.Web
  ```
- **Update Database**:
  ```bash
  dotnet ef database update --project BookShop.Infrastructure --startup-project BookShop.Web
  ```

---

### Additional Notes

- Logging and error handling are implemented throughout the services.
- Credentials and secrets are managed via Key Vault for security.
- The system is deployed on Azure.

---

For questions or further code review, please contact the maintainers or open an issue.



