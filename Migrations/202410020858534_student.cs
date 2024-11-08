namespace GuardianAngels.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class student : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        StudentId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Surname = c.String(),
                        Age = c.Int(nullable: false),
                        StudentImage = c.String(),
                        SchoolId = c.Int(nullable: false),
                        ContactNumber = c.String(),
                        ParentEmail = c.String(),
                        QR = c.String(),
                        Status = c.String(),
                        vId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.StudentId)
                .ForeignKey("dbo.Schools", t => t.SchoolId, cascadeDelete: true)
                .Index(t => t.SchoolId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Students", "SchoolId", "dbo.Schools");
            DropIndex("dbo.Students", new[] { "SchoolId" });
            DropTable("dbo.Students");
        }
    }
}
