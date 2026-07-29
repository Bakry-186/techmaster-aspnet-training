IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Instructors] (
    [InstructorId] int NOT NULL IDENTITY,
    [FullName] nvarchar(max) NOT NULL,
    [Email] nvarchar(450) NOT NULL,
    [Specialization] nvarchar(max) NOT NULL,
    [Bio] nvarchar(max) NULL,
    [IsActive] bit NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_Instructors] PRIMARY KEY ([InstructorId])
);
GO

CREATE TABLE [Students] (
    [StudentId] int NOT NULL IDENTITY,
    [FullName] nvarchar(max) NOT NULL,
    [Email] nvarchar(450) NOT NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [IsActive] bit NOT NULL,
    [IsDeleted] bit NOT NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_Students] PRIMARY KEY ([StudentId])
);
GO

CREATE TABLE [TrainingTracks] (
    [TrainingTrackId] int NOT NULL IDENTITY,
    [Title] nvarchar(max) NOT NULL,
    [Code] nvarchar(450) NOT NULL,
    [Description] nvarchar(max) NULL,
    [Level] int NOT NULL,
    [Capacity] int NOT NULL,
    [Fee] decimal(18,2) NOT NULL,
    [StartDate] datetime2 NOT NULL,
    [EndDate] datetime2 NOT NULL,
    [Status] int NOT NULL,
    [InstructorId] int NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [IsDeleted] bit NOT NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_TrainingTracks] PRIMARY KEY ([TrainingTrackId]),
    CONSTRAINT [FK_TrainingTracks_Instructors_InstructorId] FOREIGN KEY ([InstructorId]) REFERENCES [Instructors] ([InstructorId]) ON DELETE NO ACTION
);
GO

CREATE TABLE [Enrollments] (
    [EnrollmentId] int NOT NULL IDENTITY,
    [StudentId] int NOT NULL,
    [TrainingTrackId] int NOT NULL,
    [EnrollmentDate] datetime2 NOT NULL,
    [Status] int NOT NULL,
    [ProgressPercentage] decimal(5,2) NOT NULL,
    [FinalResult] nvarchar(max) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    CONSTRAINT [PK_Enrollments] PRIMARY KEY ([EnrollmentId]),
    CONSTRAINT [FK_Enrollments_Students_StudentId] FOREIGN KEY ([StudentId]) REFERENCES [Students] ([StudentId]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Enrollments_TrainingTracks_TrainingTrackId] FOREIGN KEY ([TrainingTrackId]) REFERENCES [TrainingTracks] ([TrainingTrackId]) ON DELETE NO ACTION
);
GO

CREATE TABLE [Payments] (
    [PaymentId] int NOT NULL IDENTITY,
    [EnrollmentId] int NOT NULL,
    [Amount] decimal(18,2) NOT NULL,
    [PaymentMethod] int NOT NULL,
    [PaymentDate] datetime2 NOT NULL,
    [PaymentStatus] int NOT NULL,
    [ReferenceNumber] nvarchar(max) NOT NULL,
    [Notes] nvarchar(max) NULL,
    CONSTRAINT [PK_Payments] PRIMARY KEY ([PaymentId]),
    CONSTRAINT [FK_Payments_Enrollments_EnrollmentId] FOREIGN KEY ([EnrollmentId]) REFERENCES [Enrollments] ([EnrollmentId]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_Enrollments_StudentId] ON [Enrollments] ([StudentId]);
GO

CREATE INDEX [IX_Enrollments_TrainingTrackId] ON [Enrollments] ([TrainingTrackId]);
GO

CREATE UNIQUE INDEX [IX_Instructors_Email] ON [Instructors] ([Email]);
GO

CREATE INDEX [IX_Payments_EnrollmentId] ON [Payments] ([EnrollmentId]);
GO

CREATE UNIQUE INDEX [IX_Students_Email] ON [Students] ([Email]);
GO

CREATE UNIQUE INDEX [IX_TrainingTracks_Code] ON [TrainingTracks] ([Code]);
GO

CREATE INDEX [IX_TrainingTracks_InstructorId] ON [TrainingTracks] ([InstructorId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260728151926_InitialTrainingCenterSchema', N'8.0.13');
GO

COMMIT;
GO

