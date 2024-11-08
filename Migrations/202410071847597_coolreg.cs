namespace GuardianAngels.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class coolreg : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Schoolregisters",
                c => new
                    {
                        registerId = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        studentid = c.Int(nullable: false),
                        studentName = c.String(),
                        studentImage = c.String(),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.registerId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Schoolregisters");
        }
    }
}
