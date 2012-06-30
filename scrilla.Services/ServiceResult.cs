using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace scrilla.Services
{
	public enum ErrorType
	{
		Generic,
		Security,
		NotFound,
	}

	public class ServiceResult<T> : ServiceResult
	{
		public T Result { get; set; }
	}

	public class ServiceResult
	{
		public bool HasErrors
		{
			get
			{
				return ErrorMessages != null && ErrorMessages.Count > 0;
			}
		}

		public List<KeyValuePair<ErrorType, string>> ErrorMessages { get; private set; }

		public void AddError(ErrorType key, string format, params object[] args)
		{
			AddError(key, String.Format(format, args));
		}

		public void AddError(ErrorType key, string error)
		{
			if (ErrorMessages == null)
				ErrorMessages = new List<KeyValuePair<ErrorType, string>>();

			ErrorMessages.Add(new KeyValuePair<ErrorType, string>(key, error));
		}

		public void AddErrors(IEnumerable<KeyValuePair<ErrorType, string>> errors)
		{
			foreach (var error in errors)
				AddError(error.Key, error.Value);
		}
	}
}
