namespace AdminHotelApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M13052022 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.TiposHabitacionesArchivos", newName: "TiposHabitacionesFotos");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.TiposHabitacionesFotos", newName: "TiposHabitacionesArchivos");
        }
    }
}
