using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace scrilla.Data.Repositories
{
	public interface IRepository<T> where T : class
	{
		void Add(T entity);
		void Update(T entity);
		void Delete(T entity);
		void Delete(Expression<Func<T, bool>> where);
		T GetById(long Id);
		T GetById(string Id);
		T Get(Expression<Func<T, bool>> where);
		IQueryable<T> GetAll();
		IQueryable<T> GetMany(Expression<Func<T, bool>> where);
	}
}
