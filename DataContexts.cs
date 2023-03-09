using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
namespace Employee
{
    public class DataContexts : DbContext
    {
        public DataContexts(DbContextOptions<DataContexts> options) :base(options) {
            
        }
        public DbSet<Employees> Employees => Set<Employees>();
    }
}
