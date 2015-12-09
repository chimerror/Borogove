using System;
using System.Data.Entity;

namespace Borogove.DataAccess
{
    public static class DbSetExtensions
    {
        public static TEntity FindOrCreateEntity<TEntity>(this DbSet<TEntity> dbSet, params object[] keyValues) where TEntity : class
        {
            if (dbSet == null)
            {
                throw new ArgumentNullException(nameof(dbSet));
            }

            return dbSet.Find(keyValues) ?? dbSet.Add(dbSet.Create());
        }
    }
}
