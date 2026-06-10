# ctnet-evaluation

This is a full-stack web application featuring a **React (Vite)** frontend and a **.NET (C#)** backend, utilizing a MySQL database and JWT-based authentication.

## Project Structure

The repository is organized into a monorepo structure:
- `/frontend` - The React Vite application, UI components (Ant Design), and E2E tests (Playwright).
- `/backend` - The .NET Web API, Entity Framework Core models, and authentication logic.

---

## Recommended IDE Setup

- **VS Code** or a **JetBrains IDE**, enhanced with **GitHub Copilot** for faster development.
- **Extensions:**
  - *Frontend:* ESLint, Prettier
  - *Backend:* C# Dev Kit (for VS Code)
- **Browser Tools:** [React Developer Tools](https://chromewebstore.google.com/detail/react-developer-tools/fmkadmapgofadopljbjfkapdkoienihi) for component inspection.

---

## Prerequisites

Before you begin, ensure you have the following installed:
- [Node.js](https://nodejs.org/) (v20+ recommended)
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download)
- [MySQL Server](https://dev.mysql.com/downloads/mysql/)

---

## 1. Environment Setup

The backend relies on environment variables for database connections and JWT signing. 

1. At the **root** of the project (outside the frontend and backend folders), duplicate the `.sample.env` file.
2. Rename the duplicated file to `.env`.
3. Update the credentials inside `.env` to match your local MySQL setup and preferred JWT secrets:
   ```env
   DB_SERVER=localhost
   DB_NAME=database
   DB_UID=root
   DB_PWD=your_database_password
   DB_PORT=3306
   JWT_KEY=your_super_secret_jwt_key_here
   JWT_ISSUER=http://localhost:5079
   JWT_AUDIENCE=http://localhost:5173