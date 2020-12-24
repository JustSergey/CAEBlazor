using CAEBlazor.Data;
using CAEBlazor.Shared.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CAEBlazor.Hubs
{
    public class EmployeesHub : Hub
    {
        private readonly CAEContext context;

        public EmployeesHub(CAEContext context) => this.context = context;

        public async override Task OnConnectedAsync()
        {
            List<Employee> employees = await context.Employees.Include(e => e.Company).ToListAsync();
            await Clients.Caller.SendAsync("UpdateEmployees", employees);
            await base.OnConnectedAsync();
        }

        public async Task DeleteEmployee(int id)
        {
            Employee employee = await context.Employees.FindAsync(id);
            if (employee == null)
                return;
            context.Employees.Remove(employee);
            await context.SaveChangesAsync();
            await Clients.All.SendAsync("DeleteEmployee", id);
        }
    }
}
