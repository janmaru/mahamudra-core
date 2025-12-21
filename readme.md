# Mahamudra Core

![alt text](pawel-czerwinski.jpg "Mahamudra Core")
[Photo by Paweł Czerwiński on Unsplash]

A comprehensive .NET library implementing **Railway Oriented Programming** - a powerful functional programming pattern for elegant error handling without exceptions.

[![NuGet](https://img.shields.io/nuget/v/Mahamudra.Core.svg)](https://www.nuget.org/packages/Mahamudra.Core/)
[![.NET Standard 2.1](https://img.shields.io/badge/.NET%20Standard-2.1-blue.svg)](https://docs.microsoft.com/en-us/dotnet/standard/net-standard)

## Features

✅ **Railway Oriented Programming** - Chain operations that can fail
✅ **No Exceptions** - Use exceptions only for exceptional cases, not control flow
✅ **No Nulls** - Explicit error handling with type safety
✅ **Functional Composition** - Map, Bind, Match, Tap methods
✅ **Async Support** - First-class async/await integration
✅ **Immutable Errors** - Error records with value-based equality
✅ **Custom Error Types** - Use Exception, string, or custom error classes
✅ **Clean Architecture** - Follows DDD and SOLID principles

## Installation

```bash
dotnet add package Mahamudra.Core
```

Or via NuGet Package Manager:
```
Install-Package Mahamudra.Core
```

## Quick Start

### Basic Usage

```csharp
using Mahamudra.Core.Patterns;
using Mahamudra.Core.Errors;

// Define validation functions that return Result
public Result<Person, string> ValidateName(Person person)
{
    if (string.IsNullOrWhiteSpace(person.Name))
        return Result<Person, string>.FailureResult("Name is required");
    return Result<Person, string>.SuccessResult(person);
}

public Result<Person, string> ValidateEmail(Person person)
{
    if (string.IsNullOrWhiteSpace(person.Email))
        return Result<Person, string>.FailureResult("Email is required");
    return Result<Person, string>.SuccessResult(person);
}

public Result<Person, string> ValidateAge(Person person)
{
    if (person.Age < 18)
        return Result<Person, string>.FailureResult("Must be 18 or older");
    return Result<Person, string>.SuccessResult(person);
}

// Chain validations - stops at first error
var result = Result<Person, string>.SuccessResult(person)
    .Bind(ValidateName)
    .Bind(ValidateEmail)
    .Bind(ValidateAge);

// Safe pattern matching - forces handling both paths
var response = result.Match(
    onSuccess: p => Ok(new PersonDto(p.Name, p.Email)),
    onFailure: errors => BadRequest(errors)
);
```

### Using Error Records

```csharp
using Mahamudra.Core.Errors;

public Result<Person, Error> CreatePerson(CreatePersonRequest request)
{
    if (string.IsNullOrEmpty(request.Name))
        return Result<Person, Error>.FailureResult(
            Error.Validation("Name is required"));

    if (request.Age < 18)
        return Result<Person, Error>.FailureResult(
            Error.BadRequest("Must be 18 or older"));

    var person = new Person(request.Name, request.Email, request.Age);
    return Result<Person, Error>.SuccessResult(person);
}

// With field-level validation errors
public Result<Person, ValidationError> ValidatePerson(Person person)
{
    var fieldErrors = new Dictionary<string, string[]>();

    if (string.IsNullOrEmpty(person.Name))
        fieldErrors["Name"] = new[] { "Name is required", "Name must be at least 2 characters" };

    if (string.IsNullOrEmpty(person.Email))
        fieldErrors["Email"] = new[] { "Email is required", "Invalid email format" };

    if (fieldErrors.Any())
        return Result<Person, ValidationError>.FailureResult(
            ValidationError.ForFields(fieldErrors, "Person validation failed"));

    return Result<Person, ValidationError>.SuccessResult(person);
}
```

## Core API

### Creating Results

```csharp
// Factory methods
var success = Result<Person, string>.SuccessResult(person);
var failure = Result<Person, string>.FailureResult("Error message");
var multipleErrors = Result<Person, string>.FailureResult(new List<string> { "Error 1", "Error 2" });

// Constructor-based (classic)
var success = new Success<Person, string>(person);
var failure = new Failure<Person, string>("Error message");
```

### Match - Safe Value Extraction

Forces you to handle both success and failure cases:

```csharp
var result = ValidatePerson(person);

// Pattern matching - compiler ensures both paths handled
var message = result.Match(
    onSuccess: p => $"Welcome {p.Name}!",
    onFailure: errors => $"Validation failed: {string.Join(", ", errors)}"
);

// Use in API controllers
return result.Match(
    onSuccess: p => Ok(p),
    onFailure: errors => BadRequest(errors)
);
```

### Map - Transform Success Value

Transform the value inside a successful result:

```csharp
var result = Result<Person, string>.SuccessResult(person)
    .Map(p => p.Name)                    // Result<string, string>
    .Map(name => name.ToUpper())         // Result<string, string>
    .Map(name => name.Length);           // Result<int, string>

// If any step fails, error propagates automatically
var nameLength = result.Match(
    onSuccess: length => length,
    onFailure: _ => 0
);
```

### Bind - Chain Operations (Railway Switching)

Chain operations that return Result:

```csharp
var result = GetUser(userId)                    // Result<User, Error>
    .Bind(user => ValidateUser(user))           // Result<User, Error>
    .Bind(user => SendWelcomeEmail(user))       // Result<User, Error>
    .Bind(user => CreateUserAccount(user))      // Result<User, Error>
    .Map(user => user.Id);                      // Result<Guid, Error>

// Railway stops at first error
if (result.IsFailure)
{
    // Error from any step in the chain
    var error = result.Messages[0];
}
```

### Tap - Side Effects

Execute side effects without breaking the chain:

```csharp
var result = ValidatePerson(person)
    .Tap(p => _logger.LogInformation($"Validated person: {p.Name}"))
    .Bind(p => SavePerson(p))
    .Tap(p => _logger.LogInformation($"Saved person: {p.Id}"))
    .Map(p => p.Id);
```

### Switch - Execute Actions

Execute different actions based on success/failure:

```csharp
result.Switch(
    onSuccess: person => Console.WriteLine($"Success: {person.Name}"),
    onFailure: errors => Console.WriteLine($"Failed: {string.Join(", ", errors)}")
);
```

### Async Support

```csharp
// MapAsync - async transformation
var result = await GetUserAsync(userId)
    .MapAsync(async user => await EnrichUserDataAsync(user));

// BindAsync - async chaining
var result = await GetUserAsync(userId)
    .BindAsync(async user => await ValidateUserAsync(user))
    .BindAsync(async user => await SaveUserAsync(user));
```

## Error Types

### Built-in Error Record

```csharp
// Static factory methods for common HTTP errors
Error.Validation("description", "message");     // 400
Error.BadRequest("description", "message");     // 400
Error.Unauthorized("description", "message");   // 401
Error.Forbidden("description", "message");      // 403
Error.NotFound("description", "message");       // 404
Error.Conflict("description", "message");       // 409
Error.Internal("description", "message");       // 500

// Custom error codes
new Error(418, "I'm a teapot", "Cannot brew coffee");
```

### ValidationError with Field-Level Errors

```csharp
// Single field error
var error = ValidationError.ForField("Email",
    "Email is required",
    "Invalid email format");

// Multiple field errors
var fieldErrors = new Dictionary<string, string[]>
{
    ["Name"] = new[] { "Required", "Min length 2" },
    ["Email"] = new[] { "Required", "Invalid format" },
    ["Age"] = new[] { "Must be 18 or older" }
};
var error = ValidationError.ForFields(fieldErrors, "Validation failed");

// Check for field errors
if (error.HasFieldErrors)
{
    foreach (var field in error.FieldErrors)
    {
        Console.WriteLine($"{field.Key}: {string.Join(", ", field.Value)}");
    }
}
```

### Custom Error Types

Use any type as your error type:

```csharp
// With Exception
Result<Person, Exception> ValidateAge(Person p)
{
    if (p.Age < 18)
        return Result<Person, Exception>.FailureResult(
            new ValidationException("Must be 18 or older"));
    return Result<Person, Exception>.SuccessResult(p);
}

// With custom domain error
public class DomainError
{
    public string Code { get; }
    public string Message { get; }

    public static DomainError InvalidAge =>
        new("INVALID_AGE", "Age must be between 0 and 150");
}

Result<Person, DomainError> ValidatePerson(Person p)
{
    if (p.Age < 0 || p.Age > 150)
        return Result<Person, DomainError>.FailureResult(DomainError.InvalidAge);
    return Result<Person, DomainError>.SuccessResult(p);
}
```

## Advanced Patterns

### Accumulating Errors

Use the `Acc` extension to collect multiple errors:

```csharp
var result = IsNull(input, "input")
    .Acc(_ => IsEmpty(input, "input"))
    .Acc(_ => IsValid(input, "input"));

// All errors collected, not just the first one
```

### Railway in Clean Architecture

```csharp
// Controller
[HttpPost]
public async Task<IActionResult> CreatePerson([FromBody] CreatePersonRequest request)
{
    var command = new CreatePersonCommand(request.Name, request.Email, request.Age);

    var result = await _mediator.Send(command);

    return result.Match(
        onSuccess: person => Ok(new PersonResponse(person)),
        onFailure: error => error.Code switch
        {
            400 => BadRequest(error),
            404 => NotFound(error),
            409 => Conflict(error),
            _ => StatusCode(500, error)
        }
    );
}

// Command Handler
public class CreatePersonCommandHandler
    : IRequestHandler<CreatePersonCommand, Result<Person, Error>>
{
    public async Task<Result<Person, Error>> Handle(
        CreatePersonCommand command,
        CancellationToken cancellationToken)
    {
        return await ValidatePerson(command)
            .BindAsync(async cmd => await CreatePersonAsync(cmd))
            .BindAsync(async person => await SavePersonAsync(person))
            .TapAsync(async person => await PublishEventAsync(person));
    }
}
```

## Guard Extensions

![alt text](mitchell-ng-liang.jpg "Guard")
[Photo by Mitchell Ng Liang an on Unsplash]

Additional package with guard clause extensions using Railway pattern.

### Installation

```bash
dotnet add package Mahamudra.Guard
```

### Usage

```csharp
using Mahamudra.Guard;

public Result<User, Error> CreateUser(string name, string email, int age)
{
    return Guard.Check
        .IsNullOrEmpty(name, nameof(name))
        .Bind(_ => Guard.Check.IsNullOrEmpty(email, nameof(email)))
        .Bind(_ => Guard.Check.IsNegativeOrZero(age, nameof(age)))
        .Map(_ => new User(name, email, age));
}
```

### Available Guards

```csharp
Guard.Check.IsNull(input, "parameter");
Guard.Check.IsNullOrEmpty(input, "parameter");
Guard.Check.IsNullOrWhiteSpace(input, "parameter");
Guard.Check.IsNegative(input, "parameter");
Guard.Check.IsNegativeOrZero(input, "parameter");
Guard.Check.IsZero(input, "parameter");
Guard.Check.IsOutOfRange(input, min, max, "parameter");
Guard.Check.IsDefault(input, "parameter");
```

### Custom Guards

```csharp
public static class CustomGuard
{
    public static Result<string, Error> IsValidEmail(
        this IGuard guard,
        string input,
        string parameter)
    {
        var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");

        if (!emailRegex.IsMatch(input))
            return Result<string, Error>.FailureResult(
                Error.Validation($"Invalid email format for {parameter}"));

        return Result<string, Error>.SuccessResult(input);
    }
}

// Usage
var result = Guard.Check
    .IsNullOrEmpty(email, nameof(email))
    .Bind(_ => Guard.Check.IsValidEmail(email, nameof(email)));
```

## Best Practices

### ✅ Do

- Use `Result` for expected failures (validation, business rules)
- Use `Match` to safely extract values
- Chain operations with `Bind` and `Map`
- Use specific error types (`Error`, `ValidationError`, custom errors)
- Keep constructor exceptions for programmer errors only
- Use factory methods (`SuccessResult`, `FailureResult`)

### ❌ Don't

- Don't throw exceptions for business logic failures
- Don't access `Value` or `Messages` directly without `Match`
- Don't use `Result` for unexpected failures (use exceptions)
- Don't mix error types in the same operation chain
- Don't ignore the `IsFailure` flag

## Philosophy

This library follows these principles:

1. **Make illegal states unrepresentable** - Use the type system to prevent errors
2. **Railway Oriented Programming** - Operations either stay on the success track or switch to the failure track
3. **No exceptions for control flow** - Exceptions for exceptional cases only
4. **Explicit is better than implicit** - Force developers to handle errors
5. **Composition over inheritance** - Functional composition with Map/Bind
6. **Clean Architecture** - Domain-driven design and SOLID principles

## Requirements

- .NET Standard 2.1 or higher
- C# 9.0 or higher

## License

MIT

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## Resources

- [Railway Oriented Programming](https://fsharpforfunandprofit.com/rop/) by Scott Wlaschin
- [Domain Modeling Made Functional](https://pragprog.com/titles/swdddf/domain-modeling-made-functional/)
- [Functional Programming in C#](https://www.manning.com/books/functional-programming-in-c-sharp)

---

**Version**: 5.0.0
**Author**: marujan
**Repository**: [github.com/janmaru/mahamudra-core](https://github.com/janmaru/mahamudra-core)
