using EmployeeManagementSystem.Mvc.Startup;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("Startup\\appsettings.json", optional: false)
        .AddEnvironmentVariables();
    
    builder.Services.AggregateServices(builder.Configuration);
}

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Employee Management System API v1");
    });
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseCors(cors =>
{
    cors
        .AllowAnyHeader()
        .AllowAnyMethod()
        .SetIsOriginAllowed(_ => true)
        .AllowCredentials();
});

app.UseAuthentication();
app.UseSession();
app.UseAuthorization();

app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.Run();
