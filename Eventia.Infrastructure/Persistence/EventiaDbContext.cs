using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Eventia.Domain.Entities;

namespace Eventia.Infrastructure.Persistence
{
    public class EventiaDbContext : DbContext
    {
        public EventiaDbContext(DbContextOptions<EventiaDbContext> options)
            : base(options) { }

        public DbSet<Ticket> Tickets => Set<Ticket>();
        public DbSet<Usuario> Usuarios => Set<Usuario>();

        public DbSet<TicketEvento> TicketEventos { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.UsuarioAsignado)
                .WithMany()  // Sin navegación inversa
                .HasForeignKey(t => t.UsuarioAsignadoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.UsuarioCreador)
                .WithMany()
                .HasForeignKey(t => t.UsuarioCreadorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TicketEvento>()
                .HasOne(e => e.Ticket)
                .WithMany()
                .HasForeignKey(e => e.TicketId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<TicketEvento>()
                .HasOne(e => e.UsuarioResponsable)
                .WithMany()
                .HasForeignKey(e => e.UsuarioResponsableId)
                .OnDelete(DeleteBehavior.Restrict);
        }


    }
}
