# Blazor Hosted WASM App hooked up with IdentityServer.

## About

This repo contains a Blazor Hosted WASM app which has Duende IdentityServer as the authentication authority following OAuth and OIDC flows.

It also contains 2 example APIs from which the Blazor App fetches data. The APIs have IdentityServer as the signing authority for the token to auhtorize the requests from the Blazor frontend.

IdentityServer in this case acts a central authority for authentication and authorization using OIDc protocols. It protects and manages 3 types of resources:
1. Identity Resources (Information about the user's Identity which is used to authenticate).
2. API Scopes for scope based access for the data provided by any API.
3. API Resources which contain information about all the APIs which use the IdentityServer as the signing authority.

The Blazor App has 3 projects a client application for UI components and a Server App to handle Authentication and Data Fetching and a Shared App for any common code. 
The Server App also handles secure token storage as the token is stored on the server and not the client side or the browser.

The Blazor App utilized the bff pattern to leverage the full power of the server App for secure Authentication and Authorization flows. 

## F5 Experience

This project has 2 solutions:
1. The Blazor App Solution (Client, Server and Shared)
2. The Main Solution which contains the 2 example API projects and the IdentityServer App.

To run the full demo
1. Add the local db connection string in the IdentityServer and the Blazor App (Spin up docker if on a mac). The Example API has in memory data for simplicity.
2. Open the Blazor App solution and run the Server App over Https.
3. Open the main solution and run all 3 projects (both APIs and IdentityServer) using multiple startup projects option.

All data routes are protected by authentication on the Blazor App, so first Sign in through IdentityServer and then the data fetching will work using the token from the IdentityServer.
