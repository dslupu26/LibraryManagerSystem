using Business;
using LibraryManagerSystem.Components;
using Bussiness;
using Bussiness.Generic;
using Bussiness.Profiles;
using Common;
using Common.Repositories;
using Repositories;
using Radzen;
using Bussiness.Common;
using Business.Profiles;
using Business.Business;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddSingleton<ICurrentUserProvider, CurrentUserProviderMock>(); 
builder.Services.AddSingleton<IUnitOfWorkFactory, UnitOfWorkFactory>();
builder.Services.AddSingleton<IRepositoryFactory, RepositoryFactory>();

builder.Services.AddSingleton<IDatabaseManager, DatabaseManager>();
builder.Services.AddSingleton<IGenericManagerFactory, GenericManagerFactory>();

builder.Services.AddAutoMapper(typeof(BookProfile));
builder.Services.AddAutoMapper(typeof(CustomerProfile));
builder.Services.AddAutoMapper(typeof(RentProfile));

builder.Services.AddSingleton<IBookManager, BookManager>();
builder.Services.AddSingleton<ICustomerManager, CustomerManager>();
builder.Services.AddSingleton<IRentManager, RentManager>();

builder.Services.AddSingleton<MessageService>();

builder.Services.AddRadzenComponents();
builder.Services.AddScoped<Radzen.DialogService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
