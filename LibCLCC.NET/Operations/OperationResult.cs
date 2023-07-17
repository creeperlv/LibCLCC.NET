using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibCLCC.NET.Operations
{
	/// <summary>
	/// Base class for errors and warnings
	/// </summary>
	public class Noticeable
	{

	}
	/// <summary>
	/// Indicate an error.
	/// </summary>
	public class Error : Noticeable { }
	/// <summary>
	/// Indicate a warning.
	/// </summary>
	public class Warning : Noticeable { }
	/// <summary>
	/// Operation that will not throw
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class OperationResult<T>
	{
		/// <summary>
		/// The result of an operation.
		/// </summary>
		public T Result;
		/// <summary>
		/// Initialize with given value.
		/// </summary>
		/// <param name="result"></param>
		public OperationResult(T result)
		{
			Result = result;
		}
		/// <summary>
		/// Initialize with default(T).
		/// </summary>
		public OperationResult()
		{
			Result = default;
		}
		/// <summary>
		/// Error list.
		/// </summary>
		public List<Error> Errors = new List<Error>();
		/// <summary>
		/// Warning list.
		/// </summary>
		public List<Warning> Warnings = new List<Warning>();
		/// <summary>
		/// Add an error with no parameter.
		/// </summary>
		/// <typeparam name="E"></typeparam>
		public void AddError<E>() where E : Error, new()
		{
			Errors.Add(new E());
		}
		/// <summary>
		/// Add an initialized error to the list.
		/// </summary>
		/// <param name="e"></param>
		public void AddError(Error e)
		{
			Errors.Add(e);
		}
		/// <summary>
		/// Add an initialized warning to the list.
		/// </summary>
		/// <param name="warning"></param>
		public void AddWarning(Warning warning)
		{
			Warnings.Add(warning);
		}
		/// <summary>
		/// Add a warning with no parameter
		/// </summary>
		/// <typeparam name="E"></typeparam>
		public void AddWarning<E>() where E : Warning, new()
		{
			Warnings.Add(new E());
		}
		/// <summary>
		/// Convert a result from T result.
		/// </summary>
		/// <param name="result"></param>
		public static implicit operator OperationResult<T>(T result)
		{
			return new OperationResult<T>(result);
		}
		/// <summary>
		/// Is the result has any errors?
		/// </summary>
		/// <returns></returns>
		public bool HasError() => Errors.Count > 0;
		/// <summary>
		/// Is the result has any warnings?
		/// </summary>
		/// <returns></returns>
		public bool HasWarning() => Warnings.Count > 0;
		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="V"></typeparam>
		/// <param name="AnotherResult"></param>
		/// <returns></returns>
		public bool CheckAndInheritErrorAndWarnings<V>(OperationResult<V> AnotherResult)
		{
			foreach (var item in AnotherResult.Errors)
			{
				AddError(item);
			}
			foreach (var item in AnotherResult.Warnings)
			{
				AddWarning(item);
			}
			return AnotherResult.HasError();
		}
	}
}
