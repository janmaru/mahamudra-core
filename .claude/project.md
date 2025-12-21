# Project Context - C# Clean Architecture with DDD

## Architecture Overview

This project follows **Clean Architecture** principles with **Domain-Driven Design (DDD)** and **SOLID** principles.

### Layer Structure
- **Domain Layer**: Core business logic, entities, value objects, domain events, aggregates
- **Application Layer**: Use cases, application services, DTOs, interfaces
- **Infrastructure Layer**: External concerns (database, APIs, file system)
- **Presentation Layer**: Controllers, views, API endpoints

### Dependencies Flow
Dependencies point inward: Presentation → Application → Domain
Infrastructure depends on Application (implements interfaces defined there)

## Domain-Driven Design (DDD) Principles

### Entities
- Must have identity (Id property)
- Encapsulate business logic and invariants
- Use private setters, expose behavior through methods
- Validate state in constructors and methods

```csharp
public class Order : Entity
{
    public OrderId Id { get; private set; }
    private List<OrderItem> _items = new();
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();
    
    private Order() { } // EF Core
    
    public void AddItem(Product product, int quantity)
    {
        // Business logic here
        if (quantity <= 0)
            throw new DomainException("Quantity must be positive");
            
        _items.Add(new OrderItem(product, quantity));
    }
}
```

### Value Objects
- No identity, defined by their attributes
- Immutable
- Implement equality based on values
- Examples: Money, Address, Email

```csharp
public sealed class Money : ValueObject
{
    public decimal Amount { get; }
    public string Currency { get; }
    
    private Money(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }
    
    public static Money Create(decimal amount, string currency)
    {
        // Validation
        return new Money(amount, currency);
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Amount;
        yield return Currency;
    }
}
```

### Aggregates
- Cluster of entities and value objects with a root entity
- Enforce consistency boundaries
- Only reference other aggregates by ID
- Modify only through the aggregate root

### Domain Events
- Represent something that happened in the domain
- Immutable
- Past tense naming (OrderPlacedEvent, PaymentProcessedEvent)

```csharp
public sealed record OrderPlacedEvent(OrderId OrderId, DateTime OccurredOn) : IDomainEvent;
```

### Repositories
- Defined as interfaces in Domain layer
- Implemented in Infrastructure layer
- Work with aggregate roots only
- Use collection-like interface (Add, Get, Remove)

```csharp
public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(OrderId id, CancellationToken cancellationToken = default);
    Task AddAsync(Order order, CancellationToken cancellationToken = default);
    void Update(Order order);
    void Remove(Order order);
}
```

## SOLID Principles

### Single Responsibility Principle (SRP)
- Each class should have one reason to change
- Separate commands from queries (CQRS pattern encouraged)
- Keep use cases focused and small

### Open/Closed Principle (OCP)
- Use abstractions and interfaces
- Extend behavior through inheritance or composition, not modification
- Use strategy pattern, decorator pattern when appropriate

### Liskov Substitution Principle (LSP)
- Derived classes must be substitutable for base classes
- Avoid breaking contracts in inheritance hierarchies
- Prefer composition over inheritance when in doubt

### Interface Segregation Principle (ISP)
- Many small, specific interfaces over one large interface
- Clients shouldn't depend on interfaces they don't use

### Dependency Inversion Principle (DIP)
- High-level modules don't depend on low-level modules
- Both depend on abstractions
- Use dependency injection throughout

## Coding Standards

### Naming Conventions
- **PascalCase**: Classes, methods, properties, public fields
- **camelCase**: Private fields with underscore prefix (`_orderRepository`)
- **Interfaces**: Prefix with `I` (IOrderRepository)
- **Async methods**: Suffix with `Async` (GetOrderAsync)

### Project Structure
```
src/
├── Domain/
│   ├── Entities/
│   ├── ValueObjects/
│   ├── Events/
│   ├── Exceptions/
│   └── Repositories/
├── Application/
│   ├── Commands/
│   ├── Queries/
│   ├── DTOs/
│   ├── Interfaces/
│   └── Behaviors/
├── Infrastructure/
│   ├── Persistence/
│   ├── Repositories/
│   └── Services/
└── Presentation/
    ├── Controllers/
    └── Middleware/
```

### Required Patterns

**Result Pattern**: Don't throw exceptions for expected failures
```csharp
public class Result<T>
{
    public bool IsSuccess { get; }
    public T Value { get; }
    public Error Error { get; }
}
```

**Specification Pattern**: For complex queries
```csharp
public interface ISpecification<T>
{
    Expression<Func<T, bool>> ToExpression();
    bool IsSatisfiedBy(T entity);
}
```

**Unit of Work Pattern**: For transaction management
```csharp
public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
```

### Commands and Queries (CQRS)

**Commands**: Change state, return Result or void
```csharp
public sealed record CreateOrderCommand(Guid CustomerId, List<OrderItemDto> Items) : ICommand<Result<Guid>>;
```

**Queries**: Read data, never modify state
```csharp
public sealed record GetOrderByIdQuery(Guid OrderId) : IQuery<OrderDto?>;
```

**Handlers**: One handler per command/query
```csharp
public sealed class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        // Implementation
    }
}
```

## Best Practices

### Always
- ✅ Use nullable reference types (`#nullable enable`)
- ✅ Validate input at boundaries (controllers, command handlers)
- ✅ Use strongly-typed IDs (OrderId, CustomerId instead of Guid)
- ✅ Make classes sealed by default
- ✅ Use async/await for I/O operations
- ✅ Use CancellationToken in async methods
- ✅ Add XML documentation for public APIs
- ✅ Use guard clauses for validation
- ✅ Throw domain exceptions for business rule violations
- ✅ Use readonly collections (IReadOnlyCollection, IReadOnlyList)

### Never
- ❌ Put business logic in controllers or infrastructure
- ❌ Reference Infrastructure from Domain
- ❌ Use public setters on entities
- ❌ Use primitive obsession (use value objects)
- ❌ Return null, use Result pattern or null object pattern
- ❌ Use static methods for business logic
- ❌ Expose collections directly (use IReadOnlyCollection)
- ❌ Use DTOs in Domain layer

### Dependency Injection
- Register services by layer in separate extension methods
- Use scoped lifetime for repositories and unit of work
- Use singleton for services without state
- Always inject interfaces, never concrete implementations

### Exception Handling
- Domain exceptions for business rule violations
- Application exceptions for use case failures
- Use global exception handling middleware
- Log exceptions appropriately

### Testing
- Unit tests for Domain logic (entities, value objects)
- Integration tests for repositories
- Use test builders for complex object creation
- Mock only external dependencies

## Entity Framework Core Conventions

- Use Fluent API in separate configuration classes
- Never use EF attributes in Domain entities
- Keep configurations in Infrastructure layer
- Use value conversions for value objects
- Configure owned entities for value objects within aggregates

```csharp
public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.Id);
        builder.Property(o => o.Id)
            .HasConversion(id => id.Value, value => new OrderId(value));
        
        builder.OwnsMany(o => o.Items, ib =>
        {
            ib.WithOwner().HasForeignKey("OrderId");
            ib.Property<int>("Id");
            ib.HasKey("Id");
        });
    }
}
```

## Code Quality

- Follow these principles religiously
- Refactor when code smells appear
- Keep methods small and focused (< 20 lines ideally)
- Use meaningful names that express intent
- Comment WHY, not WHAT
- Run static analysis tools (SonarQube, Roslyn analyzers)