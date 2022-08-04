namespace AdminHotelApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M003 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.TiposHabitacionesFotos", "TipoContenido");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TiposHabitacionesFotos", "TipoContenido", c => c.String(maxLength: 32));
        }
    }
}
