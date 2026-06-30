# Side Quests

A full-stack web application built with C# and ASP.NET Core that encourages users to complete healthy and productive activities through AI-generated quest descriptions while tracking their progress.

🌐 **Live Demo:** https://sidequests.azurewebsites.net/

> **Note:** The live demo's AI quest generation may occasionally be unavailable due to OpenAI API quota limits. All other features remain functional.

## Features

- User authentication with ASP.NET Core Identity
- Create and manage activities
- AI-generated quest descriptions using OpenAI
- Track completed activities
- SQLite database with Entity Framework Core
- Deployed on Microsoft Azure

## Tech Stack

- C#
- ASP.NET Core (.NET)
- Blazor Server
- Entity Framework Core
- SQLite
- ASP.NET Core Identity
- Semantic Kernel
- OpenAI API
- Microsoft Azure

## Project Showcase
![Screenshot](/readme-assets/sample1.png)
![Screenshot](/readme-assets/sample2.png)
![Screenshot](/readme-assets/sample3.png)
![Screenshot](/readme-assets/sample4.png)
![Screenshot](/readme-assets/sample5.png)

## Running Locally

### Prerequisites

- .NET SDK
- OpenAI API key

Store your API key using **appsettings.json**:

```json
{
  "OpenAI": {
    "Model": "gpt-5-mini",
    "ApiKey": "YOUR_API_KEY"
  }
}
```

**Important:** Make sure your `Program.cs` reads the configuration using the same structure (around line 60):

```csharp
var model = builder.Configuration["OpenAI:Model"];
var apiKey = builder.Configuration["OpenAI:ApiKey"];
```

The application relies on these configuration values for AI-powered quest generation. If the API key is missing or invalid, AI features will fail with a quota or authentication error while the rest of the app continues to function normally.

Run the application:

```bash
dotnet run
```

## AI Quest Generation

Quest descriptions are generated using Microsoft's Semantic Kernel with the OpenAI Chat Completions API. An active OpenAI API key with available quota is required for this feature.


# Attributions
Side Quests Logo Attribution:  
<a href="https://www.textstudio.com/">Font generator</a>

Favicon Attribution:  
<a href="https://www.flaticon.com/free-icons/quest" title="quest icons">Quest icons created by Smashicons - Flaticon</a>