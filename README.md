# Todo API - .NET 7 + Docker + Azure CI/CD

A minimal .NET 7 Web API that demonstrates modern DevOps practices: containerization with Docker, automated CI/CD using GitHub Actions, and deployment to Azure App Service using a private Azure Container Registry (ACR).

---

## Features

- RESTful API built with .NET 7 and Minimal APIs
- Dockerized and published to Azure Container Registry (ACR)
- Deployed to Azure App Service for Containers (Linux)
- CI/CD pipeline using GitHub Actions
- Version endpoint to verify deployments

---

## Tech Stack

- **Backend:** ASP.NET Core 7 Web API
- **DevOps:** Docker · Azure Container Registry · Azure App Service · GitHub Actions
- **Languages:** C#
- **Other:** Git · GitHub · CI/CD · Linux Containers

---
## How to Run Locally with Docker

1. **Clone the repository**  
   ```bash
 # Replace 'yourusername' with your actual GitHub username
   git clone https://github.com/yourusername/TodoApi.git 
   cd TodoApi

---
## API Documentation – Swagger UI
This project uses Swagger (OpenAPI) to document and test API endpoints in a browser.

Access the Swagger UI

Local:
http://localhost:8080/swagger

Azure (if enabled in production):
https://your-app-name.azurewebsites.net/swagger