using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Migrations
{
    public partial class AddedTags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CTags",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CTags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ETags",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ETags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContactCTag",
                columns: table => new
                {
                    ContactId = table.Column<Guid>(nullable: false),
                    TagId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactCTag", x => new { x.ContactId, x.TagId });
                    table.ForeignKey(
                        name: "FK_ContactCTag_Contacts_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContactCTag_CTags_TagId",
                        column: x => x.TagId,
                        principalTable: "CTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventETag",
                columns: table => new
                {
                    EventId = table.Column<Guid>(nullable: false),
                    TagId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventETag", x => new { x.EventId, x.TagId });
                    table.ForeignKey(
                        name: "FK_EventETag_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventETag_ETags_TagId",
                        column: x => x.TagId,
                        principalTable: "ETags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContactCTag_TagId",
                table: "ContactCTag",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_EventETag_TagId",
                table: "EventETag",
                column: "TagId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContactCTag");

            migrationBuilder.DropTable(
                name: "EventETag");

            migrationBuilder.DropTable(
                name: "CTags");

            migrationBuilder.DropTable(
                name: "ETags");
        }
    }
}
