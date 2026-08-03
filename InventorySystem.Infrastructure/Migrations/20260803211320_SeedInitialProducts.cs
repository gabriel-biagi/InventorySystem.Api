using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventorySystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                INSERT INTO Products (Name, UnitType) VALUES 
                ('Rolamento de Esfera 6204-2RS SKF', 0),
                ('Parafuso Sextavado M12x50mm Inox 304', 0),
                ('Chapa de Aco Carbono ASTM A36 3/16 (4.75mm)', 0),
                ('Correia em V Emborrachada A-42', 0),
                ('Veda Rosca Teflon 18mm x 50m High Performance', 0),
                ('Eletrodo Revestido AWS E6013 2.50mm', 0),
                ('Graxa Litio EP-2 para Altas Cargas 1kg', 2),
                ('Valvula Esfera Tripartida Passagem Full 1/2 NPT', 0);
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DELETE FROM Products WHERE Name IN (
                    'Rolamento de Esfera 6204-2RS SKF',
                    'Parafuso Sextavado M12x50mm Inox 304',
                    'Chapa de Aco Carbono ASTM A36 3/16 (4.75mm)',
                    'Correia em V Emborrachada A-42',
                    'Veda Rosca Teflon 18mm x 50m High Performance',
                    'Eletrodo Revestido AWS E6013 2.50mm',
                    'Graxa Litio EP-2 para Altas Cargas 1kg',
                    'Valvula Esfera Tripartida Passagem Full 1/2 NPT'
                );
            ");
        }
    }
}
