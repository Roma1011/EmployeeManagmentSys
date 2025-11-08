using EmployeeManagementSystem.Infrastructure.DAL;
using EmployeeManagementSystem.Infrastructure.Jobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace EmployeeManagementSystem.Infrastructure;

public static class InfrastructureAssembly
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddQuartz(q =>
        {
            var jobKey = new JobKey("EmployeeActivationJob");
            q.AddJob<EmployeeActivationJob>(opts => opts.WithIdentity(jobKey));
            q.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity("EmployeeActivationJob-trigger")
                .WithCronSchedule("0 * * * * ?")); 
        });

        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
        
        return services;
    }
}
