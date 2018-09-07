using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace QualityCaps.Data.Migrations
{
    public partial class Enabled : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
               name: "Enabled",
               table: "AspNetUsers");

        }
    }
}
