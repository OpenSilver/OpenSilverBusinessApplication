CREATE TABLE [aspnet_Applications]
(
    [ApplicationId] NVARCHAR(255) PRIMARY KEY NOT NULL,
    [ApplicationName] NVARCHAR(255) UNIQUE NOT NULL,
    [Description] NVARCHAR(MAX) NULL
);

CREATE TABLE [aspnet_Roles]
(
    [RoleId] NVARCHAR(255) PRIMARY KEY NOT NULL,
    [RoleName] NVARCHAR(255) NOT NULL,
    [LoweredRoleName] NVARCHAR(255) NOT NULL,
    [ApplicationId] NVARCHAR(255) NOT NULL
);

CREATE TABLE [aspnet_UsersInRoles]
(
    [UserId] NVARCHAR(255) NOT NULL,
    [RoleId] NVARCHAR(255) NOT NULL
);

CREATE TABLE [aspnet_Profile] (
    [UserId] NVARCHAR(255) UNIQUE NOT NULL,
    [LastUpdatedDate] DATETIME NOT NULL,
    [PropertyNames] NVARCHAR(MAX) NOT NULL,
    [PropertyValuesString] NVARCHAR(MAX) NOT NULL,
    [PropertyValuesBinary] VARBINARY(MAX) NOT NULL
);

CREATE TABLE [aspnet_Users] (
    [UserId] NVARCHAR(255) PRIMARY KEY NOT NULL,
    [Username] NVARCHAR(255) NOT NULL,
    [LoweredUsername] NVARCHAR(255) NOT NULL,
    [ApplicationId] NVARCHAR(255) NOT NULL,
    [Email] NVARCHAR(255) NULL,
    [LoweredEmail] NVARCHAR(255) NULL,
    [Comment] NVARCHAR(MAX) NULL,
    [Password] NVARCHAR(255) NOT NULL,
    [PasswordFormat] NVARCHAR(255) NOT NULL,
    [PasswordSalt] NVARCHAR(255) NOT NULL,
    [PasswordQuestion] NVARCHAR(MAX) NULL,
    [PasswordAnswer] NVARCHAR(MAX) NULL,
    [IsApproved] BIT NOT NULL,
    [IsAnonymous] BIT NOT NULL,
    [LastActivityDate] DATETIME NOT NULL,
    [LastLoginDate] DATETIME NOT NULL,
    [LastPasswordChangedDate] DATETIME NOT NULL,
    [CreateDate] DATETIME NOT NULL,
    [IsLockedOut] BIT NOT NULL,
    [LastLockoutDate] DATETIME NOT NULL,
    [FailedPasswordAttemptCount] INT NOT NULL,
    [FailedPasswordAttemptWindowStart] DATETIME NOT NULL,
    [FailedPasswordAnswerAttemptCount] INT NOT NULL,
    [FailedPasswordAnswerAttemptWindowStart] DATETIME NOT NULL
);

-- Create Indexes
CREATE UNIQUE INDEX idxUsers ON [aspnet_Users] ([LoweredUsername], [ApplicationId]);
CREATE INDEX idxUsersAppId ON [aspnet_Users] ([ApplicationId]);
CREATE UNIQUE INDEX idxRoles ON [aspnet_Roles] ([LoweredRoleName], [ApplicationId]);
CREATE UNIQUE INDEX idxUsersInRoles ON [aspnet_UsersInRoles] ([UserId], [RoleId]);
