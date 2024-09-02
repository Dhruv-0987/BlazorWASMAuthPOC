# Blazor Hosted WASM App with Duende IdentityServer Integration

## About
This repository houses a Blazor Hosted WASM application integrated with Duende IdentityServer, which serves as the authentication authority using OAuth and OIDC protocols. It features:

- **Two Example APIs**: These APIs serve data to the Blazor app, utilizing IdentityServer for token signing to authorize requests.
- **IdentityServer**: Serves as the central authority for authentication and authorization, managing:
  1. **Identity Resources**: User identity information for authentication.
  2. **API Scopes**: Scope-based access control for APIs.
  3. **API Resources**: Metadata for APIs using IdentityServer for token signing.

The Blazor app comprises three projects:
- **Client App**: Manages UI components.
- **Server App**: Handles authentication, data fetching, and secure token storage.
- **Shared App**: Contains common code utilized across the client and server apps.

The application employs the Backend-for-Frontend (BFF) pattern, enhancing the security of authentication and authorization processes.

![image](https://github.com/Dhruv-0987/BlazorWASMAuthPOC/assets/69164713/c2adf476-a2d4-49ca-9b5b-bcfedaed4919)

## Getting Started

### Prerequisites
- Docker (for Mac users)
- HTTPS setup for the server application

### Project Structure
The solution BlazorIDSPOC contains the following projects:
- The Blazor hosted app (client, server and shared)
- IdentityServer app
- 2 example APIs
- An Aspire App Host project
- An Aspire service defaults

### F5 experience

This app used the .NET 8 aspire App Host.

To run all projects locally just run the AppHost aspire project which will automatically run all the projects for you.

Note: just make sure docker is running for the SQL edge server.

### Authentication and Authorization
Ensure you are signed into the IdentityServer to access data routes in the Blazor App. Data fetching operates using tokens obtained from IdentityServer.
