namespace Talabat.APIs.Extensions
{
    public static class SwaggerExtensions
    {
        public static void AddSwaggerExtensions(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }

        public static void UseSwaggerExtensions(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
    }
}
