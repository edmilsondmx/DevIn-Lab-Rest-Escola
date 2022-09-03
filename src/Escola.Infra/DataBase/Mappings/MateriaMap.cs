using Escola.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Escola.Infra.DataBase.Mappings;

public class MateriaMap
{
    public class BoletimMap : IEntityTypeConfiguration<Materia>
    {
        public void Configure(EntityTypeBuilder<Materia> builder)
        {
            builder.ToTable("Materia");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.Id)
                    .HasColumnName("ID")
                    .UseIdentityColumn()
                    .ValueGeneratedOnAdd();

            builder.Property(m => m.Nome)
                    .HasColumnName("MATERIA")
                    .HasColumnType("VARCHAR")
                    .HasMaxLength(50);
        }
    }
}
