using Agenda.Client.Services;
using Agenda.Server.Controllers;
using Agenda.Server.Data;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.EntityFrameworkCore;
using Radzen;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<AgendaContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConexaoAgenda"));
});
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();
builder.Services.AddSingleton(sp =>
{
    // Get the address that the app is currently running at
    var server = sp.GetRequiredService<IServer>();
    var addressFeature = server.Features.Get<IServerAddressesFeature>();
    string baseAddress = addressFeature.Addresses.First();
    return new HttpClient { BaseAddress = new Uri(baseAddress) };
});
builder.Services.AddControllers().AddControllersAsServices();
builder.Services.AddScoped<ContatoService>();
builder.Services.AddScoped<CompromissoService>();
builder.Services.AddScoped<ContatoController>();
builder.Services.AddScoped<CompromissoController>();
builder.Services.AddScoped<ExportarController>();
builder.Services.AddScoped<ExportarAgendaController>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AgendaContext>();

    // Cria o banco de dados se ele não existir
    dbContext.Database.EnsureCreated();

    // Aplica as migrações, se houver
    dbContext.Database.Migrate();
}

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToPage("/_Host");

app.Run();