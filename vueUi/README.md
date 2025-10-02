# MyChat Vue UI

A modern chat application interface built with Vue.js 3 and TypeScript. This application provides a responsive and intuitive chat interface that communicates with a backend API.

## Features

- Real-time chat interface
- Conversation management (create, delete, update)
- Message history viewing
- Responsive layout with sidebar conversation list
- TypeScript support for enhanced type safety
- Modern Vue.js 3 Composition API

## Recommended IDE Setup

[VSCode](https://code.visualstudio.com/) + [Volar](https://marketplace.visualstudio.com/items?itemName=Vue.volar) (and disable Vetur).

## Project Structure

```
src/
├── components/          # Vue components
│   ├── ActiveConversation.vue
│   ├── ConversationList.vue
│   ├── MessageItem.vue
│   └── MyChat.vue
├── services/           # API services
│   └── ConversationApiService.ts
├── store/             # State management
│   └── ConversationStore.ts
├── types/             # TypeScript interfaces
│   ├── Conversation.ts
│   ├── ConversationHeader.ts
│   └── Message.ts
└── assets/            # Static assets
```

## Technical Details

### API Integration

The application communicates with a backend API running at `https://localhost:7192`. The `ConversationApiService` handles all API communications with the following endpoints:

- GET `/conversation` - Retrieve conversation list
- GET `/conversation/{id}` - Get full conversation
- POST `/conversation` - Create new conversation
- DELETE `/conversation/{id}` - Delete conversation
- PUT `/conversation/{id}/header` - Update conversation title
- POST `/conversation/ask/{id}` - Send message

### Key Components

- `MyChat.vue`: Main application container component
- `ConversationList.vue`: Displays list of available conversations
- `ActiveConversation.vue`: Shows the current active conversation
- `MessageItem.vue`: Individual message display component

### Prerequisites

- Node.js (^20.19.0 || >=22.12.0)
- npm/yarn/pnpm

## Project Setup

```sh
npm install
```

### Compile and Hot-Reload for Development

```sh
npm run dev
```

### Type-Check, Compile and Minify for Production

```sh
npm run build
```

### Development Commands

```bash
# Start development server
npm run dev

# Type-check
npm run type-check

# Lint and fix files
npm run lint

# Format code with Prettier
npm run format
```

## Dependencies

- Vue.js 3.5.18
- TypeScript 5.8.0
- Vite 7.0.6
- ESLint & Prettier for code quality
- Vue DevTools for development

## Development Tools

The project includes several development tools:

- ESLint for code linting
- Prettier for code formatting
- Vue TypeScript compiler (vue-tsc)
- Vite for fast development and building
