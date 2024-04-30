﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace totp_Module.Migrations
{
    /// <inheritdoc />
    public partial class TOTP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TOTPSecretKey",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TOTPSecretKey",
                table: "Users");
        }
    }
}
