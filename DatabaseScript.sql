USE [PracticeDB]
GO
/****** Object:  Table [dbo].[tblEmployees]    Script Date: 07-Jul-26 11:39:06 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblEmployees](
	[EmployeeID] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [varchar](100) NULL,
	[LastName] [varchar](100) NULL,
	[Email] [varchar](100) NULL,
	[Department] [varchar](100) NULL,
	[IsActive] [bit] NULL,
	[CreatedDate] [datetime] NULL,
 CONSTRAINT [PK_tblEmployees] PRIMARY KEY CLUSTERED 
(
	[EmployeeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[tblEmployees] ADD  CONSTRAINT [DF_tblEmployees_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[tblEmployees] ADD  CONSTRAINT [DF_tblEmployees_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
