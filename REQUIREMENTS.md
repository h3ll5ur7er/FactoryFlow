# Flow - Visual Factory Game Production Chain Editor

## Overview
Flow is a visual editor for designing and managing production chains commonly found in factory-building games like Factorio or Satisfactory. It allows users to create, visualize, and optimize complex production workflows through an intuitive node-based interface.

## Technical Stack
- **.NET 9.0**
  - Latest C# language features
  - Cross-platform runtime support
  - Native AOT compilation capabilities
- **Avalonia UI**
  - Cross-platform UI framework
  - Supports Windows, macOS, Linux
  - Web platform support via WebAssembly
  - MVVM architecture
- **Development Priorities**
  - Initial focus on functionality over performance
  - Clean, maintainable code structure
  - Extensible architecture for future optimizations

## Core Concepts

### Items and Stacks
- **Items**: Basic units that represent game resources
- **ItemStacks**: Combinations of an Item and its quantity
  
### Recipes
A recipe defines a production process with:
- Input ItemStacks (required resources)
- Output ItemStacks (produced items)
- Associated machine type
- Processing time
- Power consumption

### Node System

#### Node Types
1. **Recipe Nodes**
   - Represent production recipes
   - Dynamic number of input/output connectors based on recipe requirements
   - Visual representation of a production step
   - Include a multiplier for machine count/throughput calculations

2. **Splerger Nodes (Splitter/Merger)**
   - Used for flow control and graph organization
   - Help manage resource distribution and collection
   - Improve visual clarity of complex production chains

3. **SubGraph Nodes**
   - Represent nested production chains
   - Contain their own internal flow network
   - Expose configurable generic inputs and outputs
   - Enable hierarchical organization of complex processes

#### Connectors

Connectors come in three varieties:
1. **Dynamic Connectors**
   - Accept all connection types
   - No type restrictions
   - Maximum flexibility

2. **Generic Connectors**
   - Initially accept all types
   - Once connected, adopt and maintain the type of their first connection
   - Used primarily in SubGraph nodes

3. **Typed Connectors**
   - Restricted to specific item types
   - Used in Recipe nodes to ensure correct resource flow
   - Type is determined by the associated recipe

### Connection System
- Nodes can be freely positioned via drag-and-drop
- Output connectors can connect to input connectors
- Multiple connections per connector are supported
- Connections maintain type compatibility rules
- Visual feedback for valid/invalid connections

### Graph Structure
- Nodes and connections form a directed graph
- Support for nested graphs via SubGraph nodes
- Maintains type safety across connections
- Allows for complex production chain visualization

## Plugin System

### Architecture
- Modular design using .NET assembly loading
- Plugin discovery in designated directories
- Hot-reloading support for development
- Version compatibility checking

### Plugin Interface
```csharp
// Core interfaces that plugins must implement
public interface IGamePlugin
{
    string GameName { get; }
    Version PluginVersion { get; }
    IEnumerable<IItem> AvailableItems { get; }
    IEnumerable<IRecipe> AvailableRecipes { get; }
    IEnumerable<IMachine> AvailableMachines { get; }
}

public interface IItem
{
    string Identifier { get; }
    string DisplayName { get; }
    // Additional game-specific properties
}

public interface IRecipe
{
    string Identifier { get; }
    IEnumerable<ItemStack> Inputs { get; }
    IEnumerable<ItemStack> Outputs { get; }
    IMachine Machine { get; }
    TimeSpan ProcessingTime { get; }
    decimal PowerConsumption { get; }
}
```

### Plugin Requirements
- Must provide complete game data (items, recipes, machines)
- Support for custom resource types
- Optional custom calculation rules
- Localization support
- Plugin metadata and documentation

### Plugin Discovery and Loading
- Automatic discovery in app's plugin directory
- Manual plugin import support
- Plugin dependency resolution
- Version compatibility validation
- Graceful error handling for invalid plugins

## Production Planning

### Throughput Calculation
- Calculate required input resources for desired output quantities
- Determine optimal number of machines needed
- Support for items-per-minute calculations
- Consider recipe processing times and machine efficiency

### Node Multipliers
- Each node has an associated multiplier representing machine count
- Two operation modes:
  1. **Automatic**: Multiplier adjusts based on graph requirements
  2. **Manual**: User-defined multiplier for throughput limiting
- Real-time recalculation of resource requirements
- Visual feedback for bottlenecks and inefficiencies

## File Management
- Export production chains to shareable files
- Import shared production chain designs
- JSON-based file format for:
  - Graph structure and connections
  - Node configurations and positions
  - Plugin references and versions
  - Custom settings and metadata
- Version control friendly structure

## User Interaction
- Drag and drop node placement
- Visual connection creation between nodes
- Node repositioning for graph organization
- SubGraph creation and management
- Real-time validation of connections and configurations
- Throughput adjustment via node multipliers
- Plugin selection and management

## Future Considerations
- Performance optimization opportunities:
  - Caching of calculation results
  - Parallel processing for large graphs
  - Lazy loading of unused plugin data
- Enhanced visualization features
- Additional export formats
- Recipe optimization algorithms
- Integration with game modding APIs 