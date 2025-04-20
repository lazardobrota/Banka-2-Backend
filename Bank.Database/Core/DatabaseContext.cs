using Microsoft.EntityFrameworkCore;

namespace Bank.Database.Core;

public class DatabaseContext(DbContextOptions options) : DbContext(options)
{
    internal bool   BaseDispose   { get; set; } = true;
    internal Action DisposeAction { get; set; } = () => { };

    public override void Dispose()
    {
        DisposeAction();

        if (BaseDispose)
            base.Dispose();

        GC.SuppressFinalize(this);
    }

    public override async ValueTask DisposeAsync()
    {
        DisposeAction();

        if (BaseDispose)
            await base.DisposeAsync();

        GC.SuppressFinalize(this);
    }
}
