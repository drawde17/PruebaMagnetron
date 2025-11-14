USE [PruebaMagnetron]
GO
/****** Object:  Table [dbo].[Persona]    Script Date: 13/11/2025 7:56:00 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Persona](
	[Per_ID] [int] IDENTITY(1,1) NOT NULL,
	[Per_Nombre] [varchar](100) NOT NULL,
	[Per_Apellido] [varchar](100) NOT NULL,
	[Per_TipoDocumento] [int] NOT NULL,
	[Per_Documento] [varchar](15) NOT NULL,
 CONSTRAINT [PK_Persona] PRIMARY KEY CLUSTERED 
(
	[Per_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Fact_Encabezado]    Script Date: 13/11/2025 7:56:00 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Fact_Encabezado](
	[FEnc_ID] [int] IDENTITY(1,1) NOT NULL,
	[FEnc_Numero] [int] NOT NULL,
	[FEnc_Fecha] [datetime] NOT NULL,
	[zPer_ID] [int] NOT NULL,
 CONSTRAINT [PK_Fact_Encabezado] PRIMARY KEY CLUSTERED 
(
	[FEnc_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tipo_Documento]    Script Date: 13/11/2025 7:56:00 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tipo_Documento](
	[TDoc_ID] [int] IDENTITY(1,1) NOT NULL,
	[TDoc_Nombre] [varchar](30) NOT NULL,
	[TDoc_Activo] [bit] NOT NULL,
 CONSTRAINT [PK_Tipo_Documento] PRIMARY KEY CLUSTERED 
(
	[TDoc_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Producto]    Script Date: 13/11/2025 7:56:00 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Producto](
	[Prod_ID] [int] IDENTITY(1,1) NOT NULL,
	[Prod_Descripcion] [varchar](200) NOT NULL,
	[Prod_Precio] [decimal](18, 2) NOT NULL,
	[Prod_Costo] [decimal](18, 2) NOT NULL,
	[Prod_UM] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Producto] PRIMARY KEY CLUSTERED 
(
	[Prod_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Fact_Detalle]    Script Date: 13/11/2025 7:56:00 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Fact_Detalle](
	[FDet_ID] [int] IDENTITY(1,1) NOT NULL,
	[FDet_Linea] [varchar](100) NOT NULL,
	[FDet_Cantidad] [int] NOT NULL,
	[zProd_ID] [int] NOT NULL,
	[zFEnc_ID] [int] NOT NULL,
 CONSTRAINT [PK_Fact_Detalle] PRIMARY KEY CLUSTERED 
(
	[FDet_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[View_PersonaFacturado]    Script Date: 13/11/2025 7:56:00 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   VIEW [dbo].[View_PersonaFacturado]
AS
	SELECT DISTINCT
		P.Per_Nombre, 
		P.Per_Apellido, 
		TD.TDoc_Nombre, 
		P.Per_Documento,
		ISNULL(S.Facturado, 0) AS Facturado
	FROM dbo.Persona AS P
		INNER JOIN dbo.Tipo_Documento AS TD ON P.Per_TipoDocumento = TD.TDoc_ID 
		LEFT JOIN (
			SELECT
				FE.zPer_ID,
				SUM(PR.Prod_Precio * FD.FDet_Cantidad) AS Facturado
			FROM dbo.Fact_Encabezado AS FE 
			INNER JOIN dbo.Fact_Detalle AS FD ON FE.FEnc_ID = FD.zFEnc_ID
			INNER JOIN dbo.Producto AS PR ON FD.zProd_ID = PR.Prod_ID
			GROUP BY FE.zPer_ID
		) AS S ON P.Per_ID = S.zPer_ID 
		
GO
/****** Object:  View [dbo].[View_PersonaProductoCaro]    Script Date: 13/11/2025 7:56:00 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE       VIEW [dbo].[View_PersonaProductoCaro]
AS
	SELECT DISTINCT TOP 1 
		P.Per_Nombre, 
		P.Per_Apellido, 
		TD.TDoc_Nombre, 
		P.Per_Documento,
		PR.Prod_Descripcion AS Producto,
		PR.Prod_Precio
	FROM dbo.Persona AS P
		INNER JOIN dbo.Tipo_Documento AS TD ON P.Per_TipoDocumento = TD.TDoc_ID 
		INNER JOIN dbo.Fact_Encabezado AS FE ON P.Per_ID = FE.zPer_ID
		INNER JOIN dbo.Fact_Detalle AS FD ON FE.FEnc_ID = FD.zFEnc_ID
		INNER JOIN dbo.Producto AS PR ON FD.zProd_ID = PR.Prod_ID
		ORDER BY PR.Prod_Precio DESC	
GO
/****** Object:  View [dbo].[View_ProductoCantidadFacturada]    Script Date: 13/11/2025 7:56:00 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE     VIEW [dbo].[View_ProductoCantidadFacturada]
AS

	SELECT DISTINCT
		PR.Prod_ID
		,PR.Prod_Descripcion
		,PR.Prod_Precio
		,PR.Prod_Costo
		,PR.Prod_UM
		,FD.Cantidad
	FROM dbo.Producto AS PR
		INNER JOIN (
			SELECT
				zProd_ID,
				SUM(FDet_Cantidad) AS Cantidad
			FROM dbo.Fact_Detalle 
			GROUP BY zProd_ID
		) AS FD ON PR.Prod_ID = FD.zProd_ID
GO
/****** Object:  View [dbo].[View_ProductoUtilidadFacturada]    Script Date: 13/11/2025 7:56:00 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE     VIEW [dbo].[View_ProductoUtilidadFacturada]
AS

	SELECT DISTINCT
		PR.Prod_ID
		,PR.Prod_Descripcion
		,PR.Prod_Precio
		,PR.Prod_Costo
		,PR.Prod_UM
		,((PR.Prod_Precio - PR.Prod_Costo) * FD.Cantidad) AS Utilidad
	FROM dbo.Producto AS PR
		INNER JOIN (
			SELECT
				zProd_ID,
				SUM(FDet_Cantidad) AS Cantidad
			FROM dbo.Fact_Detalle 
			GROUP BY zProd_ID
		) AS FD ON PR.Prod_ID = FD.zProd_ID
GO
/****** Object:  View [dbo].[View_ProductoMargenGanaciaFacturada]    Script Date: 13/11/2025 7:56:00 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE       VIEW [dbo].[View_ProductoMargenGanaciaFacturada]
AS

	SELECT DISTINCT
		PR.Prod_ID
		,PR.Prod_Descripcion
		,PR.Prod_Precio
		,PR.Prod_Costo
		,PR.Prod_UM
		,((((PR.Prod_Precio * FD.Cantidad)  - (PR.Prod_Costo * FD.Cantidad)) / (PR.Prod_Precio * FD.Cantidad)) * 100) AS MargenDeGanancia
	FROM dbo.Producto AS PR
		INNER JOIN (
			SELECT
				zProd_ID,
				SUM(FDet_Cantidad) AS Cantidad
			FROM dbo.Fact_Detalle 
			GROUP BY zProd_ID
		) AS FD ON PR.Prod_ID = FD.zProd_ID
GO
/****** Object:  Table [dbo].[Log]    Script Date: 13/11/2025 7:56:00 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Log](
	[IdLog] [bigint] IDENTITY(1,1) NOT NULL,
	[Error] [nvarchar](max) NOT NULL,
	[Message] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Log] PRIMARY KEY CLUSTERED 
(
	[IdLog] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Fact_Detalle] ON 
GO
INSERT [dbo].[Fact_Detalle] ([FDet_ID], [FDet_Linea], [FDet_Cantidad], [zProd_ID], [zFEnc_ID]) VALUES (1, N'1', 5, 1, 1)
GO
INSERT [dbo].[Fact_Detalle] ([FDet_ID], [FDet_Linea], [FDet_Cantidad], [zProd_ID], [zFEnc_ID]) VALUES (2, N'1', 1, 2, 1)
GO
INSERT [dbo].[Fact_Detalle] ([FDet_ID], [FDet_Linea], [FDet_Cantidad], [zProd_ID], [zFEnc_ID]) VALUES (3, N'1', 1, 1, 2)
GO
INSERT [dbo].[Fact_Detalle] ([FDet_ID], [FDet_Linea], [FDet_Cantidad], [zProd_ID], [zFEnc_ID]) VALUES (4, N'1', 10, 3, 3)
GO
INSERT [dbo].[Fact_Detalle] ([FDet_ID], [FDet_Linea], [FDet_Cantidad], [zProd_ID], [zFEnc_ID]) VALUES (6, N'1', 1, 1, 5)
GO
INSERT [dbo].[Fact_Detalle] ([FDet_ID], [FDet_Linea], [FDet_Cantidad], [zProd_ID], [zFEnc_ID]) VALUES (8, N'1', 1, 3, 5)
GO
SET IDENTITY_INSERT [dbo].[Fact_Detalle] OFF
GO
SET IDENTITY_INSERT [dbo].[Fact_Encabezado] ON 
GO
INSERT [dbo].[Fact_Encabezado] ([FEnc_ID], [FEnc_Numero], [FEnc_Fecha], [zPer_ID]) VALUES (1, 1, CAST(N'2025-11-11T00:00:00.000' AS DateTime), 1)
GO
INSERT [dbo].[Fact_Encabezado] ([FEnc_ID], [FEnc_Numero], [FEnc_Fecha], [zPer_ID]) VALUES (2, 2, CAST(N'2025-11-11T00:00:00.000' AS DateTime), 1)
GO
INSERT [dbo].[Fact_Encabezado] ([FEnc_ID], [FEnc_Numero], [FEnc_Fecha], [zPer_ID]) VALUES (3, 3, CAST(N'2025-11-11T00:00:00.000' AS DateTime), 3)
GO
INSERT [dbo].[Fact_Encabezado] ([FEnc_ID], [FEnc_Numero], [FEnc_Fecha], [zPer_ID]) VALUES (5, 1, CAST(N'2025-11-12T14:39:25.600' AS DateTime), 1)
GO
SET IDENTITY_INSERT [dbo].[Fact_Encabezado] OFF
GO
SET IDENTITY_INSERT [dbo].[Persona] ON 
GO
INSERT [dbo].[Persona] ([Per_ID], [Per_Nombre], [Per_Apellido], [Per_TipoDocumento], [Per_Documento]) VALUES (1, N'Pepito', N'Perez', 1, N'123456')
GO
INSERT [dbo].[Persona] ([Per_ID], [Per_Nombre], [Per_Apellido], [Per_TipoDocumento], [Per_Documento]) VALUES (2, N'Juan', N'Pardo', 1, N'654321')
GO
INSERT [dbo].[Persona] ([Per_ID], [Per_Nombre], [Per_Apellido], [Per_TipoDocumento], [Per_Documento]) VALUES (3, N'Jhon', N'Bolivar', 3, N'852741')
GO
INSERT [dbo].[Persona] ([Per_ID], [Per_Nombre], [Per_Apellido], [Per_TipoDocumento], [Per_Documento]) VALUES (4, N'Carlos', N'Contreras', 2, N'963852')
GO
SET IDENTITY_INSERT [dbo].[Persona] OFF
GO
SET IDENTITY_INSERT [dbo].[Producto] ON 
GO
INSERT [dbo].[Producto] ([Prod_ID], [Prod_Descripcion], [Prod_Precio], [Prod_Costo], [Prod_UM]) VALUES (1, N'transformadores monofásicos tipo poste', CAST(7000000.00 AS Decimal(18, 2)), CAST(5000000.00 AS Decimal(18, 2)), N'1')
GO
INSERT [dbo].[Producto] ([Prod_ID], [Prod_Descripcion], [Prod_Precio], [Prod_Costo], [Prod_UM]) VALUES (2, N'transformadores trifásicos tipo poste', CAST(9000000.00 AS Decimal(18, 2)), CAST(7000000.00 AS Decimal(18, 2)), N'1')
GO
INSERT [dbo].[Producto] ([Prod_ID], [Prod_Descripcion], [Prod_Precio], [Prod_Costo], [Prod_UM]) VALUES (3, N'transformadores trifásicos tipo subestación', CAST(10000000.00 AS Decimal(18, 2)), CAST(8000000.00 AS Decimal(18, 2)), N'1')
GO
SET IDENTITY_INSERT [dbo].[Producto] OFF
GO
SET IDENTITY_INSERT [dbo].[Tipo_Documento] ON 
GO
INSERT [dbo].[Tipo_Documento] ([TDoc_ID], [TDoc_Nombre], [TDoc_Activo]) VALUES (1, N'Cédula de ciudadanía', 1)
GO
INSERT [dbo].[Tipo_Documento] ([TDoc_ID], [TDoc_Nombre], [TDoc_Activo]) VALUES (2, N'NIT', 1)
GO
INSERT [dbo].[Tipo_Documento] ([TDoc_ID], [TDoc_Nombre], [TDoc_Activo]) VALUES (3, N'Cédula de extranjería', 1)
GO
SET IDENTITY_INSERT [dbo].[Tipo_Documento] OFF
GO
ALTER TABLE [dbo].[Fact_Detalle]  WITH CHECK ADD  CONSTRAINT [FK_Fact_Detalle_Fact_Encabezado] FOREIGN KEY([zFEnc_ID])
REFERENCES [dbo].[Fact_Encabezado] ([FEnc_ID])
GO
ALTER TABLE [dbo].[Fact_Detalle] CHECK CONSTRAINT [FK_Fact_Detalle_Fact_Encabezado]
GO
ALTER TABLE [dbo].[Fact_Detalle]  WITH CHECK ADD  CONSTRAINT [FK_Fact_Detalle_Producto] FOREIGN KEY([zProd_ID])
REFERENCES [dbo].[Producto] ([Prod_ID])
GO
ALTER TABLE [dbo].[Fact_Detalle] CHECK CONSTRAINT [FK_Fact_Detalle_Producto]
GO
ALTER TABLE [dbo].[Fact_Encabezado]  WITH CHECK ADD  CONSTRAINT [FK_Fact_Encabezado_Persona] FOREIGN KEY([zPer_ID])
REFERENCES [dbo].[Persona] ([Per_ID])
GO
ALTER TABLE [dbo].[Fact_Encabezado] CHECK CONSTRAINT [FK_Fact_Encabezado_Persona]
GO
ALTER TABLE [dbo].[Persona]  WITH CHECK ADD  CONSTRAINT [FK_PersonaTDoc] FOREIGN KEY([Per_TipoDocumento])
REFERENCES [dbo].[Tipo_Documento] ([TDoc_ID])
GO
ALTER TABLE [dbo].[Persona] CHECK CONSTRAINT [FK_PersonaTDoc]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1[50] 4[25] 3) )"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Persona"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 235
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Tipo_Documento"
            Begin Extent = 
               Top = 6
               Left = 273
               Bottom = 119
               Right = 443
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Fact_Encabezado"
            Begin Extent = 
               Top = 6
               Left = 481
               Bottom = 136
               Right = 651
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Fact_Detalle"
            Begin Extent = 
               Top = 6
               Left = 689
               Bottom = 136
               Right = 859
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_PersonaFacturado'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_PersonaFacturado'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1[50] 4[25] 3) )"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Persona"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 235
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Tipo_Documento"
            Begin Extent = 
               Top = 6
               Left = 273
               Bottom = 119
               Right = 443
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Fact_Encabezado"
            Begin Extent = 
               Top = 6
               Left = 481
               Bottom = 136
               Right = 651
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Fact_Detalle"
            Begin Extent = 
               Top = 6
               Left = 689
               Bottom = 136
               Right = 859
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_PersonaProductoCaro'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_PersonaProductoCaro'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1[50] 4[25] 3) )"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Persona"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 235
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Tipo_Documento"
            Begin Extent = 
               Top = 6
               Left = 273
               Bottom = 119
               Right = 443
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Fact_Encabezado"
            Begin Extent = 
               Top = 6
               Left = 481
               Bottom = 136
               Right = 651
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Fact_Detalle"
            Begin Extent = 
               Top = 6
               Left = 689
               Bottom = 136
               Right = 859
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_ProductoCantidadFacturada'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_ProductoCantidadFacturada'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1[50] 4[25] 3) )"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Persona"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 235
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Tipo_Documento"
            Begin Extent = 
               Top = 6
               Left = 273
               Bottom = 119
               Right = 443
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Fact_Encabezado"
            Begin Extent = 
               Top = 6
               Left = 481
               Bottom = 136
               Right = 651
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Fact_Detalle"
            Begin Extent = 
               Top = 6
               Left = 689
               Bottom = 136
               Right = 859
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_ProductoMargenGanaciaFacturada'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_ProductoMargenGanaciaFacturada'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1[50] 4[25] 3) )"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Persona"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 235
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Tipo_Documento"
            Begin Extent = 
               Top = 6
               Left = 273
               Bottom = 119
               Right = 443
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Fact_Encabezado"
            Begin Extent = 
               Top = 6
               Left = 481
               Bottom = 136
               Right = 651
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Fact_Detalle"
            Begin Extent = 
               Top = 6
               Left = 689
               Bottom = 136
               Right = 859
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_ProductoUtilidadFacturada'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_ProductoUtilidadFacturada'
GO
