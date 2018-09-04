namespace WebMVCDemo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Student : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        StuId = c.String(nullable: false, maxLength: 128),
                        Name = c.String(maxLength: 20),
                        Sex = c.String(maxLength: 2),
                        Age = c.Int(nullable: false),
                        Class = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.StuId);
            
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Students");
        }
    }
}
