# MediCase

**MediCase** is a multilingual web application for learning emergency medicine communication. Learners go through
a tree of content cards, patient interview questions with audio recordings, glossaries and tests. Moderators edit
the content in many languages and administrators review and publish it.

The application was created during the *Programowanie zespołowe* (team programming) course in the 2022/2023
academic year, in cooperation with Collegium Medicum UMK, and was first presented in May 2023.

## Repository structure

| Directory | Description | Tech stack |
| --- | --- | --- |
| [`backend`](backend) | REST API and AWS infrastructure | .NET 8, ASP.NET Core, Entity Framework Core, PostgreSQL, Terraform |
| [`frontend`](frontend) | Web client | React 19, Vite, TypeScript, Material UI |

Each directory has its own README with features, architecture, setup and deployment instructions.

## Running locally

Requirements: Docker and Node.js 22.

Start PostgreSQL, the local Cognito emulator and the API:

```bash
cd backend
cp .env.example .env   # fill in DB_PASSWORD and ADMIN_PASSWORD
docker compose up --build
```

Then start the client in another terminal:

```bash
cd frontend
npm ci
npm run dev
```

The application is available at <http://localhost:3000> and the API at <http://localhost:8080>.

## Tests

```bash
(cd backend && dotnet test)
(cd frontend && npm ci && npm test -- --run && npm run lint)
```

## CI

The workflows in [`.github/workflows`](.github/workflows) run separately for `backend/` and `frontend/`, only when files
of the given project change. They deploy to AWS only when the repository variables described in the project READMEs
are set.
