using Employee;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<DataContexts>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

async Task<List<Employees>> GetAllTodo([FromServices]DataContexts context) => await context.Employees.ToListAsync();
app.MapGet("/Employees", async ([FromServices]DataContexts context) => await context.Employees.ToListAsync());
app.MapGet("/Employees/{id}", async ([FromServices]DataContexts context, int id) => await context.Employees.FindAsync(id) is Employees Item ? Results.Ok(Item) : Results.NotFound("Item not found"));
app.MapPost("Add/Employees", async ([FromServices]DataContexts context, [FromBody]Employees Item) =>
{
    context.Employees.Add(Item);
    await context.SaveChangesAsync();
    return Results.Ok(await GetAllTodo(context));

});

app.MapPut("/Employees/{id}", async ([FromServices]DataContexts context, [FromBody]Employees Item,int id) =>
{
    var employeeItem = await context.Employees.FindAsync(id);
    if (employeeItem == null) return Results.NotFound("item not found");
    employeeItem.name = Item.name;
    employeeItem.salary = Item.salary;
    employeeItem.address = Item.address;
    employeeItem.phone = Item.phone;
    await context.SaveChangesAsync();
    return Results.Ok(await GetAllTodo(context));
});

app.MapDelete("/Employees/{id}", async ([FromServices]DataContexts context, int id) =>
{
    var employeeItem = await context.Employees.FindAsync(id);
    if (employeeItem == null) return Results.NotFound("item not found");
    context.Remove(employeeItem);
    await context.SaveChangesAsync();
    return Results.Ok(await GetAllTodo(context));
});


app.Run();
