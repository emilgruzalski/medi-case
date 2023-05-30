# MediCase – Backend

REST API for **MediCase**, a multilingual web application for learning emergency medicine communication.
Learners go through a tree of content cards, patient interview questions with audio recordings, glossaries
and tests. Moderators edit the content in many languages and administrators review and publish it.

The application was created during the *Programowanie zespołowe* (team programming) course in the 2022/2023
academic year, in cooperation with Collegium Medicum UMK, and was first presented in May 2023.

The React client lives in the [MediCase-Frontend](https://github.com/emilgruzalski/MediCase-Frontend) repository.

## Features

- Content tree built from entities of different types: categories, content cards, question lists, questions,
  tests, test questions and answers – only valid parent/child combinations are accepted
- Translations of every entity into any number of languages, with HTML content from the rich text editor
- Audio and image files attached to translations
- Glossary (dictionary) entries per section and language
- **AI translations** with Amazon Translate and **speech generation** with Amazon Polly
- Two databases: moderators work on a working copy, administrators review pending changes and publish
  everything to the learners' database in one transaction
- Blocking parts of the tree so learners do not see them
- User accounts with roles **User**, **Moderator** and **Admin**, user groups, paging and search
- Search in content titles for learners and moderators

## Architecture

| Folder | Responsibility |
| --- | --- |
| `Controllers` | API endpoints |
| `Services` | Business logic (entities, tree building, synchronization, files, AI) |
| `Repositories` | Data access with Entity Framework Core |
| `Models` | Database models, `MedicalContext` and entity type rules |
| `DataTransferObjects`, `MapperProfiles` | DTOs and AutoMapper profiles |
| `Authentication` | Session token authentication handler and roles |
| `Middleware`, `Exceptions` | Error handling |
| `Seeders` | Entity types, languages, administrator account and sample content |
| `MediCaseAPI.Tests` | Unit tests (xUnit, EF Core InMemory) |

## Tech stack

.NET 7, ASP.NET Core Web API, Entity Framework Core 7, SQL Server, AutoMapper, BCrypt.Net, AWS SDK
(Amazon Translate, Amazon Polly), Swagger (Swashbuckle), xUnit, Docker.

## Getting started

Requirements: [.NET 7 SDK](https://dotnet.microsoft.com/download/dotnet/7.0) and SQL Server
(LocalDB on Windows or SQL Server in Docker).

1. Set the connection strings if you do not use LocalDB:

   ```bash
   export ConnectionStrings__MedicalDatabase="Server=localhost,1433;Database=MediCase;User Id=sa;Password=<db-password>;TrustServerCertificate=True"
   export ConnectionStrings__PublishedDatabase="Server=localhost,1433;Database=MediCasePublished;User Id=sa;Password=<db-password>;TrustServerCertificate=True"
   ```

2. Set the administrator account created on the first start:

   ```bash
   export Seed__AdminEmail="admin@medicase.local"
   export Seed__AdminPassword="<admin-password>"
   ```

3. Optionally set AWS credentials for AI translations and speech
   (`AWS_ACCESS_KEY_ID`, `AWS_SECRET_ACCESS_KEY`; the region is set in `appsettings.json`).

4. Run the API:

   ```bash
   cd MediCaseAPI
   dotnet run --launch-profile https
   ```

   Migrations are applied to both databases on startup. In the Development environment sample content is
   added and published. Swagger UI is available at <https://localhost:7295/swagger>.

## Docker

```bash
cp .env.example .env   # fill in DB_PASSWORD and ADMIN_PASSWORD
docker compose up --build
```

The API is available at <http://localhost:8080>.

## Tests

```bash
dotnet test
```

## API overview

| Method | Endpoint | Role |
| --- | --- | --- |
| POST | `/api/Users/register`, `/api/Sessions/login` | anonymous |
| POST | `/api/Sessions/logout` | any |
| GET, PUT | `/api/Users/me`, PUT `/api/Users/me/password` | any |
| GET | `/api/Learning/languages`, `/tree`, `/entity/{id}`, `/entity/{id}/dictionary`, `/files/{id}`, `/search` | any |
| GET, POST, PUT, DELETE | `/api/Entity`, PUT `/api/Entity/{id}/move`, GET `/api/Entity/search` | Moderator, Admin |
| GET | `/api/EntitiesGraphData`, `/api/EntitiesGraphData/tree`, `/api/EntityTypes` | Moderator, Admin |
| GET, POST, PUT, DELETE | `/api/EntityTranslations`, `/api/EntityDictionary`, `/api/EntityLanguages` | Moderator, Admin |
| GET, POST, DELETE | `/api/EntityTranslationFiles` | Moderator, Admin |
| POST | `/api/Ai/translate`, `/api/Ai/speech/{translationId}` | Moderator, Admin |
| GET, POST, PUT, DELETE | `/api/Users`, `/api/Groups` | Admin |
| GET | `/api/Synchronization/changes` | Admin |
| POST | `/api/Synchronization` | Admin |
