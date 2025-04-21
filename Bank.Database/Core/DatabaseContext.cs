using Microsoft.EntityFrameworkCore;

namespace Bank.Database.Core;

public class DatabaseContext(DbContextOptions options) : DbContext(options)
{
    internal bool   BaseDispose         { get; set; } = true;
    internal Action DisposeBeforeAction { get; set; } = () => { };
    internal Action DisposeAfterAction  { get; set; } = () => { };

    public override void Dispose()
    {
        DisposeBeforeAction();

        if (BaseDispose)
            base.Dispose();

        GC.SuppressFinalize(this);

        DisposeAfterAction();
    }

    public override async ValueTask DisposeAsync()
    {
        DisposeBeforeAction();

        if (BaseDispose)
            await base.DisposeAsync();

        GC.SuppressFinalize(this);
        
        DisposeAfterAction();
    }
}
