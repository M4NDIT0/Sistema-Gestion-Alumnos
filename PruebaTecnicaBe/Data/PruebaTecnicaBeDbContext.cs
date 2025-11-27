using Microsoft.EntityFrameworkCore;
using PruebaTecnicaBe.Models;
using System.Collections.Generic;

namespace PruebaTecnicaBe.Data
{
    public class PruebaTecnicaBeDbContext : DbContext
    {
        public PruebaTecnicaBeDbContext(DbContextOptions<PruebaTecnicaBeDbContext> options) : base(options)
        {
        }

       public DbSet<Alumno> Alumnos { get; set; }
    }
}

