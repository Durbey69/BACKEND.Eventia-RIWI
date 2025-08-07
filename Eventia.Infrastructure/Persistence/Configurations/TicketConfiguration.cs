using Eventia.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventia.Infrastructure.Persistence.Configurations
{
    public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder.ToTable("Tickets");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Titulo).IsRequired().HasMaxLength(200);
            builder.Property(t => t.Descripcion).IsRequired().HasMaxLength(1000);
            builder.HasOne(t => t.UsuarioAsignado)
                   .WithMany(u => u.TicketsAsignados)
                   .HasForeignKey(t => t.UsuarioAsignadoId)
                   .OnDelete(DeleteBehavior.Restrict);
        }


    }
}
