namespace Search_Weather.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate6 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Weathers", "CityName", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Weathers", "CityName", c => c.String());
        }
    }
}
