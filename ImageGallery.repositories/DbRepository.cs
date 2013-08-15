// ****************************************************************
// <copyright file="DbRepository.cs" company="Telerik Academy">
// Copyright (c) 2013 Telerik Academy. All rights reserved.
// </copyright>
// ****************************************************************

namespace ImageGallery.Repositories
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Data;

    public class DbRepository<T> : IRepository<T>
        where T : class
    {
        private DbContext dbContext;
        private DbSet<T> entitySet;

        public DbRepository(DbContext dbContext)
        {
            this.dbContext = dbContext;
            this.entitySet = this.dbContext.Set<T>();
        }

        public T Add(T item)
        {
            this.entitySet.Add(item);
            this.dbContext.SaveChanges();
            return item;
        }

        public T Update(int id, T item)
        {
            dbContext.Entry(item).State = EntityState.Modified;
            this.dbContext.SaveChanges();

            return item;
        }

        public void Delete(int id)
        {
            var item = this.entitySet.Find(id);
            if (item == null)
            {
                throw new ArgumentException("Item does not exists!");
            }

            this.Delete(item);
        }

        public void Delete(T item)
        {
            if (item == null)
            {
                throw new ArgumentException("Item does not exists!");
            }

            this.entitySet.Remove(item);
            this.dbContext.SaveChanges();
        }

        public T Get(int id)
        {
            return this.entitySet.Find(id);
        }

        public IQueryable<T> All()
        {
            return this.entitySet;
        }

        public IQueryable<T> Find(Expression<Func<T, int, bool>> predicate)
        {
            throw new NotImplementedException();
        }
    }
}
