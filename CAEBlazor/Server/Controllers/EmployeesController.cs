using CAEBlazor.Data;
using CAEBlazor.Hubs;
using CAEBlazor.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CAEBlazor.Server.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly CAEContext context;
        private readonly IHubContext<EmployeesHub> hubContext;

        public EmployeesController(CAEContext context, IHubContext<EmployeesHub> hubContext) => 
            (this.context, this.hubContext) = (context, hubContext);

        [HttpGet]
        public async Task<List<Employee>> GetAsync()
        {
            return await context.Employees.Include(e => e.Company).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<Employee> GetAsync(int id)
        {
            return await context.Employees.FindAsync(id);
        }

        [HttpPost]
        public async Task PostAsync([FromBody] Employee employee)
        {
            employee.Company = await context.Companies.FindAsync(employee.CompanyId);
            if (employee.Company == null)
                return;
            await context.AddAsync(employee);
            await context.SaveChangesAsync();
            await hubContext.Clients.All.SendAsync("AddEmployee", employee);
        }

        [HttpPut("{id}")]
        public async Task PutAsync(int id, [FromBody] Employee employee)
        {
            if (employee.Id == id && await context.Employees.AnyAsync(e => e.Id == id))
            {
                employee.Company = await context.Companies.FindAsync(employee.CompanyId);
                if (employee.Company == null)
                    return;
                context.Update(employee);
                await context.SaveChangesAsync();
                await hubContext.Clients.All.SendAsync("UpdateEmployee", employee);
            }
        }
    }
}
