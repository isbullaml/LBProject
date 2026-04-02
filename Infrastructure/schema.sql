CREATE TABLE Breaches
(
    [Id] NVARCHAR(200) NOT NULL PRIMARY KEY,  -- Using Name as ID
    [Title] NVARCHAR(200) NULL,
    [Domain] NVARCHAR(200) NULL,
    [BreachDate] DATETIME2 NOT NULL,
    [AddedDate] DATETIME2 NOT NULL,
    [ModifiedDate] DATETIME2 NOT NULL,
    [PwnCount] BIGINT NOT NULL,
    [Description] NVARCHAR(MAX) NULL,
    [LogoPath] NVARCHAR(500) NULL,
    [Attribution] NVARCHAR(200) NULL,
    [DisclosureUrl] NVARCHAR(500) NULL,
    [DataClasses] NVARCHAR(MAX) NULL, -- store JSON array as string
    [IsVerified] BIT NOT NULL DEFAULT 0,
    [IsFabricated] BIT NOT NULL DEFAULT 0,
    [IsSensitive] BIT NOT NULL DEFAULT 0,
    [IsRetired] BIT NOT NULL DEFAULT 0,
    [IsSpamList] BIT NOT NULL DEFAULT 0,
    [IsMalware] BIT NOT NULL DEFAULT 0,
    [IsSubscriptionFree] BIT NOT NULL DEFAULT 0,
    [IsStealerLog] BIT NOT NULL DEFAULT 0
);