USE [ServersAndHosts]
GO
/****** Object:  Table [dbo].[backup]    Script Date: 28.10.2024 18:38:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[backup](
	[id] [int] NOT NULL,
	[id_host] [int] NOT NULL,
	[id_server] [int] NOT NULL,
	[size_kb] [int] NOT NULL,
	[when_was] [datetime] NOT NULL,
 CONSTRAINT [PK_backup] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[component]    Script Date: 28.10.2024 18:38:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[component](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[id_component_type] [int] NOT NULL,
	[name] [varchar](64) NOT NULL,
	[mhz] [int] NULL,
	[memory] [int] NULL,
	[cores] [int] NULL,
 CONSTRAINT [PK_component] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[component_type]    Script Date: 28.10.2024 18:38:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[component_type](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[typename] [varchar](32) NOT NULL,
 CONSTRAINT [PK_component_type] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[host]    Script Date: 28.10.2024 18:38:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[host](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[id_server] [int] NOT NULL,
	[ram_mb] [int] NOT NULL,
	[cpu_cores] [int] NOT NULL,
	[memory_kb_limit] [int] NOT NULL,
	[memory_kb_took] [int] NOT NULL,
	[hostname] [varchar](64) NOT NULL,
	[host_addr] [varchar](64) NOT NULL,
	[comment] [varchar](128) NOT NULL,
 CONSTRAINT [PK_host] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[server]    Script Date: 28.10.2024 18:38:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[server](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[address] [varchar](24) NOT NULL,
	[name_in_network] [varchar](32) NOT NULL,
	[cpu_frequency_mhz] [int] NOT NULL,
	[cores_total] [int] NOT NULL,
	[ram_total_mb] [int] NOT NULL,
	[ram_free_mb] [int] NOT NULL,
	[memory_total_kb] [int] NOT NULL,
	[memory_free_kb] [int] NOT NULL,
	[cores_free] [int] NOT NULL,
 CONSTRAINT [PK_server_1] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[server_component]    Script Date: 28.10.2024 18:38:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[server_component](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[id_server] [int] NOT NULL,
	[id_component] [int] NOT NULL,
 CONSTRAINT [PK_server_component] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[component] ON 

INSERT [dbo].[component] ([id], [id_component_type], [name], [mhz], [memory], [cores]) VALUES (15, 5, N'Kingston RealMan', NULL, 1000000, NULL)
INSERT [dbo].[component] ([id], [id_component_type], [name], [mhz], [memory], [cores]) VALUES (17, 4, N'Samsung 1292', 1600, 4096, NULL)
INSERT [dbo].[component] ([id], [id_component_type], [name], [mhz], [memory], [cores]) VALUES (18, 1, N'i4-1489K', 8312, NULL, 5)
INSERT [dbo].[component] ([id], [id_component_type], [name], [mhz], [memory], [cores]) VALUES (19, 4, N'Samsung micro', 1222, 1024, NULL)
SET IDENTITY_INSERT [dbo].[component] OFF
GO
SET IDENTITY_INSERT [dbo].[component_type] ON 

INSERT [dbo].[component_type] ([id], [typename]) VALUES (1, N'CPU')
INSERT [dbo].[component_type] ([id], [typename]) VALUES (12, N'HDD')
INSERT [dbo].[component_type] ([id], [typename]) VALUES (4, N'RAM')
INSERT [dbo].[component_type] ([id], [typename]) VALUES (5, N'SSD')
INSERT [dbo].[component_type] ([id], [typename]) VALUES (0, N'TestCompType')
SET IDENTITY_INSERT [dbo].[component_type] OFF
GO
SET IDENTITY_INSERT [dbo].[host] ON 

INSERT [dbo].[host] ([id], [id_server], [ram_mb], [cpu_cores], [memory_kb_limit], [memory_kb_took], [hostname], [host_addr], [comment]) VALUES (9, 10, 4096, 3, 25000000, 15000000, N'minecraft_server', N'myhosts.good_server.ru/4258:25565', N'Наш личный по майнкрафтику')
INSERT [dbo].[host] ([id], [id_server], [ram_mb], [cpu_cores], [memory_kb_limit], [memory_kb_took], [hostname], [host_addr], [comment]) VALUES (10, 11, 1024, 1, 1024024, 0, N'site', N'billy_jinn.org', N'Тестовый сайт')
SET IDENTITY_INSERT [dbo].[host] OFF
GO
SET IDENTITY_INSERT [dbo].[server] ON 

INSERT [dbo].[server] ([id], [address], [name_in_network], [cpu_frequency_mhz], [cores_total], [ram_total_mb], [ram_free_mb], [memory_total_kb], [memory_free_kb], [cores_free]) VALUES (10, N'192.168.3.80', N'servak', 8312, 5, 4096, 0, 2048000000, 2023000000, 2)
INSERT [dbo].[server] ([id], [address], [name_in_network], [cpu_frequency_mhz], [cores_total], [ram_total_mb], [ram_free_mb], [memory_total_kb], [memory_free_kb], [cores_free]) VALUES (11, N'192.168.3.74', N'comp', 8312, 5, 16384, 15360, 1024000000, 1022975976, 4)
SET IDENTITY_INSERT [dbo].[server] OFF
GO
SET IDENTITY_INSERT [dbo].[server_component] ON 

INSERT [dbo].[server_component] ([id], [id_server], [id_component]) VALUES (179, 10, 18)
INSERT [dbo].[server_component] ([id], [id_server], [id_component]) VALUES (180, 10, 17)
INSERT [dbo].[server_component] ([id], [id_server], [id_component]) VALUES (181, 10, 15)
INSERT [dbo].[server_component] ([id], [id_server], [id_component]) VALUES (182, 10, 15)
INSERT [dbo].[server_component] ([id], [id_server], [id_component]) VALUES (183, 11, 18)
INSERT [dbo].[server_component] ([id], [id_server], [id_component]) VALUES (184, 11, 15)
INSERT [dbo].[server_component] ([id], [id_server], [id_component]) VALUES (185, 11, 17)
INSERT [dbo].[server_component] ([id], [id_server], [id_component]) VALUES (186, 11, 17)
INSERT [dbo].[server_component] ([id], [id_server], [id_component]) VALUES (187, 11, 17)
INSERT [dbo].[server_component] ([id], [id_server], [id_component]) VALUES (188, 11, 17)
SET IDENTITY_INSERT [dbo].[server_component] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UK_component]    Script Date: 28.10.2024 18:38:59 ******/
ALTER TABLE [dbo].[component] ADD  CONSTRAINT [UK_component] UNIQUE NONCLUSTERED 
(
	[name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UK_component_type]    Script Date: 28.10.2024 18:38:59 ******/
ALTER TABLE [dbo].[component_type] ADD  CONSTRAINT [UK_component_type] UNIQUE NONCLUSTERED 
(
	[typename] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[backup] ADD  CONSTRAINT [DF_backup_when_was]  DEFAULT (getdate()) FOR [when_was]
GO
ALTER TABLE [dbo].[host] ADD  CONSTRAINT [DF_host_comment]  DEFAULT ('empty') FOR [comment]
GO
ALTER TABLE [dbo].[server] ADD  CONSTRAINT [DF_server_cpu_frequency_mhz]  DEFAULT ((0)) FOR [cpu_frequency_mhz]
GO
ALTER TABLE [dbo].[server] ADD  CONSTRAINT [DF_server_cores_total]  DEFAULT ((0)) FOR [cores_total]
GO
ALTER TABLE [dbo].[backup]  WITH CHECK ADD  CONSTRAINT [FK_backup_host] FOREIGN KEY([id_host])
REFERENCES [dbo].[host] ([id])
GO
ALTER TABLE [dbo].[backup] CHECK CONSTRAINT [FK_backup_host]
GO
ALTER TABLE [dbo].[backup]  WITH CHECK ADD  CONSTRAINT [FK_backup_server1] FOREIGN KEY([id_server])
REFERENCES [dbo].[server] ([id])
GO
ALTER TABLE [dbo].[backup] CHECK CONSTRAINT [FK_backup_server1]
GO
ALTER TABLE [dbo].[component]  WITH CHECK ADD  CONSTRAINT [FK_component_component_type] FOREIGN KEY([id_component_type])
REFERENCES [dbo].[component_type] ([id])
GO
ALTER TABLE [dbo].[component] CHECK CONSTRAINT [FK_component_component_type]
GO
ALTER TABLE [dbo].[host]  WITH CHECK ADD  CONSTRAINT [FK_host_server1] FOREIGN KEY([id_server])
REFERENCES [dbo].[server] ([id])
GO
ALTER TABLE [dbo].[host] CHECK CONSTRAINT [FK_host_server1]
GO
ALTER TABLE [dbo].[server_component]  WITH CHECK ADD  CONSTRAINT [FK_server_component_component1] FOREIGN KEY([id_component])
REFERENCES [dbo].[component] ([id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[server_component] CHECK CONSTRAINT [FK_server_component_component1]
GO
ALTER TABLE [dbo].[server_component]  WITH CHECK ADD  CONSTRAINT [FK_server_component_server] FOREIGN KEY([id_server])
REFERENCES [dbo].[server] ([id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[server_component] CHECK CONSTRAINT [FK_server_component_server]
GO
