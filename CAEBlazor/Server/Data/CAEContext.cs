using CAEBlazor.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CAEBlazor.Data
{
    public class CAEContext : DbContext
    {
        public CAEContext(DbContextOptions<CAEContext> options) : base(options) { }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Company> Companies { get; set; }
    }
}
