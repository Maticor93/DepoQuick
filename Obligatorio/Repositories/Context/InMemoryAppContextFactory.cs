using Microsoft.EntityFrameworkCore;

namespace Repositories;



public class InMemoryEFContextFactory
{
    public EFContext CreateDbContext()
    {
        DbContextOptionsBuilder<EFContext> optionsBuilder = new DbContextOptionsBuilder<EFContext>();
        optionsBuilder.UseInMemoryDatabase("TestDB");

        return new EFContext(optionsBuilder.Options);
    }
}