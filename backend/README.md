# MediCase – Backend

REST API for **MediCase**, a multilingual web application for learning emergency medicine communication.
Learners go through a tree of content cards, patient interview questions with audio recordings, glossaries
and tests. Moderators edit the content in many languages and administrators review and publish it.

The application was created during the *Programowanie zespołowe* (team programming) course in the 2022/2023
academic year, in cooperation with Collegium Medicum UMK, and was first presented in May 2023.
In 2024 it moved to AWS-native services.

The React client lives in the [MediCase-Frontend](https://github.com/emilgruzalski/MediCase-Frontend) repository.

## Features

- Content tree built from entities of different types: categories, content cards, question lists, questions,
  tests, test questions and answers – only valid parent/child combinations are accepted
- Translations of every entity into any number of languages, with HTML content from the rich text editor
- Audio and image files attached to translations, stored in Amazon S3
- Glossary (dictionary) entries per section and language
- **AI translations** with Amazon Translate and **speech generation** with Amazon Polly
- Two databases: moderators work on a working copy, administrators review pending changes and publish
  everything to the learners' database in one transaction
- Sign-in with **Amazon Cognito**; roles **User**, **Moderator** and **Admin** come from Cognito groups,
  administrators invite users, manage roles and user groups

## Architecture on AWS

```
             ┌──────────────────── CloudFront ─────────────────────┐
 browser ──▶ │  /*       → S3 (frontend build)                     │
             │  /api/*   → Application Load Balancer → ECS Fargate │──▶ RDS PostgreSQL
             └─────────────────────────────────────────────────────┘    S3 (files)
 browser ──▶ Amazon Cognito (sign-in, tokens)                            Translate, Polly, Cognito
```

- The API runs as a container on **ECS Fargate** in private subnets, behind an **ALB** that only accepts
  requests from CloudFront.
- **RDS PostgreSQL** keeps both databases; the password is managed by RDS in **Secrets Manager**.
- Infrastructure is described with **Terraform** in [`infra/`](infra).
- GitHub Actions deploys with an IAM role assumed through **OIDC** – no access keys are stored in GitHub.

| Folder | Responsibility |
| --- | --- |
| `MediCaseAPI/Controllers` | API endpoints |
| `MediCaseAPI/Services` | Business logic (entities, tree building, synchronization, files, AI, Cognito) |
| `MediCaseAPI/Repositories` | Data access with Entity Framework Core |
| `MediCaseAPI/Models` | Database models, `MedicalContext` and entity type rules |
| `MediCaseAPI/Authentication` | Cognito token validation and roles |
| `MediCaseAPI/Seeders` | Entity types, languages, sample content and the first administrator |
| `MediCaseAPI.Tests` | Unit tests (xUnit, EF Core InMemory) |
| `infra` | Terraform: VPC, RDS, S3, Cognito, ECR, ECS, ALB, CloudFront, GitHub OIDC roles |
| `docker` | Local Cognito user pool for `cognito-local` |

## Tech stack

.NET 8, ASP.NET Core Web API, Entity Framework Core 8, PostgreSQL (Npgsql), AutoMapper, AWS SDK (S3, Cognito,
Translate, Polly), JWT Bearer, Swagger, xUnit, Docker, Terraform, GitHub Actions.

## Running locally

Requirements: Docker. The whole stack (PostgreSQL, a local Cognito emulator and the API) starts with:

```bash
cp .env.example .env   # fill in DB_PASSWORD and ADMIN_PASSWORD
docker compose up --build
```

- API: <http://localhost:8080>, Cognito emulator: <http://localhost:9229>
- User pool `local_medicase`, app client `medicaselocalclient000001`
- The administrator `admin@medicase.local` is created with `ADMIN_PASSWORD`
  (at least 8 characters with upper and lower case letters, a digit and a symbol).
- E-mails are not sent locally: confirmation codes (`ConfirmationCode`) and temporary passwords of invited
  users (`Password`) are kept in the emulator data, see
  `docker compose exec cognito cat /app/.cognito/db/local_medicase.json`.
- AI translations and speech need real AWS credentials (`AWS_ACCESS_KEY_ID`, `AWS_SECRET_ACCESS_KEY` in `.env`).

For development with `dotnet run` (.NET 8 SDK), start `db` and `cognito` from compose, add
`127.0.0.1 cognito` to `/etc/hosts` (tokens are issued for `http://cognito:9229`) and set:

```bash
export ConnectionStrings__MedicalDatabase="Host=localhost;Database=medicase;Username=medicase"
export ConnectionStrings__PublishedDatabase="Host=localhost;Database=medicase_published;Username=medicase"
export Database__Password="<db-password>"
export Cognito__UserPoolId=local_medicase Cognito__ClientId=medicaselocalclient000001
export Cognito__Issuer=http://cognito:9229/local_medicase Cognito__ServiceUrl=http://localhost:9229
```

## Deployment

1. Create an S3 bucket and a DynamoDB table for the Terraform state, copy `infra/backend.hcl.example`
   to `infra/backend.hcl` and fill it in.
2. Create the infrastructure:

   ```bash
   cd infra
   terraform init -backend-config=backend.hcl
   terraform apply
   ```

3. Set these repository variables in GitHub (values come from `terraform output`): `AWS_REGION`,
   `AWS_DEPLOY_ROLE_ARN`, `ECR_REPOSITORY_URL`, `ECS_CLUSTER`, `ECS_SERVICE`, `ECS_TASK_DEFINITION`,
   `ECS_SUBNETS` (comma separated) and `ECS_SECURITY_GROUP`.
4. Every push to `main` builds the image, runs the migrations as a one-off ECS task (`--migrate-only`)
   and deploys the new version. Without `AWS_DEPLOY_ROLE_ARN` the workflow only builds and tests.

The first administrator can be created by running the migration task once with `Seed__AdminEmail` and
`Seed__AdminPassword` overrides.

## Tests

```bash
dotnet test
```

## API overview

All endpoints need a Cognito access token (`Authorization: Bearer <token>`).

| Method | Endpoint | Role |
| --- | --- | --- |
| GET, PUT | `/api/Users/me` | any |
| GET | `/api/Learning/languages`, `/tree`, `/entity/{id}`, `/entity/{id}/dictionary`, `/files/{id}`, `/search` | any |
| GET, POST, PUT, DELETE | `/api/Entity`, PUT `/api/Entity/{id}/move`, GET `/api/Entity/search` | Moderator, Admin |
| GET | `/api/EntitiesGraphData`, `/api/EntitiesGraphData/tree`, `/api/EntityTypes` | Moderator, Admin |
| GET, POST, PUT, DELETE | `/api/EntityTranslations`, `/api/EntityDictionary`, `/api/EntityLanguages` | Moderator, Admin |
| GET, POST, DELETE | `/api/EntityTranslationFiles` | Moderator, Admin |
| POST | `/api/Ai/translate`, `/api/Ai/speech/{translationId}` | Moderator, Admin |
| GET, POST, PUT, DELETE | `/api/Users`, `/api/Groups` | Admin |
| GET | `/api/Synchronization/changes` | Admin |
| POST | `/api/Synchronization` | Admin |
| GET | `/health` | anonymous |
