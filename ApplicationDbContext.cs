using API.Models;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // Define las propiedades DbSet para cada tabla en tu base de datos
    public DbSet<Usuario> Usuarios { get; set; }
    // Otros DbSet según tus tablas

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Puedes agregar configuraciones adicionales para el modelo aquí
        // Por ejemplo, configuraciones de clave primaria, restricciones, etc.
    }
}