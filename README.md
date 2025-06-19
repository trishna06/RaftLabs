# RaftLabs Test Assignment

## 📋 Overview

This project demonstrates a clean and modular `.NET 8` implementation of a service component that interacts with a public API ([reqres.in](https://reqres.in)). It fetches and processes user data while implementing resilience, caching, and unit testing — simulating how real-world systems integrate with external data providers.

---

## Tech Stack

- **.NET 8** (C#)
- **HttpClient** with `IHttpClientFactory`
- **Polly** for Retry Policy
- **In-Memory Caching** via `IMemoryCache`
- **Clean Architecture**
- **NUnit** + **FluentAssertions** for Unit Testing
- **System.Text.Json** for JSON serialization

---

## Project Structure

RaftLabs.Console // Sample Console app for demo
RaftLabs.Application
├── Configuration // ApiSettings config class
├── DTOs // API response models
├── Services // ExternalUserService implementation
RaftLabs.Domain
├── Interfaces // Service interface (IExternalUserService)
├── Models // Internal User domain model
RaftLabs.Test // NUnit + FluentAssertions test project

---

## Features Implemented

### API Integration
- `GET /api/users?page={page}` — fetch paginated users
- `GET /api/users/{id}` — fetch user details

### Resilience with Polly
- 3 retries with exponential backoff using `WaitAndRetryAsync`

### In-Memory Caching
- Caches individual users for configurable duration

### Clean Architecture Principles
- Domain, Application, and Console layers separated
- Interface abstractions used for service access

### Unit Testing
- Mocked `HttpMessageHandler`
- Tests for successful deserialization, caching, and edge cases

---

## How to Run

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
- Visual Studio 2022+ or any compatible IDE

### Steps

# Restore dependencies
dotnet restore

# Build all projects
dotnet build

# Run the demo console app
dotnet run --project RaftLabs.Console

# Run unit tests
dotnet test


## Configuration

The API settings are defined in RaftLabs.Console/appsettings.json


## Output

1: George Bluth - george.bluth@reqres.in
2: Janet Weaver - janet.weaver@reqres.in
3: Emma Wong - emma.wong@reqres.in
4: Eve Holt - eve.holt@reqres.in
5: Charles Morris - charles.morris@reqres.in
6: Tracey Ramos - tracey.ramos@reqres.in
7: Michael Lawson - michael.lawson@reqres.in
8: Lindsay Ferguson - lindsay.ferguson@reqres.in
9: Tobias Funke - tobias.funke@reqres.in
10: Byron Fields - byron.fields@reqres.in
11: George Edwards - george.edwards@reqres.in
12: Rachel Howell - rachel.howell@reqres.in


## Design Decisions

- System.Text.Json was used for performance and built-in support in .NET Core.
- A UserDto model was introduced for clean separation from domain models.
- Polly handles transient network failures gracefully.
- Caching avoids unnecessary API calls, especially beneficial for GetUserByIdAsync.
