# New Feature Implementation Guide

Below is a guide for implementing new features at the backend.
Solution follows the V-Architecture pattern that means new features should be located in the separated folder, inside the `Features` folder.

There are few classes that can be created by you to implement new features:  
1. Entity (Infrastructure) - Represents the data model and is responsible for defining the structure of the data in the database
2. Configuration (Infrastructure) - Responsible for configuring the entity and its relationships with other entities
3. Constants (Infrastructure) - Contains constant values used throughout the feature
4. Dto (WebAPI) - Different view of the entity that is used for data transfer between the client and the server
5. Mapper (WebAPI) - Responsible for mapping between the entity and the DTO
6. Command/Query (WebAPI) - Parameters that are passed to the handler for executing the necessary logic
7. Handler (WebAPI) - Responsible for handling the command or query and executing the necessary logic
8. Validator (WebAPI) - Responsible for validating the command, query and dto before handler execution
9. Endpoints (WebAPI) - Responsible for handling the HTTP requests and responses, and calling the appropriate handler for executing the necessary logic
10. Tests (IntegrationTests) - Responsible for testing the feature and ensuring that it works as expected
11. ValidationTests (UnitTests) - Responsible for testing the validation logic of the feature and ensuring that it works as expected

> [!NOTE]
> You can create additional classes if needed, but the above classes are the most common ones used for implementing new features

## 1. Infrastructure
### 1. Entity
Example of the class:
```csharp
namespace PhoenixKC.Infrastructure.Features.Name;

/// <remarks>
/// Configuration of the entity located in <see cref="NameConfiguration"/>
/// </remarks>
public sealed class NameEntity
{
    //Value properties
    public Guid Id { get; set; }
    public required string Title { get; set; }

    //Navigation properties
    public List<OtherEntity> OtherEntities { get; set; } = [];
}
```

> [!NOTE]
> Name is the name of the feature, and it should be replaced with the actual name of the feature you are implementing.

Value properties are the properties that are stored in the database, while navigation properties are the properties that are used to navigate to other entities.
All properties configuration must be done in the separated configuration class.

### 2. Configuration
Example of the class:
```csharp
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PhoenixKC.Infrastructure.Features.Name;

public sealed class NameConfiguration : IEntityTypeConfiguration<NameEntity>
{
    #region Interfaces
    public void Configure(EntityTypeBuilder<NameEntity> builder)
    {
        builder.ToTable("Name");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Title).IsRequired().HasMaxLength(NameConstants.TitleMaxLength);
        builder.HasMany(x => x.OtherEntities).WithOne(x => x.NameEntity).HasForeignKey(x => x.NameEntityId);
    }
    #endregion
}
```

> [!NOTE]
> Name is the name of the feature, and it should be replaced with the actual name of the feature you are implementing.

> [!TIP]
> If you want to implement M <=> M relationship, consider creating a join entity that will represent the relationship between the two entities. 
> The join entity should have its own configuration class and should be located in the same folder as the other entities.

Configuration class is responsible for configuring the entity and its relationships with other entities. 
It is also responsible for defining the table name, primary key, and any other constraints that are needed for the entity.

> [!IMPORTANT]
> All constants used in the configuration class should be defined in the Constants class, and not hardcoded in the configuration class.
> They can be used by validation classes as well, so it is important to keep them in a separate class.

### 3. Constants
Example of the class:
```csharp
namespace PhoenixKC.Infrastructure.Features.Name;

public static class NameConstants
{
    public const int TitleMaxLength = 100;
}
```

> [!NOTE]
> Name is the name of the feature, and it should be replaced with the actual name of the feature you are implementing.

Constants class is responsible for defining constant values used throughout the feature.
They can be used in the configuration class, validation classes, and any other classes that need to use constant values.

### 4. DbContext (Optional)
Example of the class:
```csharp
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PhoenixKC.Infrastructure.Features.Auth;
using PhoenixKC.Infrastructure.Features.Name;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace PhoenixKC.Infrastructure;

public sealed class PhoenixDbContext(
    DbContextOptions<PhoenixDbContext> options
) : IdentityDbContext<PhoenixUserEntity, IdentityRole<Guid>, Guid>(options)
{
    #region Instance
    public DbSet<NameEntity> Names { get; set; } = null!; //Init by EFCore
    #endregion

    #region Base
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(PhoenixDbContext).Assembly);
    }
    #endregion
}
```

> [!NOTE]
> Name is the name of the feature, and it should be replaced with the actual name of the feature you are implementing.

After creating the entity and configuration classes, you can add the DbSet property to the DbContext class.
It can be used to direct access of the all entities stored in the database.
This is optional, because table will be created automatically by the EF Core anyway.

## 2. WebAPI
### 1. Dto
Example of the class:
```csharp
namespace PhoenixKC.WebAPI.Features.Name.Dtos;

public sealed class NameDto
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
}
```

> [!NOTE]
> Name is the name of the feature, and it should be replaced with the actual name of the feature you are implementing.

Dto class is responsible for defining the data transfer object that will be used to transfer data between the client and the server.
This class represents a different view of the entity, and it can be used to hide certain properties of the entity that should not be exposed to the client.

> [!NOTE]
> There can be multiple Dto classes

### 2. Mapper
Example of the class:
```csharp
using Riok.Mapperly.Abstractions;
using PhoenixKC.WebAPI.Features.Name.Dtos;
using PhoenixKC.Infrastructure.Features.Name;

namespace PhoenixKC.WebAPI.Features.Name;

/// <summary>
/// Contains extension methods for converting between <see cref="NameEntity"/> and <see cref="NameDto"/>
/// </summary>
[Mapper] public static partial class NameMapper
{
    /// <summary>
    /// Converts <paramref name="entity"/> to <see cref="NameDto"/>
    /// </summary>
    /// <param name="entity">The entity to convert</param>
    /// <returns>The converted dto</returns>
    public static partial NameDto ToDto(this NameEntity entity); //Source generated by Mapperly

    /// <summary>
    /// Converts <paramref name="entities"/> to <see cref="IEnumerable{NameDto}"/>
    /// </summary>
    /// <param name="entities">The enumerable of entities to convert</param>
    /// <returns>New enumerable with converted dtos</returns>
    public static partial IEnumerable<NameDto> ToDto(this IEnumerable<NameEntity> entities); //Source generated by Mapperly

    /// <summary>
    /// Converts <paramref name="dto"/> to <see cref="NameEntity"/>
    /// </summary>
    /// <param name="dto">The dto to convert</param>
    /// <returns>The converted entity</returns>
    public static partial NameEntity ToEntity(this NameDto dto); //Source generated by Mapperly

    /// <summary>
    /// Converts <paramref name="dtos"/> to <see cref="IEnumerable{NameEntity}"/>
    /// </summary>
    /// <param name="dtos">The enumerable of dtos to convert</param>
    /// <returns>New enumerable with converted entities</returns>
    public static partial IEnumerable<NameEntity> ToEntity(this IEnumerable<NameDto> dtos); //Source generated by Mapperly

    /// <summary>
    /// Assigns properties of the <paramref name="entity"/> to <paramref name="dto"/>
    /// </summary>
    /// <param name="entity">The source entity</param>
    /// <param name="dto">The destination dto</param>
    public static partial void MapToDto(this NameEntity entity, NameDto dto); //Source generated by Mapperly

    /// <summary>
    /// Assigns properties of the <paramref name="dto"/> to <paramref name="entity"/>
    /// </summary>
    /// <param name="dto">The source dto</param>
    /// <param name="entity">The destination entity</param>
    public static partial void MapToEntity(this NameDto dto, NameEntity entity); //Source generated by Mapperly

    /// <summary>
    /// Creates a query that retrieves neccessary properties from the database and maps them to a new <see cref="NameDto"/>
    /// </summary>
    /// <remarks>
    /// Skips creation of <see cref="NameEntity"/> and maps the properties directly to <see cref="NameDto"/>.
    /// This is more efficient than retrieving the entities and then mapping them to dtos.
    /// But it works only for the simple queries.
    /// </remarks>
    /// <param name="query">The query</param>
    /// <returns>New query with <see cref="NameDto"/> items</returns>
    public static partial IQueryable<NameDto> ProjectToDto(this IQueryable<NameEntity> query); //Source generated by Mapperly
}
```

> [!NOTE]
> Name is the name of the feature, and it should be replaced with the actual name of the feature you are implementing.

> [!IMPORTANT]
> Use this template class for the your mappers. You only have to put your own names.

> [!WARNING]
> Leave signature of the methods unchanged. Though, you can add attributes, change access modifiers (private for example) and add extra members

Mapper provides ways to convert between entity and dto. About Mapperly here: https://mapperly.riok.app/docs/intro/

### 3. Command/Query
Example of the command:
```csharp
using Mediator;
using FluentResults;
using PhoenixKC.WebAPI.Features.Name.Dtos;

namespace PhoenixKC.WebAPI.Features.Name.Handlers;

public sealed record class CreateNameCommand(NameDto Name) : IRequest<Result>;
```

Example of the query:
```csharp
using Mediator;
using FluentResults;
using PhoenixKC.WebAPI.Features.Name.Dtos;

namespace PhoenixKC.WebAPI.Features.Name.Handlers;

public sealed record class GetNameQuery(Guid Id) : IRequest<Result<NameDto>>;
```

> [!NOTE]
> Name is the name of the feature, and it should be replaced with the actual name of the feature you are implementing.

> [!IMPORTANT]
> Commands/Queries must implement `IRequest<Result>` or `IRequest<Result<T>>` interface from Mediator package.

Commands/Queries defines parameters for the handler.
Command is used for creating, updating or deleting data, while Query is used for retrieving data.

### 4. Handler
Example of the class:
```csharp
using Mediator;
using FluentResults;
using PhoenixKC.Infrastructure;

namespace PhoenixKC.WebAPI.Features.Name.Handlers;

public sealed class CreateNameHandler(PhoenixDbContext thisDbContext) : IRequestHandler<CreateNameCommand, Result>
{
    #region Interfaces
    public async ValueTask<Result> Handle(CreateNameCommand request, CancellationToken cancellationToken)
    {
        request.Name.Id = Guid.Empty;
        await thisDbContext.Name.AddAsync(request.Name.ToEntity(), cancellationToken);
        if(await thisDbContext.SaveChangesAsync(cancellationToken) == 0)
        {
            return Result.Fail("");
        }
        return Result.Ok();
    }
    #endregion
}
```

> [!NOTE]
> Name is the name of the feature, and it should be replaced with the actual name of the feature you are implementing.

> [!IMPORTANT]
> Handler must implement `IRequestHandler<TRequest, TResult>` interface from Mediator package.

Handler is responsible for handling the command or query and executing the necessary logic.

> [!IMPORTANT]
> Every logic must have separated Command/Query and Handler

> [!IMPORTANT]
> `TResult` must match the type parameter of `IRequest<Result>` or `IRequest<Result<T>>` interace of the Command/Query

### 5. Validator
Example of the class:
```csharp
using FluentValidation;
using PhoenixKC.WebAPI.Features.Name.Dtos;
using PhoenixKC.Infrastructure.Features.Name;

namespace PhoenixKC.WebAPI.Features.Name.Validators;

public sealed class NameDtoValidator : AbstractValidator<NameDto>
{
    public NameDtoValidator()
    {
        base.RuleFor(h => h.Title).MaximumLength(NameConstants.TitleMaxLength);
    }
}
```

> [!NOTE]
> Name is the name of the feature, and it should be replaced with the actual name of the feature you are implementing.

> [!IMPORTANT]
> Validator must implement `AbstractValidator<T>` interface from FluentValidation package.

Validator validates properties of the dtos, commands, queries before execution of the handler.

### 6. Endpoints
Example of the class:
```csharp
using Mediator;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using PhoenixKC.WebAPI.Extensions;
using FluentResults.Extensions.AspNetCore;
using PhoenixKC.WebAPI.Features.Name.Dtos;
using PhoenixKC.WebAPI.Features.Name.Handlers;

namespace PhoenixKC.WebAPI.Features.Name;

public sealed class NameEndpoints : IEndpointsProvider
{
    #region Static
    public static async Task<IResult> GetAllName(
        [FromServices] IMediator mediator, 
        CancellationToken cancellationToken
    )
    {
        Result<IEnumerable<NameDto>> result = await mediator.Send(new GetAllNameQuery(), cancellationToken);
        return result.ToActionResult().ToMvcResult();
    }
    public static async Task<IResult> GetName(
        [FromRoute] Guid id, 
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken
    )
    {
        Result<NameDto> result = await mediator.Send(new GetNameQuery(id), cancellationToken);
        return result.ToActionResult().ToMvcResult();
    }
    public static async Task<IResult> CreateName(
        [FromBody] NameDto dto,
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken
    )
    {
        Result result = await mediator.Send(new CreateNameCommand(dto), cancellationToken);
        return result.ToActionResult().ToMvcResult();
    }
    public static async Task<IResult> UpdateName(
        [FromBody] NameDto dto,
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken
    )
    {
        Result result = await mediator.Send(new UpdateNameCommand(dto), cancellationToken);
        return result.ToActionResult().ToMvcResult();
    }
    public static async Task<IResult> DeleteName(
        [FromRoute] Guid id,
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken
    )
    {
        Result result = await mediator.Send(new DeleteNameCommand(id), cancellationToken);
        return result.ToActionResult().ToMvcResult();
    }
    #endregion

    #region Interfaces
    public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder builder)
    {
        RouteGroupBuilder group = builder.MapGroup("/name");
        group.MapGet("/", GetAllName);
        group.MapGet("/{id:guid}", GetName);
        group.MapPost("/", CreateName);
        group.MapPut("/", UpdateName);
        group.MapDelete("/{id:guid}", DeleteName);
        return group;
    }
    #endregion
}
```

> [!NOTE]
> Name is the name of the feature, and it should be replaced with the actual name of the feature you are implementing.

> [!IMPORTANT]
> Validator must implement `IEndpointsProvider<T>` interface from our solution.

Endpoints class register all nesseccary endpoints once after the start of the server.
Each endpoint represented by a separated method.
Each method basically just sends the command or query to the handler and returns the result.

> [!TIP]
> Use `IMediator` to send commands and queries to the handlers.

> [!IMPORTANT]
> All endpoints must return `Task<IResult>` type from Microsoft.AspNetCore.Http package.

> [!IMPORTANT]
> Don`t forget to add CancellationToken parameter to the endpoint methods, and pass it to the mediator.Send method.

## 3. IntegrationTests
Example of the class:
```csharp
using Mediator;
using FluentResults;
using FluentAssertions;
using PhoenixKC.Infrastructure;
using PhoenixKC.WebAPI.Features.Name;
using PhoenixKC.WebAPI.Features.Name.Dtos;
using Microsoft.Extensions.DependencyInjection;
using PhoenixKC.Infrastructure.Features.Name;
using PhoenixKC.WebAPI.Features.Name.Handlers;

namespace PhoenixKC.IntegrationTests.Features.Name;

public sealed class GetAllNameTests(PhoenixWebApplicationFactory thisFactory) : IClassFixture<PhoenixWebApplicationFactory>
{
    [Fact]
    public async Task GetAllName_ShouldSucceded_WhenNameIsEmpty()
    {
        await thisFactory.ResetDatabase();

        //Arrange
        List<NameEntity> entities = [
            new NameEntity()
            {
                Title = "Name1"
            },
            new NameEntity()
            {
                Title = "Name2"
            }
        ];
        using(IServiceScope db_scope = thisFactory.Services.CreateScope())
        {
            PhoenixDbContext db_context = db_scope.ServiceProvider.GetRequiredService<PhoenixDbContext>();
            await db_context.Name.AddRangeAsync(entities, CancellationToken.None);
            await db_context.SaveChangesAsync(CancellationToken.None);
        }
        IEnumerable<NameDto> dtos = entities.ToDto();

        //Act
        using IServiceScope handler_scope = thisFactory.Services.CreateScope();
        IMediator mediator = handler_scope.ServiceProvider.GetRequiredService<IMediator>();
        Result<IEnumerable<NameDto>> result = await mediator.Send(new GetAllNameQuery(), CancellationToken.None);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(dtos);
    }
}
```

> [!NOTE]
> Name is the name of the feature, and it should be replaced with the actual name of the feature you are implementing.

IntegrationTests are responsible for testing the feature and ensuring that it works as expected.
Each feature should have its own IntegrationTests class, and each class should have its own test methods.

> [!TIP]
> Use extension methods to assert result of the test from FluentAssertions package.

> [!IMPORTANT]
> IntegrationTests class must implement `IClassFixture<PhoenixWebApplicationFactory>` interface from xUnit package.
> Also provide `PhoenixWebApplicationFactory thisFactory` parameter at the constuctor of the class