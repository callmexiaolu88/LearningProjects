using Microsoft.EntityFrameworkCore;

public class MyDbContext:DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options):base(options)
    {
        
    }

    public DbSet<AddressBook> ABS {get;set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder){
        modelBuilder.Entity<AddressBook>(e=>{
            e.HasKey(i=>i.Name);
        });
    }

}