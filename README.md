# .NET Full Stack Project

- **[UI Repository](https://github.com/jsparks9/net-flashcard-ui)**: Frontend Angular Application
- **[API Repository](https://github.com/jsparks9/net-flashcard-api)**: Backend ASP.NET Core API

## Overview

This project is a full stack application built using .NET Core for the backend and Angular for the frontend. It includes a RESTful API and a dynamic user interface that allows interaction with the data.

## Features

- **Backend**: ASP.NET Core Web API
- **Frontend**: Angular 15
- **Database**: SQL Server (via Entity Framework Core)

## Getting Started

### Prerequisites

- .NET 8 SDK
- Node.js and npm
- Angular CLI
- Docker Desktop (containerizes SQL Server)

### Setup

1. **Clone the repository**:
   ```bash
   git clone https://github.com/jsparks9/net-flashcard-ui.git
   cd net-flashcard-ui
   ```

2. **Install frontend dependencies**:
   ```bash
   npm install
   ```

3. **Set up the database**:
   - Install Docker Desktop
   - Run MySQL_Setup.sh

4. **Run the application**:
   - **Backend**:
     ```bash
     dotnet run
     ```
   - **Frontend**:
     ```bash
     ng serve
     ```

### API Documentation

- **Base URL**: `https://localhost:7174/api`

- #### Auth
  
    - `POST /Auth/login` - Authenticates a user and returns a JWT token.
    - `POST /Auth/register` - Registers a new user.

- #### Card
  
    - `GET /Card` - Retrieves all cards.
    - `POST /Card` - Creates a new card.
    - `GET /Card/{id}` - Retrieves a specific card by ID.
    - `PATCH /Card/{id}` - Updates a specific card by ID.
    - `DELETE /Card/{id}` - Deletes a specific card by ID.

- #### Deck
  
    - `GET /Deck` - Retrieves all decks.
    - `POST /Deck` - Creates a new deck.
    - `GET /Deck/user/{username}` - Retrieves all decks for a specific user by username.
    - `GET /Deck/getmydecks` - Retrieves all decks for the authenticated user.
    - `GET /Deck/{id}` - Retrieves a specific deck by ID.
    - `PATCH /Deck/{id}` - Updates a specific deck by ID.
    - `DELETE /Deck/{id}` - Deletes a specific deck by ID.
    - `POST /Deck/deck/{id}` - Adds a card to a specific deck by deck ID.
    - `DELETE /Deck/deck/{deckId}/card/{cardId}` - Removes a specific card from a deck by deck ID and card ID.
