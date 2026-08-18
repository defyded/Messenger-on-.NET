# Messenger on .NET (Backend)

A cross-platform real-time messenger backend built with a focus on clean architecture, modern .NET practices, and scalable design.

## 🛠️ Tech Stack & Architecture

- **Framework:** .NET Core / ASP.NET Core Web API
- **Database:** PostgreSQL (via Entity Framework Core)
- **Real-time Communication:** SignalR (WebSockets)
- **Security:** JWT Authentication
- **Testing:** xUnit, Moq, and **Testcontainers** for automated integration testing with real database instances.
- **Containerization:** Docker

## 📐 Architecture & Principles
- **SOLID & Clean Code:** Built with high maintainability in mind.
- **RESTful API Design:** Predictable and secure endpoints for client applications.
- **Asynchronous Pipeline:** Heavy use of `async/await` to ensure high throughput under concurrent load.

## 📱 Client Application
The client for this messenger is cross-platform, written in **.NET MAUI** using the **MVVM** pattern. 
You can check out the client source code here: [Client-for-messenger](https://github.com/defyded/Client-for-messenger)
