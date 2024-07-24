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
                options.Authority = "http://unifyauth.local";

                options.ClientId = "b0329dd4-c897-48c0-9c8a-579a3514d5e7";
                options.ClientSecret = "uoy_S83y2TcHyQmNjalZ1JGfRkzwNR5wGByF";
                options.ResponseType = "code";
                options.RequireHttpsMetadata= false;
                options.Scope.Add("openid profile uniauth_sample_api:weather:read");

                options.SaveTokens = true;
                // Allowed Callback URLs
                //options.CallbackPath = new PathString("/");

                options.NonceCookie.SameSite = SameSiteMode.Unspecified;
                options.CorrelationCookie.SameSite = SameSiteMode.Unspecified;
            });


            var app = builder.Build();

            app.UseCookiePolicy(new CookiePolicyOptions
            {
                Secure = CookieSecurePolicy.Always
            });

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            else
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}