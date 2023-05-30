# MediCase – Frontend

React client for **MediCase**, a multilingual web application for learning emergency medicine communication.
Learners choose the language they learn and their own language, go through content cards, patient interview
questions with audio recordings, glossaries and tests. Moderators edit the content in many languages and
administrators review and publish it.

The application was created during the *Programowanie zespołowe* (team programming) course in the 2022/2023
academic year, in cooperation with Collegium Medicum UMK, and was first presented in May 2023.

The REST API lives in the [MediCase-Backend](https://github.com/emilgruzalski/MediCase-Backend) repository.

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
- Polish user interface

## Tech stack

React 18, Create React App, TypeScript (store and network layer), Material UI 5, React Router 6, zustand,
axios, React Quill, DOMPurify, Jest and React Testing Library.

## Getting started

Requirements: Node.js 18 and the running [MediCase API](https://github.com/emilgruzalski/MediCase-Backend).

```bash
npm install
npm start
```

The app runs at <http://localhost:3000>. The API address is set in `.env.development`
(`REACT_APP_API_URL`, by default `http://localhost:5140/api`).

## Tests

```bash
npm test
```

## Build and deployment

```bash
npm run build
```

The GitHub Actions workflow in `.github/workflows` builds the app on every push to `main` and deploys the
`build` folder to Azure Web App. It needs the `REACT_APP_API_URL` repository variable and the
`AZUREAPPSERVICE_PUBLISHPROFILE` secret.

## Project structure

| Folder | Content |
| --- | --- |
| `src/components/learning` | Learning view: sidebar, content cards, questions, tests, glossary, audio |
| `src/components/moderator` | Moderator panel: tree, editor, files, glossary, AI buttons |
| `src/components/admin` | Admin board: users, groups, pagination |
| `src/components/user` | Login, registration, profile, language selection, review of changes |
| `src/components/layout`, `common`, `landing` | Layout, shared components and the landing page |
| `src/network` | API client and typed API functions |
| `src/state` | Session and language settings (zustand, saved in `localStorage`) |
| `src/lib` | Helpers, validation and hooks |
