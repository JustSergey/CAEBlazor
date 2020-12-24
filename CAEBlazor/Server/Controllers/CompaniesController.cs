using CAEBlazor.Data;
using CAEBlazor.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CAEBlazor.Server.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly CAEContext context;

        public CompaniesController(CAEContext context) => this.context = context;

        [HttpGet]
        public async Task<List<Company>> GetAsync()
        {
            return await context.Companies.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<Company> GetAsync(int id)
        {
            return await context.Companies.FindAsync(id);
        }

        [HttpPost]
        public async Task PostAsync([FromBody] Company company)
        {
            await context.AddAsync(company);
            await context.SaveChangesAsync();
        }

        [HttpPut("{id}")]
        public async Task PutAsync(int id, [FromBody] Company company)
        {
            if (company.Id == id && await context.Companies.AnyAsync(c => c.Id == id))
            {
                context.Update(company);
                await context.SaveChangesAsync();
            }
        }

        [HttpDelete("{id}")]
        public async Task DeleteAsync(int id)
        {
            Company company = await context.Companies.FindAsync(id);
            if (company == null)
                return;
            context.Companies.Remove(company);
            await context.SaveChangesAsync();
        }
    }
}
