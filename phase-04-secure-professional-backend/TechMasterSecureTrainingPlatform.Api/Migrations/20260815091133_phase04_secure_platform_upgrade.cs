using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainingCenter.Api.Migrations
{
    /// <inheritdoc />
    public partial class phase04_secure_platform_upgrade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                IF COL_LENGTH('Students', 'UserId') IS NULL
                    ALTER TABLE [Students] ADD [UserId] int NULL;
                """);

            migrationBuilder.Sql("""
                IF COL_LENGTH('Instructors', 'UserId') IS NULL
                    ALTER TABLE [Instructors] ADD [UserId] int NULL;
                """);

            migrationBuilder.Sql("""
                IF OBJECT_ID(N'[ActivityLogs]', N'U') IS NULL
                BEGIN
                    CREATE TABLE [ActivityLogs] (
                        [ActivityLogId] int NOT NULL IDENTITY,
                        [UserId] int NULL,
                        [UserEmail] nvarchar(max) NOT NULL,
                        [UserRole] nvarchar(max) NOT NULL,
                        [Action] nvarchar(max) NOT NULL,
                        [EntityType] nvarchar(450) NOT NULL,
                        [EntityId] int NULL,
                        [Details] nvarchar(max) NULL,
                        [CreatedAt] datetime2 NOT NULL,
                        CONSTRAINT [PK_ActivityLogs] PRIMARY KEY ([ActivityLogId])
                    );
                    CREATE INDEX [IX_ActivityLogs_CreatedAt] ON [ActivityLogs] ([CreatedAt]);
                    CREATE INDEX [IX_ActivityLogs_EntityType_EntityId] ON [ActivityLogs] ([EntityType], [EntityId]);
                END
                """);

            migrationBuilder.Sql("""
                IF OBJECT_ID(N'[ApplicationUsers]', N'U') IS NULL
                BEGIN
                    CREATE TABLE [ApplicationUsers] (
                        [Id] int NOT NULL IDENTITY,
                        [FullName] nvarchar(max) NOT NULL,
                        [Email] nvarchar(450) NOT NULL,
                        [PasswordHash] nvarchar(max) NOT NULL,
                        [Role] nvarchar(450) NOT NULL,
                        [IsActive] bit NOT NULL,
                        [CreatedAt] datetime2 NOT NULL,
                        [UpdatedAt] datetime2 NULL,
                        [LastLoginAt] datetime2 NULL,
                        [StudentId] int NULL,
                        [InstructorId] int NULL,
                        CONSTRAINT [PK_ApplicationUsers] PRIMARY KEY ([Id]),
                        CONSTRAINT [FK_ApplicationUsers_Instructors_InstructorId] FOREIGN KEY ([InstructorId]) REFERENCES [Instructors] ([InstructorId]) ON DELETE SET NULL,
                        CONSTRAINT [FK_ApplicationUsers_Students_StudentId] FOREIGN KEY ([StudentId]) REFERENCES [Students] ([StudentId]) ON DELETE SET NULL
                    );
                    CREATE UNIQUE INDEX [IX_ApplicationUsers_Email] ON [ApplicationUsers] ([Email]);
                    CREATE INDEX [IX_ApplicationUsers_InstructorId] ON [ApplicationUsers] ([InstructorId]);
                    CREATE INDEX [IX_ApplicationUsers_Role] ON [ApplicationUsers] ([Role]);
                    CREATE INDEX [IX_ApplicationUsers_StudentId] ON [ApplicationUsers] ([StudentId]);
                END
                """);

            migrationBuilder.Sql("""
                IF OBJECT_ID(N'[TrackSessions]', N'U') IS NULL
                BEGIN
                    CREATE TABLE [TrackSessions] (
                        [TrackSessionId] int NOT NULL IDENTITY,
                        [TrainingTrackId] int NOT NULL,
                        [Title] nvarchar(max) NOT NULL,
                        [Description] nvarchar(max) NULL,
                        [SessionDate] datetime2 NOT NULL,
                        [DurationMinutes] int NOT NULL,
                        [CreatedAt] datetime2 NOT NULL,
                        [UpdatedAt] datetime2 NULL,
                        CONSTRAINT [PK_TrackSessions] PRIMARY KEY ([TrackSessionId]),
                        CONSTRAINT [FK_TrackSessions_TrainingTracks_TrainingTrackId] FOREIGN KEY ([TrainingTrackId]) REFERENCES [TrainingTracks] ([TrainingTrackId]) ON DELETE CASCADE
                    );
                    CREATE INDEX [IX_TrackSessions_TrainingTrackId] ON [TrackSessions] ([TrainingTrackId]);
                END
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "ActivityLogs");
            migrationBuilder.DropTable(name: "ApplicationUsers");
            migrationBuilder.DropTable(name: "TrackSessions");

            migrationBuilder.DropColumn(name: "UserId", table: "Students");
            migrationBuilder.DropColumn(name: "UserId", table: "Instructors");
        }
    }
}
