
var builder = WebApplication.CreateBuilder(args);

//add a service to the container
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddCors();
//Add Identity Auth sevive through Extention Methood FD
builder.Services.AddIdentityService(builder.Configuration);

//Configure the HTTP request pipeLine
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
     app.UseDeveloperExceptionPage();
    // app.UseSwagger();
    //app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPIv6 v1"));
}
app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseRouting();
//Cross site 
app.UseCors(x => x.AllowAnyHeader()
    .AllowAnyMethod()
    .WithOrigins("http://localhost:4200",
    "https://localhost:4200",
    "https://web.postman.co",
    "https://web.postman.co/"));
app.UseAuthentication();
app.UseAuthorization();
//to use default and static file user basically agular 
app.UseDefaultFiles();
app.UseStaticFiles();
app.MapControllers();
//to use this tell api where to go after refresh of no path found
app.MapFallbackToController("Index", "Fallback");
// app.MapHub<PresecceHub>("hubs/presence");
//app.MapHub<MesasgeHub>("hubs/message");

// AppContext.SetSwitch("Ngpsql.EnabledLegasyTimestampBehavior",true);

//var host = CreateHostbuilder(args).Build();
using var scope = app.Services.CreateScope();
var Services = scope.ServiceProvider;
try
{
    var context = Services.GetRequiredService<DataContext>();
    var userManager = Services.GetRequiredService<UserManager<AppUser>>();
    var roleManager = Services.GetRequiredService<RoleManager<AppRole>>();
    await context.Database.MigrateAsync();
    await Seed.SeedUsers(userManager, roleManager);
}
catch (Exception ex)
{
    var logger = Services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occourd during Migrations");

}
await app.RunAsync();
