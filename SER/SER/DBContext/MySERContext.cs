using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using SER.Entidades;

namespace SER.DBContext
{
    public partial class MySERContext : DbContext
    {
        public MySERContext()
        {
        }

        public MySERContext(DbContextOptions<MySERContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Academium> Academia { get; set; } = null!;
        public virtual DbSet<Alumno> Alumnos { get; set; } = null!;
        public virtual DbSet<AlumnoTrabajoRecepcionalProyectoGuiadoView> AlumnoTrabajoRecepcionalProyectoGuiadoViews { get; set; } = null!;
        public virtual DbSet<Archivo> Archivos { get; set; } = null!;
        public virtual DbSet<CuerpoAcademico> CuerpoAcademicos { get; set; } = null!;
        public virtual DbSet<Direccion> Direccions { get; set; } = null!;
        public virtual DbSet<DireccionSinodalDelTrabajo> DireccionSinodalDelTrabajos { get; set; } = null!;
        public virtual DbSet<Documento> Documentos { get; set; } = null!;
        public virtual DbSet<ExamenDefensa> ExamenDefensas { get; set; } = null!;
        public virtual DbSet<Expediente> Expedientes { get; set; } = null!;
        public virtual DbSet<ExperienciaEducativa> ExperienciaEducativas { get; set; } = null!;
        public virtual DbSet<Integrante> Integrantes { get; set; } = null!;
        public virtual DbSet<Lgac> Lgacs { get; set; } = null!;
        public virtual DbSet<Organizacion> Organizacions { get; set; } = null!;
        public virtual DbSet<Pladeafei> Pladeafeis { get; set; } = null!;
        public virtual DbSet<PlanDeCurso> PlanDeCursos { get; set; } = null!;
        public virtual DbSet<PlanDeTrabajo> PlanDeTrabajos { get; set; } = null!;
        public virtual DbSet<Profesor> Profesors { get; set; } = null!;
        public virtual DbSet<ProyectoDeInvestigacion> ProyectoDeInvestigacions { get; set; } = null!;
        public virtual DbSet<SinodalDelTrabajo> SinodalDelTrabajos { get; set; } = null!;
        public virtual DbSet<TipoDocumento> TipoDocumentos { get; set; } = null!;
        public virtual DbSet<TrabajoRecepcional> TrabajoRecepcionals { get; set; } = null!;
        public virtual DbSet<TrabajoRecepcionalSinodalDelTrabajo> TrabajoRecepcionalSinodalDelTrabajos { get; set; } = null!;
        public virtual DbSet<Usuario> Usuarios { get; set; } = null!;
        public virtual DbSet<Vinculacion> Vinculacions { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=localhost; Database=SER; User ID=sa;Password=qwerty*1234;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Academium>(entity =>
            {
                entity.HasKey(e => e.AcademiaId);

                entity.Property(e => e.AcademiaId).HasColumnName("AcademiaID");

                entity.Property(e => e.Descripcion).IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Alumno>(entity =>
            {
                entity.HasKey(e => e.Matricula);

                entity.ToTable("Alumno");

                entity.Property(e => e.Matricula)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.CorreoElectronico)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasMany(d => d.ExperienciaEducativas)
                    .WithMany(p => p.Alumnos)
                    .UsingEntity<Dictionary<string, object>>(
                        "AlumnoExperienciaEducativa",
                        l => l.HasOne<ExperienciaEducativa>().WithMany().HasForeignKey("ExperienciaEducativaId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_AlumnoExperienciaEducativa_ExperienciaEducativa"),
                        r => r.HasOne<Alumno>().WithMany().HasForeignKey("AlumnoId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_AlumnoExperienciaEducativa_Alumno"),
                        j =>
                        {
                            j.HasKey("AlumnoId", "ExperienciaEducativaId");

                            j.ToTable("AlumnoExperienciaEducativa");

                            j.IndexerProperty<string>("AlumnoId").HasMaxLength(15).IsUnicode(false).HasColumnName("AlumnoID");

                            j.IndexerProperty<int>("ExperienciaEducativaId").HasColumnName("ExperienciaEducativaID");
                        });

                entity.HasMany(d => d.TrabajoRecepcionals)
                    .WithMany(p => p.Alumnos)
                    .UsingEntity<Dictionary<string, object>>(
                        "AlumnoTrabajoRecepcional",
                        l => l.HasOne<TrabajoRecepcional>().WithMany().HasForeignKey("TrabajoRecepcionalId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_AlumnoTrabajoRecepcional_TrabajoRecepcional"),
                        r => r.HasOne<Alumno>().WithMany().HasForeignKey("AlumnoId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_AlumnoTrabajoRecepcional_Alumno"),
                        j =>
                        {
                            j.HasKey("AlumnoId", "TrabajoRecepcionalId");

                            j.ToTable("AlumnoTrabajoRecepcional");

                            j.IndexerProperty<string>("AlumnoId").HasMaxLength(15).IsUnicode(false).HasColumnName("AlumnoID");

                            j.IndexerProperty<int>("TrabajoRecepcionalId").HasColumnName("TrabajoRecepcionalID");
                        });
            });

            modelBuilder.Entity<AlumnoTrabajoRecepcionalProyectoGuiadoView>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("AlumnoTrabajoRecepcionalProyectoGuiadoView");

                entity.Property(e => e.CorreoElectronico)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Estado)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ExperienciaEducativa)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ExperienciaEducativaId).HasColumnName("ExperienciaEducativaID");

                entity.Property(e => e.Fechadeinicio).HasColumnType("date");

                entity.Property(e => e.Matricula)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Modalidad)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.TrabajoRecepcionalId).HasColumnName("TrabajoRecepcionalID");
            });

            modelBuilder.Entity<Archivo>(entity =>
            {
                entity.HasKey(e => e.IdArchivo)
                    .HasName("NewTable_PK");

                entity.ToTable("Archivo");

                entity.Property(e => e.Direccion)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.NombreArchivo)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CuerpoAcademico>(entity =>
            {
                entity.ToTable("CuerpoAcademico");

                entity.Property(e => e.CuerpoAcademicoId).HasColumnName("CuerpoAcademicoID");

                entity.Property(e => e.AcademiaId).HasColumnName("AcademiaID");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Objetivogeneral).IsUnicode(false);
            });

            modelBuilder.Entity<Direccion>(entity =>
            {
                entity.ToTable("Direccion");

                entity.Property(e => e.DireccionId).HasColumnName("DireccionID");

                entity.Property(e => e.ExperienciaEducativaId).HasColumnName("ExperienciaEducativaID");

                entity.Property(e => e.FechaInicio).HasColumnType("date");

                entity.HasOne(d => d.ExperienciaEducativa)
                    .WithMany(p => p.Direccions)
                    .HasForeignKey(d => d.ExperienciaEducativaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Direccion_ExperienciaEducativa");
            });

            modelBuilder.Entity<DireccionSinodalDelTrabajo>(entity =>
            {
                entity.HasKey(e => new { e.DireccionId, e.SinodalDelTrabajoId })
                    .HasName("PK_DireccionSinodalDeTrabajo");

                entity.ToTable("DireccionSinodalDelTrabajo");

                entity.Property(e => e.DireccionId).HasColumnName("DireccionID");

                entity.Property(e => e.SinodalDelTrabajoId).HasColumnName("SinodalDelTrabajoID");

                entity.Property(e => e.Tipo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Direccion)
                    .WithMany(p => p.DireccionSinodalDelTrabajos)
                    .HasForeignKey(d => d.DireccionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DireccionSinodalDelTrabajo_Direccion");

                entity.HasOne(d => d.SinodalDelTrabajo)
                    .WithMany(p => p.DireccionSinodalDelTrabajos)
                    .HasForeignKey(d => d.SinodalDelTrabajoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DireccionSinodalDelTrabajo_SinodalDelTrabajo");
            });

            modelBuilder.Entity<Documento>(entity =>
            {
                entity.ToTable("Documento");

                entity.Property(e => e.DocumentoId).HasColumnName("DocumentoID");

                entity.Property(e => e.Notas).IsUnicode(false);

                entity.Property(e => e.TipoDocumentoId).HasColumnName("TipoDocumentoID");

                entity.Property(e => e.TrabajoRecepcionalId).HasColumnName("TrabajoRecepcionalID");

                entity.HasOne(d => d.TipoDocumento)
                    .WithMany(p => p.Documentos)
                    .HasForeignKey(d => d.TipoDocumentoId)
                    .HasConstraintName("Documento_FK");

                entity.HasOne(d => d.TrabajoRecepcional)
                    .WithMany(p => p.Documentos)
                    .HasForeignKey(d => d.TrabajoRecepcionalId)
                    .HasConstraintName("FK_Documento_TrabajoRecepcional");

                entity.HasMany(d => d.ExamenDefensas)
                    .WithMany(p => p.Documentos)
                    .UsingEntity<Dictionary<string, object>>(
                        "DocumentoExamenDefensa",
                        l => l.HasOne<ExamenDefensa>().WithMany().HasForeignKey("ExamenDefensaId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_DocumentoExamenDefensa_ExamenDefensa"),
                        r => r.HasOne<Documento>().WithMany().HasForeignKey("DocumentoId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_DocumentoExamenDefensa_Documento"),
                        j =>
                        {
                            j.HasKey("DocumentoId", "ExamenDefensaId");

                            j.ToTable("DocumentoExamenDefensa");

                            j.IndexerProperty<int>("DocumentoId").HasColumnName("DocumentoID");

                            j.IndexerProperty<int>("ExamenDefensaId").HasColumnName("ExamenDefensaID");
                        });
            });

            modelBuilder.Entity<ExamenDefensa>(entity =>
            {
                entity.ToTable("ExamenDefensa");

                entity.Property(e => e.ExamenDefensaId).HasColumnName("ExamenDefensaID");

                entity.Property(e => e.Estado)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FechaAplicacion).HasColumnType("date");

                entity.Property(e => e.TrabajoRecepcionalId).HasColumnName("TrabajoRecepcionalID");

                entity.HasOne(d => d.TrabajoRecepcional)
                    .WithMany(p => p.ExamenDefensas)
                    .HasForeignKey(d => d.TrabajoRecepcionalId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ExamenDeDefensa_TrabajoRecepcional");
            });

            modelBuilder.Entity<Expediente>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Expediente");

                entity.Property(e => e.CorreoElectronico)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Estado)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Fechadeinicio).HasColumnType("date");

                entity.Property(e => e.LineaDeInvestigacion).IsUnicode(false);

                entity.Property(e => e.Matricula)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Modalidad)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NombreAlumno)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.TrabajoRecepcionalId).HasColumnName("TrabajoRecepcionalID");
            });

            modelBuilder.Entity<ExperienciaEducativa>(entity =>
            {
                entity.ToTable("ExperienciaEducativa");

                entity.Property(e => e.ExperienciaEducativaId).HasColumnName("ExperienciaEducativaID");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Nrc)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Periodo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ProfesorId).HasColumnName("ProfesorID");

                entity.Property(e => e.Seccion)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.Profesor)
                    .WithMany(p => p.ExperienciaEducativas)
                    .HasForeignKey(d => d.ProfesorId)
                    .HasConstraintName("FK_ExperienciaEducativa_Profesor");
            });

            modelBuilder.Entity<Integrante>(entity =>
            {
                entity.ToTable("Integrante");

                entity.Property(e => e.IntegranteId).HasColumnName("IntegranteID");

                entity.Property(e => e.CuerpoAcademicoId).HasColumnName("CuerpoAcademicoID");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroDePersonal)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Tipo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.CuerpoAcademico)
                    .WithMany(p => p.Integrantes)
                    .HasForeignKey(d => d.CuerpoAcademicoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Integrante_FK");

                entity.HasMany(d => d.TrabajoRecepcionals)
                    .WithMany(p => p.Integrantes)
                    .UsingEntity<Dictionary<string, object>>(
                        "TrabajoRecepcionalIntegrante",
                        l => l.HasOne<TrabajoRecepcional>().WithMany().HasForeignKey("TrabajoRecepcionalId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_TrabajoRecepcionalIntegrante_TrabajoRecepcional"),
                        r => r.HasOne<Integrante>().WithMany().HasForeignKey("IntegranteId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_TrabajoRecepcionalIntegrante_Integrante"),
                        j =>
                        {
                            j.HasKey("IntegranteId", "TrabajoRecepcionalId");

                            j.ToTable("TrabajoRecepcionalIntegrante");

                            j.IndexerProperty<int>("IntegranteId").HasColumnName("IntegranteID");

                            j.IndexerProperty<int>("TrabajoRecepcionalId").HasColumnName("TrabajoRecepcionalID");
                        });
            });

            modelBuilder.Entity<Lgac>(entity =>
            {
                entity.ToTable("Lgac");

                entity.Property(e => e.LgacId).HasColumnName("LgacID");

                entity.Property(e => e.CuerpoAcademicoId).HasColumnName("CuerpoAcademicoID");

                entity.Property(e => e.Descripcion).IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Organizacion>(entity =>
            {
                entity.ToTable("Organizacion");

                entity.Property(e => e.OrganizacionId).HasColumnName("OrganizacionID");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Pladeafei>(entity =>
            {
                entity.ToTable("Pladeafei");

                entity.Property(e => e.PladeafeiId).HasColumnName("PladeafeiID");

                entity.Property(e => e.Accion).IsUnicode(false);

                entity.Property(e => e.ObjetivoGeneral).IsUnicode(false);

                entity.Property(e => e.Periodo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasMany(d => d.Lgacs)
                    .WithMany(p => p.Pladeafeis)
                    .UsingEntity<Dictionary<string, object>>(
                        "PladeafeiLgac",
                        l => l.HasOne<Lgac>().WithMany().HasForeignKey("LgacId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_PladeafeiLgac_lgac"),
                        r => r.HasOne<Pladeafei>().WithMany().HasForeignKey("PladeafeiId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_PladeafeiLgac_Pladeafei"),
                        j =>
                        {
                            j.HasKey("PladeafeiId", "LgacId");

                            j.ToTable("PladeafeiLgac");

                            j.IndexerProperty<int>("PladeafeiId").HasColumnName("PladeafeiID");

                            j.IndexerProperty<int>("LgacId").HasColumnName("LgacID");
                        });
            });

            modelBuilder.Entity<PlanDeCurso>(entity =>
            {
                entity.ToTable("PlanDeCurso");

                entity.Property(e => e.PlanDeCursoId).HasColumnName("PlanDeCursoID");

                entity.Property(e => e.ExperienciaEducativaId).HasColumnName("ExperienciaEducativaID");

                entity.Property(e => e.ObjetivoGeneral).IsUnicode(false);

                entity.HasOne(d => d.ExperienciaEducativa)
                    .WithMany(p => p.PlanDeCursos)
                    .HasForeignKey(d => d.ExperienciaEducativaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PlanDeCurso_ExperienciaEducativa");
            });

            modelBuilder.Entity<PlanDeTrabajo>(entity =>
            {
                entity.ToTable("PlanDeTrabajo");

                entity.Property(e => e.PlanDeTrabajoId).HasColumnName("PlanDeTrabajoID");

                entity.Property(e => e.AcademiaId).HasColumnName("AcademiaID");

                entity.Property(e => e.FechaDeAprobacion).HasColumnType("date");

                entity.Property(e => e.PeriodoEscolar)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Academia)
                    .WithMany(p => p.PlanDeTrabajos)
                    .HasForeignKey(d => d.AcademiaId)
                    .HasConstraintName("FK_PlanDeTrabajo_Academia");
            });

            modelBuilder.Entity<Profesor>(entity =>
            {
                entity.ToTable("Profesor");

                entity.Property(e => e.ProfesorId).HasColumnName("ProfesorID");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NombreUsuario)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroDePersonal)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ProyectoDeInvestigacion>(entity =>
            {
                entity.ToTable("ProyectoDeInvestigacion");

                entity.Property(e => e.ProyectoDeInvestigacionId).HasColumnName("ProyectoDeInvestigacionID");

                entity.Property(e => e.CuerpoAcademicoId).HasColumnName("CuerpoAcademicoID");

                entity.Property(e => e.FechaInicio).HasColumnType("date");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.CuerpoAcademico)
                    .WithMany(p => p.ProyectoDeInvestigacions)
                    .HasForeignKey(d => d.CuerpoAcademicoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ProyectoDeInvestigacion_FK");

                entity.HasMany(d => d.Lgacs)
                    .WithMany(p => p.ProyectoDeInvestigacions)
                    .UsingEntity<Dictionary<string, object>>(
                        "ProyectoDeInvestigacionLgac",
                        l => l.HasOne<Lgac>().WithMany().HasForeignKey("LgacId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_ProyectoDeInvestigacionLgac_Lgac"),
                        r => r.HasOne<ProyectoDeInvestigacion>().WithMany().HasForeignKey("ProyectoDeInvestigacionId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_ProyectoDeInvestigacionLgac_ProyectoDeInvestigacion"),
                        j =>
                        {
                            j.HasKey("ProyectoDeInvestigacionId", "LgacId");

                            j.ToTable("ProyectoDeInvestigacionLgac");

                            j.IndexerProperty<int>("ProyectoDeInvestigacionId").HasColumnName("ProyectoDeInvestigacionID");

                            j.IndexerProperty<int>("LgacId").HasColumnName("LgacID");
                        });
            });

            modelBuilder.Entity<SinodalDelTrabajo>(entity =>
            {
                entity.ToTable("SinodalDelTrabajo");

                entity.Property(e => e.SinodalDelTrabajoId).HasColumnName("SinodalDelTrabajoID");

                entity.Property(e => e.CorreoElectronico)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.OrganizacionId).HasColumnName("OrganizacionID");

                entity.Property(e => e.Telefono)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.HasOne(d => d.Organizacion)
                    .WithMany(p => p.SinodalDelTrabajos)
                    .HasForeignKey(d => d.OrganizacionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("SinodalDelTrabajo_FK");
            });

            modelBuilder.Entity<TipoDocumento>(entity =>
            {
                entity.HasKey(e => e.IdTipo)
                    .HasName("TipoDocumento_PK");

                entity.ToTable("TipoDocumento");

                entity.Property(e => e.ExperienciaEducativa)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.NombreDocumento)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TrabajoRecepcional>(entity =>
            {
                entity.ToTable("TrabajoRecepcional");

                entity.Property(e => e.TrabajoRecepcionalId).HasColumnName("TrabajoRecepcionalID");

                entity.Property(e => e.AcademiaId).HasColumnName("AcademiaID");

                entity.Property(e => e.Estado)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Fechadeinicio).HasColumnType("date");

                entity.Property(e => e.LineaDeInvestigacion).IsUnicode(false);

                entity.Property(e => e.Modalidad)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .HasMaxLength(200)
                    .IsUnicode(false);
                
                entity.Property(e => e.Duracion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.PladeafeiId).HasColumnName("PladeafeiID");

                entity.Property(e => e.ProyectoDeInvestigacionId).HasColumnName("ProyectoDeInvestigacionID");

                entity.Property(e => e.VinculacionId).HasColumnName("VinculacionID");

                entity.HasOne(d => d.Academia)
                    .WithMany(p => p.TrabajoRecepcionals)
                    .HasForeignKey(d => d.AcademiaId)
                    .HasConstraintName("FK_TrabajoRecepcional_Academia");

                entity.HasOne(d => d.Pladeafei)
                    .WithMany(p => p.TrabajoRecepcionals)
                    .HasForeignKey(d => d.PladeafeiId)
                    .HasConstraintName("FK_TrabajoRecepcional_Pladeafei");

                entity.HasOne(d => d.ProyectoDeInvestigacion)
                    .WithMany(p => p.TrabajoRecepcionals)
                    .HasForeignKey(d => d.ProyectoDeInvestigacionId)
                    .HasConstraintName("FK_TrabajoRecepcional_ProyectoDeInvestigacion");

                entity.HasOne(d => d.Vinculacion)
                    .WithMany(p => p.TrabajoRecepcionals)
                    .HasForeignKey(d => d.VinculacionId)
                    .HasConstraintName("FK_TrabajoRecepcional_Vinculacion");
            });

            modelBuilder.Entity<TrabajoRecepcionalSinodalDelTrabajo>(entity =>
            {
                entity.HasKey(e => new { e.SinodalDelTrabajoId, e.TrabajoRecepcionalId });

                entity.ToTable("TrabajoRecepcionalSinodalDelTrabajo");

                entity.Property(e => e.SinodalDelTrabajoId).HasColumnName("SinodalDelTrabajoID");

                entity.Property(e => e.TrabajoRecepcionalId).HasColumnName("TrabajoRecepcionalID");

                entity.Property(e => e.TipoSinodal)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.SinodalDelTrabajo)
                    .WithMany(p => p.TrabajoRecepcionalSinodalDelTrabajos)
                    .HasForeignKey(d => d.SinodalDelTrabajoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TrabajoRecepcionalSinodalDelTrabajo_SinodalDelTrabajo");

                entity.HasOne(d => d.TrabajoRecepcional)
                    .WithMany(p => p.TrabajoRecepcionalSinodalDelTrabajos)
                    .HasForeignKey(d => d.TrabajoRecepcionalId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TrabajoRecepcionalSinodalDelTrabajo_TrabajoRecepcional");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.IdUsuario)
                    .HasName("Usuario_PK");

                entity.ToTable("Usuario");

                entity.Property(e => e.Contra)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.NombreUsuario)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Tipo)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Vinculacion>(entity =>
            {
                entity.ToTable("Vinculacion");

                entity.Property(e => e.VinculacionId).HasColumnName("VinculacionID");

                entity.Property(e => e.FechaDeInicioDeConvenio).HasColumnType("date");

                entity.Property(e => e.OrganizacionIid).HasColumnName("OrganizacionIID");

                entity.HasOne(d => d.OrganizacionI)
                    .WithMany(p => p.Vinculacions)
                    .HasForeignKey(d => d.OrganizacionIid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Vinculacion_FK");

                entity.HasMany(d => d.Lgacs)
                    .WithMany(p => p.Vinculacions)
                    .UsingEntity<Dictionary<string, object>>(
                        "VinculacionLgac",
                        l => l.HasOne<Lgac>().WithMany().HasForeignKey("LgacId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_VinculacionLgac_Lgac"),
                        r => r.HasOne<Vinculacion>().WithMany().HasForeignKey("VinculacionId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_VinculacionLgac_Vinculacion"),
                        j =>
                        {
                            j.HasKey("VinculacionId", "LgacId");

                            j.ToTable("VinculacionLgac");

                            j.IndexerProperty<int>("VinculacionId").HasColumnName("VinculacionID");

                            j.IndexerProperty<int>("LgacId").HasColumnName("LgacID");
                        });
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
