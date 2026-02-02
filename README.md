# TypeCLI

A terminal-based typing practice game built with C# and .NET 8.0. Practice your typing speed and accuracy directly in your terminal with multiple game modes inspired by [Monkeytype](https://monkeytype.com/).

## Features

- **Multiple Game Modes** - Timed tests (15s, 30s, 60s, 120s), word count tests (10, 25, 50, 100 words), and quote mode
- **Real-time Statistics** - Live WPM and accuracy tracking as you type
- **Color-coded Feedback** - Instant visual feedback for correct and incorrect keystrokes
- **Results Breakdown** - Detailed post-game statistics including final WPM and accuracy

## Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

## Installation & Running

```bash
# Clone the repository
git clone https://github.com/yourusername/TypeCLI.git
cd TypeCLI

# Build the project
dotnet build

# Run the game
dotnet run --project src/TypeCLI/TypeCLI.csproj
```

## Game Modes

| Mode | Description |
|------|-------------|
| **Timed 15s** | Type as many words as possible in 15 seconds |
| **Timed 30s** | Type as many words as possible in 30 seconds |
| **Timed 60s** | Type as many words as possible in 60 seconds |
| **Timed 120s** | Type as many words as possible in 120 seconds |
| **Words 10** | Complete 10 words as fast as possible |
| **Words 25** | Complete 25 words as fast as possible |
| **Words 50** | Complete 50 words as fast as possible |
| **Words 100** | Complete 100 words as fast as possible |
| **Quote** | Type a random quote |

## Running Tests

```bash
dotnet test
```

## Project Structure

```
src/TypeCLI/
├── Core/           # Game engine, input handling, timer
├── Models/         # Game state, modes, results
├── Statistics/     # WPM and accuracy calculations
├── Content/        # Word and quote providers
├── UI/             # Terminal rendering with Spectre.Console
├── Data/           # Word lists and quotes (embedded resources)
└── Program.cs      # Entry point
```

## Tech Stack

- **C#** / **.NET 8.0**
- **[Spectre.Console](https://spectreconsole.net/)** - Rich terminal UI
