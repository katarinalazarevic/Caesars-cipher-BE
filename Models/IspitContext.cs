namespace Models;

public class IspitContext : DbContext
{
    public DbSet<Alphabet> Alphabets { get; set; }
    public IspitContext(DbContextOptions options) : base(options)
    {
        
    }
}
