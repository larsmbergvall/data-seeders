# C# Dev Data Seeders

Very simple package for creating class based data seeders. Inspired by
[Laravel Model Factories](https://laravel.com/docs/eloquent-factories)

To create a seeder, make a new class: `PostSeeder.cs`

```csharp
using DevData.Seeders;

// Namespaces, usings, etc

public class PostSeeder : BaseSeeder
{
    // If using entity framework, you might want to dependency inject a DbContext:
    
    private readonly DataContext _context;

    public PostSeeder(DataContext context)
    {
        _context = context;
    }

    // This seeder will only run if this method returns true. In this case,
    // we only want to run it if we don't have any posts yet
    protected override bool ShouldRun()
    {
        return !_context.Posts.Any();
    }

    // This is the actual code that is executed when the seeder runs. I.e. this method should
    // create and persist any data that you want to create
    protected override void Run()
    {
        _context.Posts.AddRange(new PostFactory().Create(100));
        _context.SaveChanges();
    }
}
```

Technically, all you need is the `bool ShouldRun()` and `void Run()`, depending on what you want to do

### Seeder configurations

A seeder configuration is basically a set of seeders grouped together. For instance, you might have configuration
that seeds any data you might need to develop your application.

To set up a configuration, create a new class: `DevSeederConfig.cs`

```csharp
using DevData.Seeders;

// Namespaces, usings, etc

public class DevSeederConfig : DevSeederConfiguration
{
    public DevSeederConfig(PostSeeder postSeeder)
    {
        // Add any seeders that you want to use in this configuration
        AddSeeder(postSeeder);
    }
}
```

If running in a web app, make some changes to your `Program.cs`:

```csharp
// ...

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddScoped<PostSeeder>();
    builder.Services.AddScoped<DevSeederConfig>();
}

// ...
var app = builder.Build();
// ...

if (app.Environment.IsDevelopment() && args.Length == 1 && args[0] == "seed")
{
    using var scope = app.Services.CreateScope();
    
    // Here we only use the DevSeederConfig configuration, but you can run
    // as many different configurations as you need
    var devSeeder = scope.ServiceProvider.GetService<DevSeederConfig>();
    devSeeder!.Seed();

    Console.WriteLine("Seeding completed. Shutting down.");

    Environment.Exit(0);
}
// ...

```

The last block allows you to seed by running your app with `dotnet run seed`