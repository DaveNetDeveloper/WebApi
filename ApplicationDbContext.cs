using API.Models;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) {
    }

    // Define las propiedades DbSet para cada tabla en base de datos
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Rol> Roles { get; set; }
    public DbSet<TipoEntidad> TipoEntidades { get; set; }
    public DbSet<Transaccion> Transacciones { get; set; }
    public DbSet<QR> QRs { get; set; }
    public DbSet<Entidad> Entidades { get; set; }
    public DbSet<Producto> Productos { get; set; }
    public DbSet<Actividad> Actividades { get; set; }
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<FAQ> FAQS { get; set; }
    public DbSet<Testimonio> Testimonios { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configuraciones adicionales para el modelo, como configuraciones de clave primaria, restricciones, etc.
    }
}