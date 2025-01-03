# Flow - Visual Factory Game Production Chain Editor

Flow is a modern, cross-platform application for designing and optimizing production chains commonly found in factory-building games like Factorio or Satisfactory. It provides an intuitive node-based interface for visualizing and planning complex manufacturing processes.

## Features

### Core Functionality
- **Visual Node Editor**: Drag-and-drop interface for creating and managing production chains
- **Real-time Validation**: Immediate feedback on connection validity and production chain integrity
- **Type-Safe Connections**: Ensures resources are connected correctly based on item types
- **Production Planning**: Design and optimize complex manufacturing processes

### Node System
- **Recipe Nodes**: Represent production recipes with inputs, outputs, and processing details
- **Splerger Nodes**: Manage resource distribution and collection for improved flow control
- **SubGraph Nodes**: Create hierarchical organization through nested production chains
- **Dynamic Connectors**: Flexible connection system with type validation and compatibility checks

### Plugin System
- **Game Integration**: Modular plugin system for supporting different factory-building games
- **Dynamic Loading**: Automatic discovery and loading of game plugins
- **Resource Management**: Complete game data support including:
  - Items and resources
  - Manufacturing machines
  - Production recipes
  - Processing times and power consumption

### Technical Features
- **Cross-Platform**: Built with .NET 9.0 and Avalonia UI
- **Modern Architecture**: MVVM design pattern with clean, maintainable code
- **Performance Optimized**: Connection path caching and efficient hit testing
- **Type Safety**: Strong type checking and runtime validation
- **Event-Driven**: Responsive UI with real-time updates

## System Requirements
- .NET 9.0 Runtime
- Supported Platforms:
  - Windows
  - macOS
  - Linux
  - Web (via WebAssembly)

## Getting Started
1. Install the .NET 9.0 Runtime
2. Download and install Flow
3. Place game plugins in the `plugins` directory
4. Launch Flow and start designing your production chains

## Development
The project follows strict software engineering principles:
- Test-Driven Development (TDD)
- SOLID principles
- Clean architecture
- Comprehensive error handling
- Performance optimization
- Accessibility compliance

## Project Structure
- `Flow.App`: Main application and UI components
- `Flow.Core`: Core domain models and interfaces
- `Flow.Tests`: Test projects
- `Flow.Games.*`: Game-specific plugin implementations

## Contributing
Contributions are welcome! Please refer to CONTRIBUTING.md for development guidelines and best practices.

## License
This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.