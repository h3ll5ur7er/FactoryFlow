# Contributing to Flow

## Development Philosophy

Flow follows strict software engineering principles to maintain high code quality and maintainability. This document outlines our development practices and guidelines.

## Test-Driven Development (TDD)

We follow a strict TDD approach for all non-UI components:

1. **Write the Test First**
   - Create a failing test that defines the desired behavior
   - Test should be clear, focused, and descriptive
   - Follow the naming convention: `MethodName_Scenario_ExpectedBehavior`

2. **Write the Implementation**
   - Write the minimum code required to make the test pass
   - Focus on functionality first, optimization later
   - Keep the implementation simple and straightforward

3. **Refactor**
   - Clean up the code while keeping tests green
   - Apply SOLID principles
   - Look for opportunities to improve design

### Exception for UI Components
- UI components are exempt from TDD due to the complexity of automated UI testing
- UI changes should be manually tested
- UI logic should be minimal, with business logic moved to testable view models

## SOLID Principles

All code must adhere to SOLID principles:

### Single Responsibility Principle
- Each class should have only one reason to change
- Keep classes focused and cohesive
- Extract separate concerns into their own classes

### Open/Closed Principle
- Classes should be open for extension but closed for modification
- Use interfaces and abstract classes to define extension points
- Avoid modifying existing code; extend instead

### Liskov Substitution Principle
- Derived classes must be substitutable for their base classes
- Maintain expected behavior when using inheritance
- Use composition over inheritance when appropriate

### Interface Segregation Principle
- Keep interfaces small and focused
- Clients should not depend on methods they don't use
- Split large interfaces into smaller, more specific ones

### Dependency Inversion Principle
- Depend on abstractions, not concrete implementations
- Use dependency injection
- Define clear interface boundaries

## Code Organization

### Project Structure
```
Flow/
├── Flow.Core/           # Core domain models and interfaces
├── Flow.App/            # Main application and UI
├── Flow.Tests/          # Test projects
└── Flow.Plugins/        # Plugin implementations
```

### Naming Conventions
- **Interfaces**: Prefix with 'I' (e.g., `IRecipe`)
- **Abstract Classes**: Prefix with 'Base' (e.g., `BaseNode`)
- **Test Classes**: Suffix with 'Tests' (e.g., `RecipeTests`)
- **Test Methods**: `MethodName_Scenario_ExpectedBehavior`

## Testing Guidelines

### Unit Tests
- One test class per production class
- Tests should be independent and isolated
- Use meaningful test names that describe the scenario
- Follow the Arrange-Act-Assert pattern

Example:
```csharp
[Fact]
public void GetGreetingAsync_WithValidName_ReturnsFormattedGreeting()
{
    // Arrange
    var service = new GreetingService();
    var name = "Test User";

    // Act
    var result = await service.GetGreetingAsync(name);

    // Assert
    Assert.Equal($"Hello, {name}! Welcome to Flow.", result);
}
```

### Integration Tests
- Test component interactions
- Focus on plugin system integration
- Test realistic usage scenarios

### Manual Testing
- Required for UI components
- Follow a consistent testing checklist
- Document any UI-specific test cases

## Pull Request Process

1. **Before Creating a PR**
   - Ensure all tests pass
   - Follow code style guidelines
   - Update documentation if needed

2. **PR Description**
   - Clearly describe the changes
   - Reference any related issues
   - List manual testing performed (for UI changes)

3. **Code Review**
   - All PRs require at least one review
   - Address review comments promptly
   - Keep discussions focused and professional

## Development Workflow

1. **Starting New Work**
   - Create a new branch from main
   - Write failing tests first
   - Implement the feature
   - Refactor as needed

2. **Committing Changes**
   - Write clear commit messages
   - Keep commits focused and logical
   - Follow conventional commit format

3. **Documentation**
   - Update relevant documentation
   - Include XML comments for public APIs
   - Keep README and other docs up to date

## Getting Help

- Check existing documentation
- Review related tests for examples
- Ask questions in project discussions
- Pair program when needed 