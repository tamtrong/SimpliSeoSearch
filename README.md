# README - Simpli SEO Ranking Search

## Purpose of the Project
The Simpli SEO Ranking Search is an application designed to automate the process of searching specific keywords on popular search engines (Google and Bing) and retrieving the ranking positions for a given URL.
### Key Features
- Supports both **Google** and **Bing** search engines.
- Supported by NET 9.
---

## Project Structure
The solution is divided into four projects to create a clean architecture:

### 1. **Core Business Library (`SimpliSeoSearch.Core`)**
   - Contains core business logic, interfaces, and implementations for search engines and caching.

### 2. **Web API (`SimpliSeoSearch.API`)**
   - Provides RESTful endpoints to interact with the backend services.

### 3. **Blazor Web App (`SimpliSeoSearch.BlazorApp`)**
   - Frontend for user interaction, allowing users to input keywords, select search engines, and view results.

### 4. **Unit Tests (`SimpliSeoSearch.UnitTests`)**
   - Unit tests for services and controllers using `xUnit` and `Moq`.
---

## How to Run the Project
### Steps
1. Start the Web API project:
   ```bash
   cd SimpliSeoSearch.API
   dotnet run
   ```
2. Start the Blazor Web App:
   ```bash
   cd SimpliSeoSearch.BlazorApp
   dotnet run
   ```

## Future Improvements
- **Extend Search Engines:** Add support for other search engines like Yahoo or DuckDuckGo.
- **Monitoring and Analytics:** Add telemetry for tracking user interactions and search trends.
- .....
