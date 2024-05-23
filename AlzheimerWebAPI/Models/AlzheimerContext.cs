using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AlzheimerWebAPI.Models;

public partial class AlzheimerContext : DbContext
{
    public AlzheimerContext()
    {
    }

    public AlzheimerContext(DbContextOptions<AlzheimerContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cuidadores> Cuidadores { get; set; }

    public virtual DbSet<Dias> Dias { get; set; }

    public virtual DbSet<Dispositivos> Dispositivos { get; set; }

    public virtual DbSet<Familiares> Familiares { get; set; }

    public virtual DbSet<Geocercas> Geocercas { get; set; }

    public virtual DbSet<Medicamentos> Medicamentos { get; set; }

    public virtual DbSet<Notificaciones> Notificaciones { get; set; }

    public virtual DbSet<Pacientes> Pacientes { get; set; }

    public virtual DbSet<PacientesCuidadores> PacientesCuidadores { get; set; }

    public virtual DbSet<PacientesFamiliares> PacientesFamiliares { get; set; }

    public virtual DbSet<Personas> Personas { get; set; }

    public virtual DbSet<TiposNotificaciones> TiposNotificaciones { get; set; }

    public virtual DbSet<TiposUsuarios> TiposUsuarios { get; set; }

    public virtual DbSet<Ubicaciones> Ubicaciones { get; set; }

    public virtual DbSet<Usuarios> Usuarios { get; set; }

    /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=Alzheimer;User=sa;Password=1234;TrustServerCertificate=True;", x => x.UseNetTopologySuite())
        .UseLazyLoadingProxies();*/

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cuidadores>(entity =>
        {
            entity.HasKey(e => e.IdCuidador).HasName("PK__Cuidador__8E88B5898E2760C2");

            entity.HasIndex(e => e.IdUsuario, "UQ__Cuidador__5B65BF962223E6BA").IsUnique();

            entity.Property(e => e.IdCuidador).ValueGeneratedOnAdd();

            entity.HasOne(d => d.IdFamiliarNavigation).WithOne(p => p.Cuidador)
                .HasForeignKey<Cuidadores>(d => d.IdFamiliar)
                .HasConstraintName("FK__Cuidadore__IdFam__6EF57B66");

            entity.HasOne(d => d.IdUsuarioNavigation).WithOne(p => p.Cuidadore)
                .HasForeignKey<Cuidadores>(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Cuidadore__IdUsu__6E01572D");
        });

        modelBuilder.Entity<Dias>(entity =>
        {
            entity.HasKey(e => e.IdDia).HasName("PK__Dias__0E65D7186D970B7A");

            entity.Property(e => e.IdDia).ValueGeneratedOnAdd();
            entity.Property(e => e.Domingo).HasDefaultValue(false);
            entity.Property(e => e.Jueves).HasDefaultValue(false);
            entity.Property(e => e.Lunes).HasDefaultValue(false);
            entity.Property(e => e.Martes).HasDefaultValue(false);
            entity.Property(e => e.Miercoles).HasDefaultValue(false);
            entity.Property(e => e.Sabado).HasDefaultValue(false);
            entity.Property(e => e.Viernes).HasDefaultValue(false);

            entity.HasOne(d => d.IdCuidaPacienteNavigation).WithMany(p => p.Dia)
                .HasForeignKey(d => d.IdCuidaPaciente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Dias__IdCuidaPac__76969D2E");
        });

        modelBuilder.Entity<Dispositivos>(entity =>
        {
            entity.HasKey(e => e.IdDispositivo).HasName("PK__Disposit__B1EDB8E4E868E3A3");

            entity.Property(e => e.IdDispositivo)
                .HasMaxLength(17)
                .IsUnicode(false);

            entity.HasOne(d => d.IdGeocercaNavigation).WithOne(p => p.Dispositivo)
                .HasForeignKey<Dispositivos>(d => d.IdGeocerca)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Dispositi__IdGeo__6FE99F9F");
        });

        modelBuilder.Entity<Familiares>(entity =>
        {
            entity.HasKey(e => e.IdFamiliar).HasName("PK__Familiar__C821EAAAAE62E5B3");

            entity.HasIndex(e => e.IdUsuario, "UQ__Familiar__5B65BF964AB67D29").IsUnique();

            entity.Property(e => e.IdFamiliar).ValueGeneratedOnAdd();

            entity.HasOne(d => d.IdUsuarioNavigation).WithOne(p => p.Familiare)
                .HasForeignKey<Familiares>(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Familiare__IdUsu__6D0D32F4");
        });

        modelBuilder.Entity<Geocercas>(entity =>
        {
            entity.HasKey(e => e.IdGeocerca).HasName("PK__Geocerca__AE2E8719BC3B8543");
            entity.Property(e => e.CoordenadaInicial).HasColumnType("geography");
            entity.Property(e => e.IdGeocerca).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<Medicamentos>(entity =>
        {
            entity.HasKey(e => e.IdMedicamento).HasName("PK__Medicame__AC96376EB4904B95");

            entity.Property(e => e.IdMedicamento).ValueGeneratedOnAdd();
            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.IdPaciente)
                .HasMaxLength(18)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.IdPacienteNavigation).WithMany(p => p.Medicamentos)
                .HasForeignKey(d => d.IdPaciente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Medicamen__IdPac__75A278F5");
        });

        modelBuilder.Entity<Notificaciones>(entity =>
        {
            entity.HasKey(e => e.IdNotificacion).HasName("PK__Notifica__F6CA0A857A579124");

            entity.Property(e => e.IdNotificacion).ValueGeneratedOnAdd();
            entity.Property(e => e.IdPaciente)
                .HasMaxLength(18)
                .IsUnicode(false);
            entity.Property(e => e.Mensaje)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.IdPacienteNavigation).WithMany(p => p.Notificaciones)
                .HasForeignKey(d => d.IdPaciente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Notificac__IdPac__73BA3083");

            entity.HasOne(d => d.IdTipoNotificacionNavigation).WithMany(p => p.Notificaciones)
                .HasForeignKey(d => d.IdTipoNotificacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Notificac__IdTip__74AE54BC");
        });

        modelBuilder.Entity<Pacientes>(entity =>
        {
            entity.HasKey(e => e.IdPaciente).HasName("PK__Paciente__C93DB49B5FB2DF7A");

            entity.HasIndex(e => e.IdPersona, "UQ__Paciente__2EC8D2AD45272801").IsUnique();

            entity.HasIndex(e => e.IdDispositivo, "UQ__Paciente__B1EDB8E5FF269BF6").IsUnique();

            entity.Property(e => e.IdPaciente)
                .HasMaxLength(18)
                .IsUnicode(false);
            entity.Property(e => e.IdDispositivo)
                .HasMaxLength(17)
                .IsUnicode(false);

            entity.HasOne(d => d.IdDispositivoNavigation).WithOne(p => p.Paciente)
                .HasForeignKey<Pacientes>(d => d.IdDispositivo)
                .HasConstraintName("FK__Pacientes__IdDis__71D1E811");

            entity.HasOne(d => d.IdPersonaNavigation).WithOne(p => p.Paciente)
                .HasForeignKey<Pacientes>(d => d.IdPersona)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Pacientes__IdPer__72C60C4A");
        });

        modelBuilder.Entity<PacientesCuidadores>(entity =>
        {
            entity.HasKey(e => e.IdCuidaPaciente).HasName("PK__Paciente__35EBC067E4A4F27A");

            entity.Property(e => e.IdCuidaPaciente).ValueGeneratedOnAdd();
            entity.Property(e => e.IdPaciente)
                .HasMaxLength(18)
                .IsUnicode(false);

            entity.HasOne(d => d.IdCuidadorNavigation).WithMany(p => p.PacientesCuidadores)
                .HasForeignKey(d => d.IdCuidador)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Pacientes__IdCui__778AC167");

            entity.HasOne(d => d.IdPacienteNavigation).WithMany(p => p.PacientesCuidadores)
                .HasForeignKey(d => d.IdPaciente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Pacientes__IdPac__787EE5A0");
        });

        modelBuilder.Entity<PacientesFamiliares>(entity =>
        {
            entity.HasKey(e => e.IdPacienteFamiliar).HasName("PK__Paciente__1F46122D666533C5");

            entity.Property(e => e.IdPacienteFamiliar).ValueGeneratedOnAdd();
            entity.Property(e => e.IdPaciente)
                .HasMaxLength(18)
                .IsUnicode(false);

            entity.HasOne(d => d.IdFamiliarNavigation).WithMany(p => p.PacientesFamiliares)
                .HasForeignKey(d => d.IdFamiliar)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Pacientes__IdFam__797309D9");

            entity.HasOne(d => d.IdPacienteNavigation).WithMany(p => p.PacientesFamiliares)
                .HasForeignKey(d => d.IdPaciente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Pacientes__IdPac__7A672E12");
        });

        modelBuilder.Entity<Personas>(entity =>
        {
            entity.HasKey(e => e.IdPersona).HasName("PK__Personas__2EC8D2ACA16761E0");

            entity.Property(e => e.IdPersona).ValueGeneratedOnAdd();
            entity.Property(e => e.ApellidoM).HasMaxLength(255);
            entity.Property(e => e.ApellidoP).HasMaxLength(255);
            entity.Property(e => e.Nombre).HasMaxLength(255);
            entity.Property(e => e.NumeroTelefono).HasMaxLength(255);
        });

        modelBuilder.Entity<TiposNotificaciones>(entity =>
        {
            entity.HasKey(e => e.IdTipoNotificacion).HasName("PK__TiposNot__0ECE0435F467912D");

            entity.Property(e => e.IdTipoNotificacion).ValueGeneratedNever();
            entity.Property(e => e.TipoNotificacion)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TiposUsuarios>(entity =>
        {
            entity.HasKey(e => e.IdTipoUsuario).HasName("PK__TiposUsu__CA04062BF58993C3");

            entity.Property(e => e.IdTipoUsuario).ValueGeneratedOnAdd();
            entity.Property(e => e.TipoUsuario)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Ubicaciones>(entity =>
        {
            entity.HasKey(e => e.IdUbicacion).HasName("PK__Ubicacio__778CAB1DF451C449");

            entity.Property(e => e.IdUbicacion).ValueGeneratedOnAdd();
            entity.Property(e => e.Ubicacion).HasColumnType("geography");
            entity.Property(e => e.FechaHora).HasColumnType("smalldatetime");
            entity.Property(e => e.IdDispositivo)
                .HasMaxLength(17)
                .IsUnicode(false);

            entity.HasOne(d => d.IdDispositivoNavigation).WithMany(p => p.Ubicaciones)
                .HasForeignKey(d => d.IdDispositivo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Ubicacion__IdDis__70DDC3D8");
        });

        modelBuilder.Entity<Usuarios>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__Usuarios__5B65BF97FA43EADF");

            entity.HasIndex(e => e.IdPersona, "UQ__Usuarios__2EC8D2AD74EE6819").IsUnique();

            entity.Property(e => e.IdUsuario).ValueGeneratedOnAdd();
            entity.Property(e => e.Contrasenia).HasMaxLength(255);
            entity.Property(e => e.Correo).HasMaxLength(255);

            entity.HasOne(d => d.IdPersonaNavigation).WithOne(p => p.Usuario)
                .HasForeignKey<Usuarios>(d => d.IdPersona)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Usuarios__IdPers__6C190EBB");

            entity.HasOne(d => d.IdTipoUsuarioNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdTipoUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Usuarios__IdTipo__6B24EA82");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
