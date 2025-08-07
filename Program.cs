using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://0.0.0.0:80");

// Register EF Core with in-memory database
builder.Services.AddDbContext<TodoDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment() || app.Environment.IsProduction()) // optional condition
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.Lifetime.ApplicationStarted.Register(() =>
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<TodoDbContext>();
    db.Database.EnsureCreated(); // <--- This line creates the DB and tables if they don't exist
});

// Define API endpoints
app.MapGet("/api/todos", async (TodoDbContext db) =>
    await db.TodoItems.ToListAsync());

app.MapGet("/api/todos/{id}", async (int id, TodoDbContext db) =>
    await db.TodoItems.FindAsync(id) is TodoItem todo
        ? Results.Ok(todo)
        : Results.NotFound());

app.MapPost("/api/todos", async ([FromBody] TodoItem todo, TodoDbContext db) =>
{
    db.TodoItems.Add(todo);
    await db.SaveChangesAsync();
    return Results.Created($"/api/todos/{todo.Id}", todo);
});

app.MapPut("/api/todos/{id}", async (int id, TodoItem updated, TodoDbContext db) =>
{
    var todo = await db.TodoItems.FindAsync(id);
    if (todo is null) return Results.NotFound();

    todo.Title = updated.Title;
    todo.IsCompleted = updated.IsCompleted;
    await db.SaveChangesAsync();

    return Results.Ok(todo);
});

app.MapDelete("/api/todos/{id}", async (int id, TodoDbContext db) =>
{
    var todo = await db.TodoItems.FindAsync(id);
    if (todo is null) return Results.NotFound();

    db.TodoItems.Remove(todo);
    await db.SaveChangesAsync();

    return Results.NoContent();
});
app.Use(async (context, next) =>
{
    context.Request.EnableBuffering(); // optional, in case you're reading body more than once
    await next();
    if (context.Response.StatusCode == 400)
    {
        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
        logger.LogWarning("400 Bad Request received at {Path}", context.Request.Path);
    }
});

app.Run();