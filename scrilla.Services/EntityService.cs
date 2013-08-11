using DapperExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scrilla.Services
{
	public class EntityService
	{
		protected IDatabase _db;

		public EntityService(IDatabase database)
		{
			_db = database;
		}

		protected ServiceResult<T> GetEntity<T>(int id) where T : class
		{
			var result = new ServiceResult<T>();

			result.Result = _db.Get<T>(id);
			if (result.Result == null)
			{
				result.AddError(ErrorType.NotFound, "{0} {1} not found", (typeof(T)).Name, id);
			}

			return result;
		}

		protected ServiceResult<IEnumerable<T>> GetAllEntity<T>() where T : class
		{
			var result = new ServiceResult<IEnumerable<T>>();
			result.Result = _db.GetList<T>();
			return result;
		}
	}
}
