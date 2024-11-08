namespace GuardianAngels.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class usernum : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "NameSurname", c => c.String());
            AddColumn("dbo.AspNetUsers", "number", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "number");
            DropColumn("dbo.AspNetUsers", "NameSurname");
        }
    }
}
