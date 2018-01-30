namespace MemoryGameExample.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Highscore : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Highscores",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Time = c.Int(nullable: false),
                        Moves = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Highscores");
        }
    }
}
