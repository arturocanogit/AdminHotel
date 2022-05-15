namespace AdminHotelApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M140520221 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TiposHabitacionesFotos", "Contenido", c => c.Binary());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TiposHabitacionesFotos", "Contenido", c => c.Binary(maxLength: 2048));
        }
    }
}
