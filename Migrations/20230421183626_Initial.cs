using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BrewComp.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "BrewComp.NET");

            migrationBuilder.CreateTable(
                name: "Clubs",
                schema: "BrewComp.NET",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Abbreviation = table.Column<string>(type: "text", nullable: true),
                    Homepage = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clubs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Competitions",
                schema: "BrewComp.NET",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CompetitionDates = table.Column<string>(type: "text", nullable: false),
                    DropOffDates = table.Column<string>(type: "text", nullable: false),
                    ShippingDates = table.Column<string>(type: "text", nullable: false),
                    RegistrationDates = table.Column<string>(type: "text", nullable: false),
                    EntryRegistrationDates = table.Column<string>(type: "text", nullable: false),
                    CategoryIds = table.Column<List<string>>(type: "text[]", nullable: false),
                    DropOffAddresses = table.Column<string>(type: "text", nullable: false),
                    CompetitionCoordinators = table.Column<List<string>>(type: "text[]", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Competitions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                schema: "BrewComp.NET",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "BrewComp.NET",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: true),
                    ClubId = table.Column<Guid>(type: "uuid", nullable: true),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Clubs_ClubId",
                        column: x => x.ClubId,
                        principalSchema: "BrewComp.NET",
                        principalTable: "Clubs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RoleClaims",
                schema: "BrewComp.NET",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleClaims_Role_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "BrewComp.NET",
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BrewCompUserCompetition",
                schema: "BrewComp.NET",
                columns: table => new
                {
                    CompetitionsId = table.Column<Guid>(type: "uuid", nullable: false),
                    EntrantsId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrewCompUserCompetition", x => new { x.CompetitionsId, x.EntrantsId });
                    table.ForeignKey(
                        name: "FK_BrewCompUserCompetition_Competitions_CompetitionsId",
                        column: x => x.CompetitionsId,
                        principalSchema: "BrewComp.NET",
                        principalTable: "Competitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BrewCompUserCompetition_Users_EntrantsId",
                        column: x => x.EntrantsId,
                        principalSchema: "BrewComp.NET",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Entries",
                schema: "BrewComp.NET",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CompetitionId = table.Column<Guid>(type: "uuid", nullable: false),
                    EntryId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    BrewerId = table.Column<string>(type: "text", nullable: false),
                    CoBrewer = table.Column<string>(type: "text", nullable: true),
                    Style = table.Column<string>(type: "text", nullable: false),
                    BrewersSpecs = table.Column<string>(type: "text", nullable: true),
                    Allergens = table.Column<string>(type: "text", nullable: true),
                    CompositeScore = table.Column<float>(type: "real", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Entries_Competitions_CompetitionId",
                        column: x => x.CompetitionId,
                        principalSchema: "BrewComp.NET",
                        principalTable: "Competitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Entries_Users_BrewerId",
                        column: x => x.BrewerId,
                        principalSchema: "BrewComp.NET",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
                schema: "BrewComp.NET",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaims_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "BrewComp.NET",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                schema: "BrewComp.NET",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_UserLogins_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "BrewComp.NET",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                schema: "BrewComp.NET",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Role_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "BrewComp.NET",
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "BrewComp.NET",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                schema: "BrewComp.NET",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_UserTokens_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "BrewComp.NET",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BrewCompUserCompetition_EntrantsId",
                schema: "BrewComp.NET",
                table: "BrewCompUserCompetition",
                column: "EntrantsId");

            migrationBuilder.CreateIndex(
                name: "IX_Competitions_Id",
                schema: "BrewComp.NET",
                table: "Competitions",
                column: "Id")
                .Annotation("Npgsql:IndexInclude", new[] { "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_Entries_BrewerId",
                schema: "BrewComp.NET",
                table: "Entries",
                column: "BrewerId");

            migrationBuilder.CreateIndex(
                name: "IX_Entries_CompetitionId",
                schema: "BrewComp.NET",
                table: "Entries",
                column: "CompetitionId");

            migrationBuilder.CreateIndex(
                name: "IX_Entries_Id",
                schema: "BrewComp.NET",
                table: "Entries",
                column: "Id")
                .Annotation("Npgsql:IndexInclude", new[] { "EntryId" });

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                schema: "BrewComp.NET",
                table: "Role",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_RoleId",
                schema: "BrewComp.NET",
                table: "RoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_UserId",
                schema: "BrewComp.NET",
                table: "UserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_UserId",
                schema: "BrewComp.NET",
                table: "UserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                schema: "BrewComp.NET",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                schema: "BrewComp.NET",
                table: "Users",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ClubId",
                schema: "BrewComp.NET",
                table: "Users",
                column: "ClubId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Id",
                schema: "BrewComp.NET",
                table: "Users",
                column: "Id")
                .Annotation("Npgsql:IndexInclude", new[] { "NormalizedUserName", "LastName", "FirstName" });

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                schema: "BrewComp.NET",
                table: "Users",
                column: "NormalizedUserName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BrewCompUserCompetition",
                schema: "BrewComp.NET");

            migrationBuilder.DropTable(
                name: "Entries",
                schema: "BrewComp.NET");

            migrationBuilder.DropTable(
                name: "RoleClaims",
                schema: "BrewComp.NET");

            migrationBuilder.DropTable(
                name: "UserClaims",
                schema: "BrewComp.NET");

            migrationBuilder.DropTable(
                name: "UserLogins",
                schema: "BrewComp.NET");

            migrationBuilder.DropTable(
                name: "UserRoles",
                schema: "BrewComp.NET");

            migrationBuilder.DropTable(
                name: "UserTokens",
                schema: "BrewComp.NET");

            migrationBuilder.DropTable(
                name: "Competitions",
                schema: "BrewComp.NET");

            migrationBuilder.DropTable(
                name: "Role",
                schema: "BrewComp.NET");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "BrewComp.NET");

            migrationBuilder.DropTable(
                name: "Clubs",
                schema: "BrewComp.NET");
        }
    }
}
