using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Data;
using System.Linq.Expressions;
using scrilla.Data.Repositories;

namespace scrilla.Data.EF
{
	public abstract class RepositoryBase<T> : IRepository<T> where T : class
	{
		private scrillaContext dataContext;
		private readonly IDbSet<T> dbset;

		protected RepositoryBase(IDatabaseFactory databaseFactory)
		{
			DatabaseFactory = databaseFactory;
			dbset = DataContext.Set<T>();
		}

		protected IDatabaseFactory DatabaseFactory { get; private set; }

		protected scrillaContext DataContext
		{
			get { return dataContext ?? (dataContext = DatabaseFactory.Get()); }
		}
		public virtual void Add(T entity)
		{
			dbset.Add(entity);
		}
		public virtual void Update(T entity)
		{
			dbset.Attach(entity);
			dataContext.Entry(entity).State = EntityState.Modified;
		}
		public virtual void Delete(T entity)
		{
			dbset.Remove(entity);
		}
		public virtual void Delete(Expression<Func<T, bool>> where)
		{
			IEnumerable<T> objects = dbset.Where<T>(where).AsEnumerable();
			foreach (T obj in objects)
				dbset.Remove(obj);
		}
		public virtual T GetById(long id)
		{
			return dbset.Find(id);
		}
		public virtual T GetById(string id)
		{
			return dbset.Find(id);
		}
		public virtual IQueryable<T> GetAll()
		{
			return dbset.AsQueryable<T>();
		}
		public virtual IQueryable<T> GetMany(Expression<Func<T, bool>> where)
		{
			return dbset.Where(where).AsQueryable<T>();
		}
		public T Get(Expression<Func<T, bool>> where)
		{
			return dbset.Where(where).FirstOrDefault<T>();
		}
	}
}
