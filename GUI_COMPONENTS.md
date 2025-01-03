# Flow Graph UI Components

## Overview
This document describes the core UI components needed for Flow's graph visualization system. Each component follows MVVM architecture and will be implemented using Avalonia UI.

## Core Components

### 1. GraphCanvas
The main container and interaction surface for the graph visualization.

#### Properties
- ViewportSize: Size
- Scale: double (zoom level)
- Offset: Point (pan position)
- Nodes: ObservableCollection<NodeControl>
- Connections: ObservableCollection<ConnectionControl>

#### Responsibilities
- Manages viewport transformations (pan/zoom)
- Handles node placement and dragging
- Manages connection routing
- Implements selection system
- Provides snap-to-grid functionality

#### Events
- NodeAdded
- NodeRemoved
- ConnectionCreated
- ConnectionRemoved
- SelectionChanged

### 2. NodeControl
Visual representation of a graph node (Recipe, Splerger, or SubGraph).

#### Properties
- Position: Point
- Size: Size
- Title: string
- NodeType: NodeType (enum)
- IsSelected: bool
- Connectors: ObservableCollection<ConnectorControl>
- Multiplier: double

#### Visual States
- Normal
- Selected
- Invalid
- Active (processing)

#### Responsibilities
- Renders node content
- Manages connector layout
- Handles drag operations
- Displays node status and metrics

### 3. ConnectorControl
Input/Output points for node connections.

#### Properties
- Type: ConnectorType (Input/Output)
- ConnectionType: ConnectionType (Dynamic/Generic/Typed)
- Position: Point
- IsConnected: bool
- AllowMultipleConnections: bool
- AcceptedTypes: IEnumerable<Type>

#### Visual States
- Available
- Connected
- Invalid
- Highlighted (during connection creation)

#### Responsibilities
- Provides connection points
- Validates potential connections
- Shows connection state feedback
- Handles connection creation interaction

### 4. ConnectionControl
Visual representation of connections between nodes.

#### Properties
- SourceConnector: ConnectorControl
- TargetConnector: ConnectorControl
- PathGeometry: Geometry
- ConnectionType: ConnectionType
- IsValid: bool

#### Visual States
- Normal
- Selected
- Invalid
- Highlighted

#### Responsibilities
- Renders connection path
- Updates path geometry on node movement
- Shows connection type and state
- Handles selection and hover effects

### 5. GraphToolbar
Control panel for graph manipulation tools.

#### Components
- Node creation buttons
- Zoom controls (in/out/fit)
- Grid snap toggle
- Layout options
- View options (show/hide elements)

#### Responsibilities
- Provides quick access to common tools
- Shows current tool state
- Manages tool activation/deactivation

## Implementation Guidelines

### MVVM Pattern
- Each component should have corresponding:
  - ViewModel (business logic)
  - Model (data structure)
  - View (visual representation)

### Testing Approach
1. **ViewModel Tests**
   - Node positioning logic
   - Connection validation
   - Selection management
   - State transitions

2. **Visual Tests**
   - Component rendering
   - Interaction handling
   - Visual feedback
   - Layout calculations

### Performance Considerations
- Use virtualization for large graphs
- Implement connection path caching
- Optimize hit testing for selection
- Batch visual updates

### Accessibility
- Keyboard navigation support
- High contrast visual states
- Screen reader compatibility
- Configurable interaction modes

## Future Enhancements
- Mini-map navigation
- Custom node templates
- Advanced routing algorithms
- Performance optimizations
- Touch/pen input support 