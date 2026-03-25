using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Account.Core.Infrastructure.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "account");

            migrationBuilder.CreateTable(
                name: "Accounts",
                schema: "account",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EmailAddress = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Username = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    IsConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Identities",
                schema: "account",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IdentityProvider = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Identities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                schema: "account",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccountIdentities",
                schema: "account",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountIdentities", x => new { x.Id, x.AccountId });
                    table.ForeignKey(
                        name: "FK_AccountIdentities_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalSchema: "account",
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccountSecrets",
                schema: "account",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "bytea", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "bytea", nullable: true),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountSecrets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountSecrets_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalSchema: "account",
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccountTokens",
                schema: "account",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Token = table.Column<string>(type: "text", nullable: false),
                    TokenType = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Expiration = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountTokens_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalSchema: "account",
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccountRoles",
                schema: "account",
                columns: table => new
                {
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountRoles", x => new { x.AccountId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AccountRoles_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalSchema: "account",
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "account",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccountIdentityIdentity",
                schema: "account",
                columns: table => new
                {
                    IdentitiesId = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountIdentitiesId = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountIdentitiesAccountId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountIdentityIdentity", x => new { x.IdentitiesId, x.AccountIdentitiesId, x.AccountIdentitiesAccountId });
                    table.ForeignKey(
                        name: "FK_AccountIdentityIdentity_AccountIdentities_AccountIdentities~",
                        columns: x => new { x.AccountIdentitiesId, x.AccountIdentitiesAccountId },
                        principalSchema: "account",
                        principalTable: "AccountIdentities",
                        principalColumns: new[] { "Id", "AccountId" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountIdentityIdentity_Identities_IdentitiesId",
                        column: x => x.IdentitiesId,
                        principalSchema: "account",
                        principalTable: "Identities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountIdentities_AccountId",
                schema: "account",
                table: "AccountIdentities",
                column: "AccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccountIdentityIdentity_AccountIdentitiesId_AccountIdentiti~",
                schema: "account",
                table: "AccountIdentityIdentity",
                columns: new[] { "AccountIdentitiesId", "AccountIdentitiesAccountId" });

            migrationBuilder.CreateIndex(
                name: "IX_AccountRoles_RoleId",
                schema: "account",
                table: "AccountRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_EmailAddress",
                schema: "account",
                table: "Accounts",
                column: "EmailAddress",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccountSecrets_AccountId",
                schema: "account",
                table: "AccountSecrets",
                column: "AccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccountTokens_AccountId",
                schema: "account",
                table: "AccountTokens",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Name",
                schema: "account",
                table: "Roles",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountIdentityIdentity",
                schema: "account");

            migrationBuilder.DropTable(
                name: "AccountRoles",
                schema: "account");

            migrationBuilder.DropTable(
                name: "AccountSecrets",
                schema: "account");

            migrationBuilder.DropTable(
                name: "AccountTokens",
                schema: "account");

            migrationBuilder.DropTable(
                name: "AccountIdentities",
                schema: "account");

            migrationBuilder.DropTable(
                name: "Identities",
                schema: "account");

            migrationBuilder.DropTable(
                name: "Roles",
                schema: "account");

            migrationBuilder.DropTable(
                name: "Accounts",
                schema: "account");
        }
    }
}
