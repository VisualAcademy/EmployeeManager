CREATE TABLE [dbo].[EmployeesHistories] (
    [Id]         BIGINT         IDENTITY (1, 1) NOT NULL,
    [EmployeeId] BIGINT         NOT NULL,
    [FirstName]  NVARCHAR (MAX) NULL,
    [LastName]   NVARCHAR (MAX) NULL,
    [MiddleName] NVARCHAR (MAX) NULL,
    [DOB]        NVARCHAR (MAX) NULL,
    [SSN]        NVARCHAR (MAX) NULL,
    [Email]      NVARCHAR (MAX) NULL,
    [Address]    NVARCHAR (MAX) NULL,
    [City]       NVARCHAR (MAX) NULL,
    [State]      NVARCHAR (MAX) NULL,
    [PostalCode] NVARCHAR (MAX) NULL,
    [Gender]     NVARCHAR (MAX) NULL,
    [Active]     BIT            NOT NULL,
    [CreatedAt]  DATETIME2 (7)  NOT NULL,
    [Remarks]    NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_EmployeesHistories] PRIMARY KEY CLUSTERED ([Id] ASC)
);

