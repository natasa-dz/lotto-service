# Lotto Service Project

## Description
The Lotto Service project consists of three components: Lotto Service, Lotto Client, and Lotto Host, built in C# using WPF (Windows Presentation Foundation). This system simulates a lottery service that allows users to interact with the lottery service, request results, and manage lottery tickets via a desktop application. The project is designed to handle lottery ticket generation, drawing, and the validation of winning numbers.

### Components
- **Lotto Service**: Core service responsible for managing lottery data, drawing lottery numbers, and storing ticket information.
- **Lotto Client**: A user interface built with WPF, allowing users to generate tickets, view results, and interact with the Lotto Service.
- **Lotto Host**: A host application that connects the Lotto Client with the Lotto Service.

## Features
- **Ticket Generation**: Users can generate random lottery tickets and check their numbers.
- **Number Drawing**: Simulates a lottery draw and compares ticket numbers to the drawn numbers.
- **Service Architecture**: The backend lotto service processes requests from the client and interacts with the data storage.

## Getting Started

### Prerequisites
- Visual Studio (with WPF and .NET support)
- C# programming knowledge
- .NET framework

### Installation
1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/lotto-service.git
   cd lotto-service
