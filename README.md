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

## Getting Started

### Prerequisites
- Docker (for Mac users)
- HTTPS setup for the server application

### Project Structure
- **Blazor App Solution**: Includes Client, Server, and Shared projects.
- **Main Solution**: Contains two API projects and the IdentityServer project.

### Running the Demo
1. **Configure Database Connections**:
   - Update the local database connection strings in the IdentityServer and Blazor App configurations.
   - For Mac users, set up Docker as required.
   - Note: Example APIs use in-memory data for simplicity.

2. **Run the Blazor App**:
   - Open the Blazor App solution.
   - Run the Server App over HTTPS.

3. **Run the Main Solution**:
   - Open the main solution (BlazorIDSPOC.sln).
   - Configure and run all three projects (both APIs and IdentityServer) using the "Multiple Startup Projects" option in Visual Studio.

### Authentication and Authorization
Ensure you are signed into the IdentityServer to access data routes in the Blazor App. Data fetching operates using tokens obtained from IdentityServer.
