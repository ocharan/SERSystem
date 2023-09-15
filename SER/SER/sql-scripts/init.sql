CREATE DATABASE ser;
GO 

USE [ser]
GO
/****** Object:  Table [dbo].[AlumnoTrabajoRecepcional]    Script Date: 07/09/2023 02:23:10 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AlumnoTrabajoRecepcional](
	[AlumnoID] [varchar](9) NOT NULL,
	[TrabajoRecepcionalID] [int] NOT NULL,
	[Nombre] [varchar](100) NULL,
 CONSTRAINT [PK_AlumnoTrabajoRecepcional] PRIMARY KEY CLUSTERED 
(
	[AlumnoID] ASC,
	[TrabajoRecepcionalID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TrabajoRecepcional]    Script Date: 07/09/2023 02:23:10 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TrabajoRecepcional](
	[TrabajoRecepcionalID] [int] IDENTITY(1,1) NOT NULL,
	[Estado] [varchar](100) NULL,
	[Fechadeinicio] [date] NULL,
	[LineaDeInvestigacion] [varchar](max) NULL,
	[Modalidad] [varchar](100) NULL,
	[Nombre] [varchar](200) NULL,
	[AcademiaID] [int] NULL,
	[PladeafeiID] [int] NULL,
	[ProyectoDeInvestigacionID] [int] NULL,
	[VinculacionID] [int] NULL,
	[Duracion] [varchar](100) NULL,
	[ExperienciaActual] [varchar](100) NULL,
	[JustificacionAlumnos] [varchar](100) NULL,
 CONSTRAINT [PK_TrabajoRecepcional] PRIMARY KEY CLUSTERED 
(
	[TrabajoRecepcionalID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  View [dbo].[AlumnoTrabajoRecepcionalProyectoGuiadoView]    Script Date: 07/09/2023 02:23:10 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[AlumnoTrabajoRecepcionalProyectoGuiadoView]
AS
SELECT dbo.Alumno.CorreoElectronico, dbo.Alumno.Matricula, dbo.Alumno.Nombre, dbo.TrabajoRecepcional.Estado, dbo.TrabajoRecepcional.Fechadeinicio, dbo.TrabajoRecepcional.Modalidad, 
                  dbo.ExperienciaEducativa.Nombre AS ExperienciaEducativa, dbo.ExperienciaEducativa.ExperienciaEducativaID, dbo.TrabajoRecepcional.TrabajoRecepcionalID
FROM     dbo.Alumno INNER JOIN
                  dbo.AlumnoTrabajoRecepcional ON dbo.Alumno.Matricula = dbo.AlumnoTrabajoRecepcional.AlumnoID INNER JOIN
                  dbo.TrabajoRecepcional ON dbo.AlumnoTrabajoRecepcional.TrabajoRecepcionalID = dbo.TrabajoRecepcional.TrabajoRecepcionalID INNER JOIN
                  dbo.AlumnoExperienciaEducativa ON dbo.Alumno.Matricula = dbo.AlumnoExperienciaEducativa.AlumnoID INNER JOIN
                  dbo.ExperienciaEducativa ON dbo.AlumnoExperienciaEducativa.ExperienciaEducativaID = dbo.ExperienciaEducativa.ExperienciaEducativaID
WHERE  (dbo.ExperienciaEducativa.Nombre = 'Proyecto Guiado');
GO
/****** Object:  View [dbo].[Expediente]    Script Date: 07/09/2023 02:23:10 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- dbo.Expediente source

-- dbo.Expediente source

CREATE VIEW [dbo].[Expediente] AS SELECT dbo.Alumno.Nombre as NombreAlumno, dbo.Alumno.Matricula, dbo.Alumno.CorreoElectronico, dbo.TrabajoRecepcional.Nombre,
dbo.TrabajoRecepcional.TrabajoRecepcionalID, dbo.TrabajoRecepcional.Estado, dbo.TrabajoRecepcional.Fechadeinicio,
dbo.TrabajoRecepcional.Modalidad, dbo.TrabajoRecepcional.LineaDeInvestigacion from dbo.Alumno inner join 
dbo.AlumnoTrabajoRecepcional on dbo.AlumnoTrabajoRecepcional.AlumnoID  = Matricula 
inner JOIN dbo.TrabajoRecepcional on dbo.TrabajoRecepcional.TrabajoRecepcionalID  = AlumnoTrabajoRecepcional.TrabajoRecepcionalID;
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 07/09/2023 02:23:10 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Academia]    Script Date: 07/09/2023 02:23:10 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Academia](
	[AcademiaID] [int] IDENTITY(1,1) NOT NULL,
	[Descripcion] [varchar](max) NULL,
	[Nombre] [varchar](200) NULL,
 CONSTRAINT [PK_Academia] PRIMARY KEY CLUSTERED 
(
	[AcademiaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AcademicBody]    Script Date: 07/09/2023 02:23:10 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AcademicBody](
	[AcademicBodyId] [int] IDENTITY(1,1) NOT NULL,
	[AcademicBodyKey] [varchar](50) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[IES] [varchar](50) NOT NULL,
	[ConsolidationDegree] [varchar](100) NOT NULL,
	[Discipline] [varchar](100) NOT NULL,
 CONSTRAINT [PK_AcademicBody] PRIMARY KEY CLUSTERED 
(
	[AcademicBodyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AcademicBodyLgac]    Script Date: 07/09/2023 02:23:10 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AcademicBodyLgac](
	[AcademicBodyLgac] [int] IDENTITY(1,1) NOT NULL,
	[AcademicBodyId] [int] NOT NULL,
	[LgacId] [int] NOT NULL,
 CONSTRAINT [PK_AcademicBodyLgac] PRIMARY KEY CLUSTERED 
(
	[AcademicBodyLgac] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AcademicBodyMember]    Script Date: 07/09/2023 02:23:10 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AcademicBodyMember](
	[AcademicBodyMemberId] [int] IDENTITY(1,1) NOT NULL,
	[AcademicBodyId] [int] NOT NULL,
	[ProfessorId] [int] NOT NULL,
	[Role] [varchar](15) NOT NULL,
 CONSTRAINT [PK_AcademicBodyMember] PRIMARY KEY CLUSTERED 
(
	[AcademicBodyMemberId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Course]    Script Date: 07/09/2023 02:23:10 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Course](
	[CourseId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](200) NOT NULL,
	[Nrc] [varchar](5) NOT NULL,
	[Period] [varchar](100) NOT NULL,
	[Section] [varchar](1) NOT NULL,
	[IsOpen] [bit] NOT NULL,
	[ProfessorId] [int] NULL,
	[FileId] [int] NULL,
 CONSTRAINT [PK_Course] PRIMARY KEY CLUSTERED 
(
	[CourseId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [Unique_Course_Nrc] UNIQUE NONCLUSTERED 
(
	[Nrc] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CourseFile]    Script Date: 07/09/2023 02:23:10 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CourseFile](
	[FileId] [int] IDENTITY(1,1) NOT NULL,
	[Path] [varchar](200) NOT NULL,
 CONSTRAINT [PK_CourseFile] PRIMARY KEY CLUSTERED 
(
	[FileId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CourseRegistration]    Script Date: 07/09/2023 02:23:10 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CourseRegistration](
	[CourseRegistrationId] [int] IDENTITY(1,1) NOT NULL,
	[StudentId] [int] NOT NULL,
	[CourseId] [int] NOT NULL,
	[Score] [int] NULL,
	[RegistrationType] [varchar](15) NOT NULL,
 CONSTRAINT [PK_CourseRegistration] PRIMARY KEY CLUSTERED 
(
	[CourseRegistrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Direccion]    Script Date: 07/09/2023 02:23:10 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Direccion](
	[FechaInicio] [date] NULL,
	[DireccionID] [int] IDENTITY(1,1) NOT NULL,
	[ExperienciaEducativaID] [int] NOT NULL,
 CONSTRAINT [PK_Direccion] PRIMARY KEY CLUSTERED 
(
	[DireccionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DireccionSinodalDelTrabajo]    Script Date: 07/09/2023 02:23:10 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DireccionSinodalDelTrabajo](
	[DireccionID] [int] NOT NULL,
	[SinodalDelTrabajoID] [int] NOT NULL,
	[Tipo] [varchar](100) NULL,
 CONSTRAINT [PK_DireccionSinodalDeTrabajo] PRIMARY KEY CLUSTERED 
(
	[DireccionID] ASC,
	[SinodalDelTrabajoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Documento]    Script Date: 07/09/2023 02:23:10 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Documento](
	[DocumentoID] [int] IDENTITY(1,1) NOT NULL,
	[TrabajoRecepcionalID] [int] NULL,
	[Notas] [varchar](max) NULL,
	[TipoDocumentoID] [int] NULL,
 CONSTRAINT [PK_Documento] PRIMARY KEY CLUSTERED 
(
	[DocumentoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DocumentoExamenDefensa]    Script Date: 07/09/2023 02:23:10 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DocumentoExamenDefensa](
	[DocumentoID] [int] NOT NULL,
	[ExamenDefensaID] [int] NOT NULL,
 CONSTRAINT [PK_DocumentoExamenDefensa] PRIMARY KEY CLUSTERED 
(
	[DocumentoID] ASC,
	[ExamenDefensaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ExamenDefensa]    Script Date: 07/09/2023 02:23:10 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExamenDefensa](
	[FechaAplicacion] [date] NULL,
	[ExamenDefensaID] [int] IDENTITY(1,1) NOT NULL,
	[TrabajoRecepcionalID] [int] NOT NULL,
	[Estado] [varchar](100) NULL,
 CONSTRAINT [PK_ExamenDefensa] PRIMARY KEY CLUSTERED 
(
	[ExamenDefensaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Lgac]    Script Date: 07/09/2023 02:23:10 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Lgac](
	[LgacId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Description] [varchar](max) NOT NULL,
 CONSTRAINT [PK_Lgac] PRIMARY KEY CLUSTERED 
(
	[LgacId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Organizacion]    Script Date: 07/09/2023 02:23:10 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Organizacion](
	[Nombre] [varchar](100) NULL,
	[OrganizacionID] [int] IDENTITY(0,1) NOT NULL,
 CONSTRAINT [Organizacion_PK] PRIMARY KEY CLUSTERED 
(
	[OrganizacionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Pladeafei]    Script Date: 07/09/2023 02:23:10 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Pladeafei](
	[Accion] [varchar](max) NULL,
	[ObjetivoGeneral] [varchar](max) NULL,
	[Periodo] [varchar](100) NULL,
	[PladeafeiID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_Pladeafei] PRIMARY KEY CLUSTERED 
(
	[PladeafeiID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PladeafeiLgac]    Script Date: 07/09/2023 02:23:10 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PladeafeiLgac](
	[PladeafeiID] [int] NOT NULL,
	[LgacID] [int] NOT NULL,
 CONSTRAINT [PK_PladeafeiLgac] PRIMARY KEY CLUSTERED 
(
	[PladeafeiID] ASC,
	[LgacID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PlanDeCurso]    Script Date: 07/09/2023 02:23:10 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PlanDeCurso](
	[ObjetivoGeneral] [varchar](max) NULL,
	[PlanDeCursoID] [int] IDENTITY(1,1) NOT NULL,
	[ExperienciaEducativaID] [int] NOT NULL,
 CONSTRAINT [PK_PlanDeCurso] PRIMARY KEY CLUSTERED 
(
	[PlanDeCursoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PlanDeTrabajo]    Script Date: 07/09/2023 02:23:10 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PlanDeTrabajo](
	[FechaDeAprobacion] [date] NULL,
	[PeriodoEscolar] [varchar](100) NULL,
	[PlanDeTrabajoID] [int] IDENTITY(1,1) NOT NULL,
	[AcademiaID] [int] NULL,
 CONSTRAINT [PK_PlanDeTrabajo] PRIMARY KEY CLUSTERED 
(
	[PlanDeTrabajoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Professor]    Script Date: 07/09/2023 02:23:10 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Professor](
	[ProfessorId] [int] IDENTITY(1,1) NOT NULL,
	[FullName] [varchar](200) NOT NULL,
	[AcademicDegree] [varchar](20) NOT NULL,
	[StudyField] [varchar](100) NOT NULL,
	[UserId] [int] NOT NULL,
 CONSTRAINT [PK_Professor] PRIMARY KEY CLUSTERED 
(
	[ProfessorId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProyectoDeInvestigacion]    Script Date: 07/09/2023 02:23:10 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProyectoDeInvestigacion](
	[FechaInicio] [date] NULL,
	[Nombre] [varchar](200) NULL,
	[ProyectoDeInvestigacionID] [int] IDENTITY(1,1) NOT NULL,
	[CuerpoAcademicoID] [int] NOT NULL,
 CONSTRAINT [PK_ProyectoDeInvestigacion] PRIMARY KEY CLUSTERED 
(
	[ProyectoDeInvestigacionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProyectoDeInvestigacionLgac]    Script Date: 07/09/2023 02:23:10 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProyectoDeInvestigacionLgac](
	[ProyectoDeInvestigacionID] [int] NOT NULL,
	[LgacID] [int] NOT NULL,
 CONSTRAINT [PK_ProyectoDeInvestigacionLgac] PRIMARY KEY CLUSTERED 
(
	[ProyectoDeInvestigacionID] ASC,
	[LgacID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SinodalDelTrabajo]    Script Date: 07/09/2023 02:23:10 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SinodalDelTrabajo](
	[CorreoElectronico] [varchar](100) NULL,
	[Telefono] [varchar](10) NULL,
	[SinodalDelTrabajoID] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](200) NULL,
	[NumeroDePersonal] [int] NULL,
	[OrganizacionID] [int] NOT NULL,
 CONSTRAINT [PK_SinodalDelTrabajo] PRIMARY KEY CLUSTERED 
(
	[SinodalDelTrabajoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Student]    Script Date: 07/09/2023 02:23:10 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Student](
	[StudentId] [int] IDENTITY(1,1) NOT NULL,
	[Enrollment] [varchar](9) NOT NULL,
	[FullName] [varchar](200) NOT NULL,
	[Email] [varchar](100) NOT NULL,
 CONSTRAINT [PK_Student] PRIMARY KEY CLUSTERED 
(
	[StudentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [Unique_Student_Email] UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [Unique_Student_Enrollment] UNIQUE NONCLUSTERED 
(
	[Enrollment] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TipoDocumento]    Script Date: 07/09/2023 02:23:10 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TipoDocumento](
	[NombreDocumento] [varchar](100) NULL,
	[ExperienciaEducativa] [varchar](100) NULL,
	[IdTipo] [int] IDENTITY(0,1) NOT NULL,
 CONSTRAINT [TipoDocumento_PK] PRIMARY KEY CLUSTERED 
(
	[IdTipo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TrabajoRecepcionalIntegrante]    Script Date: 07/09/2023 02:23:10 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TrabajoRecepcionalIntegrante](
	[IntegranteID] [int] NOT NULL,
	[TrabajoRecepcionalID] [int] NOT NULL,
 CONSTRAINT [PK_TrabajoRecepcionalIntegrante] PRIMARY KEY CLUSTERED 
(
	[IntegranteID] ASC,
	[TrabajoRecepcionalID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TrabajoRecepcionalSinodalDelTrabajo]    Script Date: 07/09/2023 02:23:10 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TrabajoRecepcionalSinodalDelTrabajo](
	[SinodalDelTrabajoID] [int] NOT NULL,
	[TrabajoRecepcionalID] [int] NOT NULL,
	[TipoSinodal] [varchar](100) NULL,
 CONSTRAINT [PK_TrabajoRecepcionalSinodalDelTrabajo] PRIMARY KEY CLUSTERED 
(
	[SinodalDelTrabajoID] ASC,
	[TrabajoRecepcionalID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 07/09/2023 02:23:10 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[UserId] [int] IDENTITY(0,1) NOT NULL,
	[Username] [varchar](50) NOT NULL,
	[Password] [varchar](100) NOT NULL,
	[Email] [varchar](64) NOT NULL,
	[Role] [varchar](20) NOT NULL,
	[RecoveryToken] [varchar](300) NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [Unique_User_Email] UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [Unique_User_Username] UNIQUE NONCLUSTERED 
(
	[Username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Vinculacion]    Script Date: 07/09/2023 02:23:10 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Vinculacion](
	[FechaDeInicioDeConvenio] [date] NULL,
	[VinculacionID] [int] IDENTITY(1,1) NOT NULL,
	[OrganizacionIID] [int] NOT NULL,
 CONSTRAINT [PK_Vinculacion] PRIMARY KEY CLUSTERED 
(
	[VinculacionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[VinculacionLgac]    Script Date: 07/09/2023 02:23:10 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VinculacionLgac](
	[VinculacionID] [int] NOT NULL,
	[LgacID] [int] NOT NULL,
 CONSTRAINT [PK_VinculacionLgac] PRIMARY KEY CLUSTERED 
(
	[VinculacionID] ASC,
	[LgacID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AcademicBodyLgac]  WITH CHECK ADD  CONSTRAINT [FK_AcademicBodyLgac_AcademicBody] FOREIGN KEY([AcademicBodyId])
REFERENCES [dbo].[AcademicBody] ([AcademicBodyId])
GO
ALTER TABLE [dbo].[AcademicBodyLgac] CHECK CONSTRAINT [FK_AcademicBodyLgac_AcademicBody]
GO
ALTER TABLE [dbo].[AcademicBodyLgac]  WITH CHECK ADD  CONSTRAINT [FK_AcademicBodyLgac_Lgac] FOREIGN KEY([LgacId])
REFERENCES [dbo].[Lgac] ([LgacId])
GO
ALTER TABLE [dbo].[AcademicBodyLgac] CHECK CONSTRAINT [FK_AcademicBodyLgac_Lgac]
GO
ALTER TABLE [dbo].[AcademicBodyMember]  WITH CHECK ADD  CONSTRAINT [FK_AcademicBodyMember_AcademicBody] FOREIGN KEY([AcademicBodyId])
REFERENCES [dbo].[AcademicBody] ([AcademicBodyId])
GO
ALTER TABLE [dbo].[AcademicBodyMember] CHECK CONSTRAINT [FK_AcademicBodyMember_AcademicBody]
GO
ALTER TABLE [dbo].[AcademicBodyMember]  WITH CHECK ADD  CONSTRAINT [FK_AcademicBodyMember_Professor] FOREIGN KEY([ProfessorId])
REFERENCES [dbo].[Professor] ([ProfessorId])
GO
ALTER TABLE [dbo].[AcademicBodyMember] CHECK CONSTRAINT [FK_AcademicBodyMember_Professor]
GO
ALTER TABLE [dbo].[AlumnoTrabajoRecepcional]  WITH CHECK ADD  CONSTRAINT [FK_AlumnoTrabajoRecepcional_TrabajoRecepcional] FOREIGN KEY([TrabajoRecepcionalID])
REFERENCES [dbo].[TrabajoRecepcional] ([TrabajoRecepcionalID])
GO
ALTER TABLE [dbo].[AlumnoTrabajoRecepcional] CHECK CONSTRAINT [FK_AlumnoTrabajoRecepcional_TrabajoRecepcional]
GO
ALTER TABLE [dbo].[Course]  WITH CHECK ADD  CONSTRAINT [FK_Course_CourseFile] FOREIGN KEY([FileId])
REFERENCES [dbo].[CourseFile] ([FileId])
ON UPDATE CASCADE
ON DELETE SET DEFAULT
GO
ALTER TABLE [dbo].[Course] CHECK CONSTRAINT [FK_Course_CourseFile]
GO
ALTER TABLE [dbo].[Course]  WITH CHECK ADD  CONSTRAINT [FK_Course_Profesor] FOREIGN KEY([ProfessorId])
REFERENCES [dbo].[Professor] ([ProfessorId])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[Course] CHECK CONSTRAINT [FK_Course_Profesor]
GO
ALTER TABLE [dbo].[CourseRegistration]  WITH CHECK ADD  CONSTRAINT [FK_CourseRegistration_Course] FOREIGN KEY([CourseId])
REFERENCES [dbo].[Course] ([CourseId])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[CourseRegistration] CHECK CONSTRAINT [FK_CourseRegistration_Course]
GO
ALTER TABLE [dbo].[CourseRegistration]  WITH CHECK ADD  CONSTRAINT [FK_CourseRegistration_Student] FOREIGN KEY([StudentId])
REFERENCES [dbo].[Student] ([StudentId])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[CourseRegistration] CHECK CONSTRAINT [FK_CourseRegistration_Student]
GO
ALTER TABLE [dbo].[DireccionSinodalDelTrabajo]  WITH CHECK ADD  CONSTRAINT [FK_DireccionSinodalDelTrabajo_Direccion] FOREIGN KEY([DireccionID])
REFERENCES [dbo].[Direccion] ([DireccionID])
GO
ALTER TABLE [dbo].[DireccionSinodalDelTrabajo] CHECK CONSTRAINT [FK_DireccionSinodalDelTrabajo_Direccion]
GO
ALTER TABLE [dbo].[DireccionSinodalDelTrabajo]  WITH CHECK ADD  CONSTRAINT [FK_DireccionSinodalDelTrabajo_SinodalDelTrabajo] FOREIGN KEY([SinodalDelTrabajoID])
REFERENCES [dbo].[SinodalDelTrabajo] ([SinodalDelTrabajoID])
GO
ALTER TABLE [dbo].[DireccionSinodalDelTrabajo] CHECK CONSTRAINT [FK_DireccionSinodalDelTrabajo_SinodalDelTrabajo]
GO
ALTER TABLE [dbo].[Documento]  WITH CHECK ADD  CONSTRAINT [Documento_FK] FOREIGN KEY([TipoDocumentoID])
REFERENCES [dbo].[TipoDocumento] ([IdTipo])
GO
ALTER TABLE [dbo].[Documento] CHECK CONSTRAINT [Documento_FK]
GO
ALTER TABLE [dbo].[Documento]  WITH CHECK ADD  CONSTRAINT [FK_Documento_TrabajoRecepcional] FOREIGN KEY([TrabajoRecepcionalID])
REFERENCES [dbo].[TrabajoRecepcional] ([TrabajoRecepcionalID])
GO
ALTER TABLE [dbo].[Documento] CHECK CONSTRAINT [FK_Documento_TrabajoRecepcional]
GO
ALTER TABLE [dbo].[DocumentoExamenDefensa]  WITH CHECK ADD  CONSTRAINT [FK_DocumentoExamenDefensa_Documento] FOREIGN KEY([DocumentoID])
REFERENCES [dbo].[Documento] ([DocumentoID])
GO
ALTER TABLE [dbo].[DocumentoExamenDefensa] CHECK CONSTRAINT [FK_DocumentoExamenDefensa_Documento]
GO
ALTER TABLE [dbo].[DocumentoExamenDefensa]  WITH CHECK ADD  CONSTRAINT [FK_DocumentoExamenDefensa_ExamenDefensa] FOREIGN KEY([ExamenDefensaID])
REFERENCES [dbo].[ExamenDefensa] ([ExamenDefensaID])
GO
ALTER TABLE [dbo].[DocumentoExamenDefensa] CHECK CONSTRAINT [FK_DocumentoExamenDefensa_ExamenDefensa]
GO
ALTER TABLE [dbo].[ExamenDefensa]  WITH CHECK ADD  CONSTRAINT [FK_ExamenDeDefensa_TrabajoRecepcional] FOREIGN KEY([TrabajoRecepcionalID])
REFERENCES [dbo].[TrabajoRecepcional] ([TrabajoRecepcionalID])
GO
ALTER TABLE [dbo].[ExamenDefensa] CHECK CONSTRAINT [FK_ExamenDeDefensa_TrabajoRecepcional]
GO
ALTER TABLE [dbo].[PladeafeiLgac]  WITH CHECK ADD  CONSTRAINT [FK_PladeafeiLgac_lgac] FOREIGN KEY([LgacID])
REFERENCES [dbo].[Lgac] ([LgacId])
GO
ALTER TABLE [dbo].[PladeafeiLgac] CHECK CONSTRAINT [FK_PladeafeiLgac_lgac]
GO
ALTER TABLE [dbo].[PladeafeiLgac]  WITH CHECK ADD  CONSTRAINT [FK_PladeafeiLgac_Pladeafei] FOREIGN KEY([PladeafeiID])
REFERENCES [dbo].[Pladeafei] ([PladeafeiID])
GO
ALTER TABLE [dbo].[PladeafeiLgac] CHECK CONSTRAINT [FK_PladeafeiLgac_Pladeafei]
GO
ALTER TABLE [dbo].[Professor]  WITH CHECK ADD  CONSTRAINT [FK_Profesor_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[Professor] CHECK CONSTRAINT [FK_Profesor_User]
GO
ALTER TABLE [dbo].[ProyectoDeInvestigacionLgac]  WITH CHECK ADD  CONSTRAINT [FK_ProyectoDeInvestigacionLgac_Lgac] FOREIGN KEY([LgacID])
REFERENCES [dbo].[Lgac] ([LgacId])
GO
ALTER TABLE [dbo].[ProyectoDeInvestigacionLgac] CHECK CONSTRAINT [FK_ProyectoDeInvestigacionLgac_Lgac]
GO
ALTER TABLE [dbo].[ProyectoDeInvestigacionLgac]  WITH CHECK ADD  CONSTRAINT [FK_ProyectoDeInvestigacionLgac_ProyectoDeInvestigacion] FOREIGN KEY([ProyectoDeInvestigacionID])
REFERENCES [dbo].[ProyectoDeInvestigacion] ([ProyectoDeInvestigacionID])
GO
ALTER TABLE [dbo].[ProyectoDeInvestigacionLgac] CHECK CONSTRAINT [FK_ProyectoDeInvestigacionLgac_ProyectoDeInvestigacion]
GO
ALTER TABLE [dbo].[SinodalDelTrabajo]  WITH CHECK ADD  CONSTRAINT [SinodalDelTrabajo_FK] FOREIGN KEY([OrganizacionID])
REFERENCES [dbo].[Organizacion] ([OrganizacionID])
GO
ALTER TABLE [dbo].[SinodalDelTrabajo] CHECK CONSTRAINT [SinodalDelTrabajo_FK]
GO
ALTER TABLE [dbo].[TrabajoRecepcional]  WITH CHECK ADD  CONSTRAINT [FK_TrabajoRecepcional_Pladeafei] FOREIGN KEY([PladeafeiID])
REFERENCES [dbo].[Pladeafei] ([PladeafeiID])
GO
ALTER TABLE [dbo].[TrabajoRecepcional] CHECK CONSTRAINT [FK_TrabajoRecepcional_Pladeafei]
GO
ALTER TABLE [dbo].[TrabajoRecepcional]  WITH CHECK ADD  CONSTRAINT [FK_TrabajoRecepcional_ProyectoDeInvestigacion] FOREIGN KEY([ProyectoDeInvestigacionID])
REFERENCES [dbo].[ProyectoDeInvestigacion] ([ProyectoDeInvestigacionID])
GO
ALTER TABLE [dbo].[TrabajoRecepcional] CHECK CONSTRAINT [FK_TrabajoRecepcional_ProyectoDeInvestigacion]
GO
ALTER TABLE [dbo].[TrabajoRecepcional]  WITH CHECK ADD  CONSTRAINT [FK_TrabajoRecepcional_Vinculacion] FOREIGN KEY([VinculacionID])
REFERENCES [dbo].[Vinculacion] ([VinculacionID])
GO
ALTER TABLE [dbo].[TrabajoRecepcional] CHECK CONSTRAINT [FK_TrabajoRecepcional_Vinculacion]
GO
ALTER TABLE [dbo].[TrabajoRecepcionalIntegrante]  WITH CHECK ADD  CONSTRAINT [FK_TrabajoRecepcionalIntegrante_TrabajoRecepcional] FOREIGN KEY([TrabajoRecepcionalID])
REFERENCES [dbo].[TrabajoRecepcional] ([TrabajoRecepcionalID])
GO
ALTER TABLE [dbo].[TrabajoRecepcionalIntegrante] CHECK CONSTRAINT [FK_TrabajoRecepcionalIntegrante_TrabajoRecepcional]
GO
ALTER TABLE [dbo].[TrabajoRecepcionalSinodalDelTrabajo]  WITH CHECK ADD  CONSTRAINT [FK_TrabajoRecepcionalSinodalDelTrabajo_SinodalDelTrabajo] FOREIGN KEY([SinodalDelTrabajoID])
REFERENCES [dbo].[SinodalDelTrabajo] ([SinodalDelTrabajoID])
GO
ALTER TABLE [dbo].[TrabajoRecepcionalSinodalDelTrabajo] CHECK CONSTRAINT [FK_TrabajoRecepcionalSinodalDelTrabajo_SinodalDelTrabajo]
GO
ALTER TABLE [dbo].[TrabajoRecepcionalSinodalDelTrabajo]  WITH CHECK ADD  CONSTRAINT [FK_TrabajoRecepcionalSinodalDelTrabajo_TrabajoRecepcional] FOREIGN KEY([TrabajoRecepcionalID])
REFERENCES [dbo].[TrabajoRecepcional] ([TrabajoRecepcionalID])
GO
ALTER TABLE [dbo].[TrabajoRecepcionalSinodalDelTrabajo] CHECK CONSTRAINT [FK_TrabajoRecepcionalSinodalDelTrabajo_TrabajoRecepcional]
GO
ALTER TABLE [dbo].[Vinculacion]  WITH CHECK ADD  CONSTRAINT [Vinculacion_FK] FOREIGN KEY([OrganizacionIID])
REFERENCES [dbo].[Organizacion] ([OrganizacionID])
GO
ALTER TABLE [dbo].[Vinculacion] CHECK CONSTRAINT [Vinculacion_FK]
GO
ALTER TABLE [dbo].[VinculacionLgac]  WITH CHECK ADD  CONSTRAINT [FK_VinculacionLgac_Lgac] FOREIGN KEY([LgacID])
REFERENCES [dbo].[Lgac] ([LgacId])
GO
ALTER TABLE [dbo].[VinculacionLgac] CHECK CONSTRAINT [FK_VinculacionLgac_Lgac]
GO
ALTER TABLE [dbo].[VinculacionLgac]  WITH CHECK ADD  CONSTRAINT [FK_VinculacionLgac_Vinculacion] FOREIGN KEY([VinculacionID])
REFERENCES [dbo].[Vinculacion] ([VinculacionID])
GO
ALTER TABLE [dbo].[VinculacionLgac] CHECK CONSTRAINT [FK_VinculacionLgac_Vinculacion]
GO
