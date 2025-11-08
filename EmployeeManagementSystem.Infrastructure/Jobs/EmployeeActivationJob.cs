using EmployeeManagementSystem.Infrastructure.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Quartz;

namespace EmployeeManagementSystem.Infrastructure.Jobs;

[DisallowConcurrentExecution]
public class EmployeeActivationJob(ApplicationDbContext ctx,ILogger<EmployeeActivationJob> logger): IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            logger.LogInformation("Employee activation job started at {Time}", DateTime.UtcNow);

            var oneHourAgo = DateTime.UtcNow.AddHours(-1);
            var employeesToActivate = await ctx.Employees
                .Where(e => !e.IsActive && e.CreatedAt <= oneHourAgo)
                .ToListAsync();

            foreach (var employee in employeesToActivate)
            {
                employee.Activate();
                logger.LogInformation("Activating employee {EmployeeId} - {FirstName} {LastName}", 
                    employee.Id, employee.FirstName, employee.LastName);
            }

            if (employeesToActivate.Any())
            {
                await ctx.SaveChangesAsync();
                logger.LogInformation("Activated {Count} employees", employeesToActivate.Count);
            }
            else
                logger.LogInformation("No employees to activate");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while executing employee activation job");
            throw;
        }
    }
}

