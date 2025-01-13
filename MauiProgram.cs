using Blazored.LocalStorage;
using BytesTracker.Model;
using BytesTracker.Dto;

using BytesTracker.Helper;
using Microsoft.Extensions.Logging;
using BytesTracker.Services;
using SQLite;
namespace BytesTracker
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });


            builder.Services.AddMauiBlazorWebView();

            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "bytesTracker.db");

            builder.Services.AddSingleton<SQLiteAsyncConnection>(_ =>
            {
                var connection = new SQLiteAsyncConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache);
                connection.CreateTableAsync<Users>().GetAwaiter().GetResult();
                connection.CreateTableAsync<Model.Tags>().GetAwaiter().GetResult();
            //    connection.DropTableAsync<Model.Transaction>();
                connection.CreateTableAsync<Model.Transaction>().GetAwaiter().GetResult();



                return connection;
            });


            builder.Services.AddSingleton<AuthenticationState>();
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddBlazoredLocalStorage();

            builder.Services.AddSingleton<UserService>();
            builder.Services.AddSingleton<TagService>();
            builder.Services.AddSingleton<TransactionService>();

            

            builder.Services.AddScoped<Dto.User>();
            builder.Services.AddScoped<Dto.Transaction>();
            builder.Services.AddScoped<Dto.Tags>();
            builder.Services.AddScoped<Dto.SortFormDTO>();










#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
