namespace AdminHotelApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M29052022_1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clientes", "Email", c => c.String(maxLength: 32));
            DropColumn("dbo.Clientes", "Rfc");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Clientes", "Rfc", c => c.String(maxLength: 32));
            DropColumn("dbo.Clientes", "Email");
        }
    }
}
