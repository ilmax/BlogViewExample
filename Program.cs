using BlogViewExample;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<BlogContext>(opt =>
{
    opt.UseSqlite("Datasource=SQLite.db");
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

var context = app.Services.CreateScope().ServiceProvider.GetRequiredService<BlogContext>();
await context.SeedAsync();

app.UseRouting();
app.UseSwagger();
app.UseSwaggerUI(opt =>
{
    opt.SwaggerEndpoint("/swagger/v1/swagger.json", "My Awesome API");
    opt.RoutePrefix = "";
});

app.MapGet("/efquery", async (BlogContext db, string name) =>
{
    var query = db.Blogs.AsQueryable();
    if (!string.IsNullOrEmpty(name))
    {
        query = query.Where(b => b.Title.Contains(name));
    }

    var blogs = (IBlogView[])await query
        .Select(b => new Blog { Id = b.Id, Title = b.Title })
        .ToArrayAsync();
    return Results.Ok(blogs);
})
    .WithName("efquery")
    .WithOpenApi();

app.MapGet("/dto", async (BlogContext db, string name) =>
{
    var query = db.Blogs.AsQueryable();
    if (!string.IsNullOrEmpty(name))
    {
        query = query.Where(b => b.Title.Contains(name));
    }

    var blogs = await query
        .Select(b => new BlogDto { Id = b.Id, Title = b.Title })
        .ToArrayAsync();
    return Results.Ok(blogs);
})
    .WithName("dto")
    .WithOpenApi();

app.Run();
