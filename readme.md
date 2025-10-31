# Introduction

Urban areas face growing challenges with traffic congestion and delays in public transport. These issues reduce efficiency, increase pollution, and frustrate commuters.  

To address this, the city of **Ringsted** has deployed a network of IoT sensors that continuously collect data on vehicle movements and traffic flow.  

This project proposes a **big data solution** that will:
- Collect and process real-time traffic data  
- Present insights to city planners for immediate decision-making  
- Store selected data for long-term analysis  

By leveraging data-driven insights, the project aims to optimize traffic flow, improve safety, and enhance public transportation efficiency.

# System Design

## ⚙️ Architecture Overview

The Smart Traffic system integrates IoT devices, real-time messaging, backend services, and a web-based dashboard. The solution is containerized using Docker for portability and scalability.

---

## 1. Hardware Layer
- **Arduino**: Collects sensor data such as vehicle detection.
- Publishes data via the **MQTT protocol** to the messaging system.

---

## 2. Messaging & Data Pipeline
- **MQTT Broker**: Lightweight protocol for IoT devices to publish/subscribe traffic data.
- **Apache Kafka**: Handles high-throughput, real-time data streaming and ensures reliable delivery.

---

## 3. Backend Services
- **ASP.NET Core API**:
  - Acts as the central gateway for data access.
  - Provides REST endpoints for the frontend and external systems.
  - Consumes Kafka streams to process incoming traffic data.

---

## 4. Frontend
- **Blazor (Server)**:
  - Provides a dashboard for city planners.
  - Displays real-time traffic flow, alerts, and analytics.
  - Allows interaction with both live and stored data.

---

## 5. Database Layer
- **MariaDB**:
  - Stores historical traffic data for long-term analysis.
  - Supports queries for trend detection, reporting, and optimization.

---

## 6. Deployment & Environment
- **Docker Containers**:
  - Each component (API, Blazor frontend, Kafka, MariaDB) runs in isolated containers.
  - Ensures portability, scalability, and simplified deployment.





