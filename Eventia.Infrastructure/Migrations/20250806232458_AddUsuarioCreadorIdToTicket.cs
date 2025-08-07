using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eventia.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUsuarioCreadorIdToTicket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UsuarioCreadorId",
                table: "Tickets",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_UsuarioCreadorId",
                table: "Tickets",
                column: "UsuarioCreadorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Usuarios_UsuarioCreadorId",
                table: "Tickets",
                column: "UsuarioCreadorId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Usuarios_UsuarioCreadorId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_UsuarioCreadorId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "UsuarioCreadorId",
                table: "Tickets");
        }
    }
}
