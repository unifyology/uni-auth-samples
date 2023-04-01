using System.IdentityModel.Tokens.Jwt;

namespace UnifyAuth.Sample.WebMVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
            .AddCookie("Cookies")
            .AddOpenIdConnect("oidc", options =>
            {
                options.Authority = "https://localhost:5001";

                options.ClientId = "658c6886-d800-4c07-a384-c5d4c1bf522d";
                options.ClientSecret = "VexkBCJdSnglPez60oAFY1uHIv5gCzulwxIIrJ9o63g=";
                options.ResponseType = "code";

                options.Scope.Add("openid profile uniauth_sample_api:weather:read");

                options.SaveTokens = true;
                // Allowed Callback URLs
                options.CallbackPath= new PathString("/");
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}