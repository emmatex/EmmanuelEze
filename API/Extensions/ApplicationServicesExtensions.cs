using API.Errors;
using Domain.Factory;
using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.PaymentProvider;
using Infrastructure.PaymentProvider.CheapPayment;
using Infrastructure.PaymentProvider.Expensive;
using Infrastructure.PaymentProvider.Premium;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace API.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IPaymentGatewayProvider, ExpensivePaymentProcessor>();
            services.AddTransient<IPaymentGatewayProvider, PremiumPaymentProcessor>();
            services.AddTransient<IPaymentProcessorManger, PaymentProcessorManager>();
            services.AddTransient<IPaymentGatewayProvider, CheapPaymentProcessor>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<ICheapPaymentGateway, CheapPaymentProcessor>();
            services.AddScoped(typeof(IGenericRepository<>), (typeof(GenericRepository<>)));
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .SelectMany(x => x.Value.Errors)
                        .Select(x => x.ErrorMessage).ToArray();
                    var errorResponse = new ApiValidationErrorResponse
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(errorResponse);
                };
            });
            return services;
        }
    }
}
