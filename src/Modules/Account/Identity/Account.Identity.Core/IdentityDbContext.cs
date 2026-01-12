using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Account.Identity.Core
{
    public abstract class EntityFrameworkDbContext<TDbContext> : DbContext
        where TDbContext : DbContext
    {
        protected EntityFrameworkDbContext(DbContextOptions<TDbContext> options)
            : base(options)
        {
        }

        protected abstract string Schema { get; }
    }
}
