# MediCase – Frontend

React client for **MediCase**, a multilingual web application for learning emergency medicine communication.
Learners choose the language they learn and their own language, go through content cards, patient interview
questions with audio recordings, glossaries and tests. Moderators edit the content in many languages and
administrators review and publish it.

The application was created during the *Programowanie zespołowe* (team programming) course in the 2022/2023
academic year, in cooperation with Collegium Medicum UMK, and was first presented in May 2023.
In 2024 it moved to Vite and AWS: sign-in with Amazon Cognito and hosting on S3 with CloudFront.
In 2025 it was updated to React 19 and Material UI 7.

The REST API lives in the [`backend`](../backend) directory.

## Features

- Learning view with the content tree, search, previous/next navigation and a drawer on phones
- Content cards with an optional translation into the learner's language
- Patient interview questions and pronunciation recordings
- Glossary for every section
- Tests with immediate feedback, final score and retry
- Moderator panel: tree editor that allows only valid element types, rich text editor (Quill) with live preview,
  file uploads, glossary editor, language list, hiding elements from learners
- **AI translations** and **speech generation** from the moderator editor
- Admin board: users, roles and groups with paging and search, review of pending changes and publishing
- Sign-in, registration with e-mail confirmation, password reset and invitations with **Amazon Cognito**
- Polish user interface

## Tech stack

React 19, Vite 6, TypeScript (store and network layer), Material UI 7, React Router 7, zustand, axios,
Quill 2 (react-quill-new), DOMPurify, Amazon Cognito, Vitest 3, React Testing Library and ESLint 9.

## Getting started

Requirements: Node.js 22 and the backend started with `docker compose up` in
[`backend`](../backend) (API and a local Cognito emulator).

```bash
npm install
npm run dev
```

The app runs at <http://localhost:3000>. Addresses of the API and Cognito are set in `.env.development`:

| Variable | Meaning |
| --- | --- |
| `VITE_API_URL` | API address, `/api` in production (same CloudFront domain) |
| `VITE_COGNITO_REGION` | Region of the Cognito user pool |
| `VITE_COGNITO_CLIENT_ID` | App client of the user pool |
| `VITE_COGNITO_ENDPOINT` | Optional endpoint of a local Cognito emulator |

## Tests and linting

```bash
npm test
npm run lint
```

## Deployment

The GitHub Actions workflow in [`.github/workflows/frontend.yml`](../.github/workflows/frontend.yml) lints, tests and
builds the app when a push to `main` or a pull request changes `frontend/`. On `main` it uploads the build to S3 and
invalidates CloudFront. The infrastructure is created with Terraform in [`backend/infra`](../backend/infra); set these
repository variables from its outputs: `AWS_REGION`, `AWS_DEPLOY_FRONTEND_ROLE_ARN`, `COGNITO_CLIENT_ID`,
`FRONTEND_BUCKET` and `CLOUDFRONT_DISTRIBUTION_ID`.

## Project structure

| Folder | Content |
| --- | --- |
| `src/components/learning` | Learning view: sidebar, content cards, questions, tests, glossary, audio |
| `src/components/moderator` | Moderator panel: tree, editor, files, glossary, AI buttons |
| `src/components/admin` | Admin board: users, groups, pagination |
| `src/components/user` | Login, registration, profile, language selection, review of changes |
| `src/components/layout`, `common`, `landing` | Layout, shared components and the landing page |
| `src/network` | API client, Cognito client and typed API functions |
| `src/state` | Session and language settings (zustand, saved in `localStorage`) |
| `src/lib` | Sign-in and token refresh, helpers, validation and hooks |
