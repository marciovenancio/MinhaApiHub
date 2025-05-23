using Microsoft.EntityFrameworkCore;

namespace MinhaApiHub.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // Aqui serão adicionados os DbSets para suas entidades
    // Exemplo: public DbSet<MinhaEntidade> MinhaEntidade { get; set; }

    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Cotacao> Cotacoes { get; set; }
    public DbSet<Comissao> Comissoes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("qalabs");
        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.ToTable("clientes");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.NomeRazaoSocial).HasColumnName("nome_razao_social").IsRequired();
            entity.Property(e => e.CpfCnpj).HasColumnName("cpf_cnpj").IsRequired();
            entity.Property(e => e.Email).HasColumnName("email").IsRequired();
            entity.Property(e => e.Telefone).HasColumnName("telefone").IsRequired();
        });
        modelBuilder.Entity<Cotacao>(entity =>
        {
            entity.ToTable("cotacoes");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ClienteId).HasColumnName("cliente_id").IsRequired();
            entity.Property(e => e.TipoSeguro).HasColumnName("tipo_seguro").IsRequired();
            entity.Property(e => e.DataPedido).HasColumnName("data_pedido").IsRequired();
            entity.Property(e => e.Status).HasColumnName("status").IsRequired();
            entity.HasOne(e => e.Cliente)
                  .WithMany()
                  .HasForeignKey(e => e.ClienteId);
        });
        modelBuilder.Entity<Comissao>(entity =>
        {
            entity.ToTable("comissoes");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ClienteId).HasColumnName("cliente_id").IsRequired();
            entity.Property(e => e.NumeroApolice).HasColumnName("numero_apolice").IsRequired();
            entity.Property(e => e.TipoSeguro).HasColumnName("tipo_seguro").IsRequired();
            entity.Property(e => e.Data).HasColumnName("data").IsRequired();
            entity.Property(e => e.Premio).HasColumnName("premio").IsRequired();
            entity.Property(e => e.ComissaoValor).HasColumnName("comissao").IsRequired();
            entity.Property(e => e.Status).HasColumnName("status").IsRequired();
            entity.HasOne(e => e.Cliente)
                  .WithMany()
                  .HasForeignKey(e => e.ClienteId);
        });
    }
} 