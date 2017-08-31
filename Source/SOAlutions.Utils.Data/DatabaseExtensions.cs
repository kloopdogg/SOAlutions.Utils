// © 2013 SOAlutions, Inc. All rights reserved.
// Please direct all inquiries to http://www.soalutions.net

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace SOAlutions.Utils.Data
{
	public static class DatabaseExtensions
	{
		#region Delegates

		/// <summary>
		/// The GenerateObjectFromReader delegate represents any method
		/// which returns an object of type T for a DataReader
		/// </summary>
		public delegate T GenerateObjectFromReader<T>(IDataReader reader);

		#endregion Delegates

		public static T CreateObject<T>(this Database db, DbCommand cmd, GenerateObjectFromReader<T> gofr)
		{
			try
			{
				using (IDataReader dr = db.ExecuteReader(cmd))
				{
					try
					{
						T temp = gofr(dr);
						return temp;
					}
					finally
					{
						if (!dr.IsClosed)
						{
							dr.Close();
						}
					}
				}
			}
			catch (DbException ex)
			{
				throw new StoredProcedureException(cmd, ex);
			}
		}

		public static Collection<T> CreateCollection<T>(this Database db, DbCommand cmd, GenerateObjectFromReader<T> gofr)
		{
			try
			{
				using (IDataReader dr = db.ExecuteReader(cmd))
				{
					try
					{
						Collection<T> temp = GenerateCollectionFromReader(dr, gofr);
						return temp;
					}
					finally
					{
						if (!dr.IsClosed)
						{
							dr.Close();
						}
					}
				}
			}
			catch (DbException ex)
			{
				throw new StoredProcedureException(cmd, ex);
			}
		}

		private static Collection<T> GenerateCollectionFromReader<T>(IDataReader reader, GenerateObjectFromReader<T> gofr)
		{
			Collection<T> collection = new Collection<T>();
			T t = default(T);
			bool hasRows = false;
			do
			{
				t = gofr(reader);
				try
				{
					hasRows = t.GetHashCode() > 0;
				}
				catch
				{
					hasRows = false;
				}
				if (hasRows)
				{
					collection.Add(t);
				}
			} while (hasRows);
			return collection;
		}

		public static T GetAccessorObject<T>(this Database db, string procedureName, IResultSetMapper<T> resultSetMapper,
			params object[] parameterValues) where T : new()
		{
			ParameterMapperBase parameterMapper = new ParameterMapperBase(db);
			return GetAccessorObject(db, procedureName, parameterMapper, resultSetMapper, parameterValues);
		}

		public static T GetAccessorObject<T>(this Database db, string procedureName, ParameterMapperBase parameterMapper,
			IResultSetMapper<T> resultSetMapper, params object[] parameterValues) where T : new()
		{
			try
			{
				IEnumerable<T> collection = GetAccessorCollection<T>(db, procedureName, parameterMapper, resultSetMapper, parameterValues);
				if (collection != null)
				{
					return collection.Single();
				}
				else
				{
					return default(T);
				}
			}
			catch (DbException ex)
			{
				if (parameterMapper.Command != null)
				{
					throw new StoredProcedureException(parameterMapper.Command, ex);
				}
				else
				{
					throw;
				}
			}
		}

		public static IEnumerable<T> GetAccessorCollection<T>(this Database db, string procedureName, IResultSetMapper<T> resultSetMapper,
			params object[] parameterValues) where T : new()
		{
			ParameterMapperBase parameterMapper = new ParameterMapperBase(db);
			return db.GetAccessorCollection<T>(procedureName, parameterMapper, resultSetMapper, parameterValues);
		}

		public static IEnumerable<T> GetAccessorCollection<T>(this Database db, string procedureName, IRowMapper<T> rowMapper,
			params object[] parameterValues) where T : new()
		{
			ParameterMapperBase parameterMapper = new ParameterMapperBase(db);
			return db.GetAccessorCollection<T>(procedureName, parameterMapper, rowMapper, parameterValues);
		}

		public static IEnumerable<T> GetAccessorCollection<T>(this Database db, string procedureName, ParameterMapperBase parameterMapper,
			IRowMapper<T> rowMapper, params object[] parameterValues) where T : new()
		{
			IEnumerable<T> collection = null;

			try
			{
				collection = db.ExecuteSprocAccessor<T>(procedureName, parameterMapper, rowMapper, parameterValues);

				if (collection != null)
				{
					return collection;
				}
				else
				{
					return new List<T>();
				}
			}
			catch (DbException ex)
			{
				if (parameterMapper.Command != null)
				{
					throw new StoredProcedureException(parameterMapper.Command, ex);
				}
				else
				{
					throw;
				}
			}
		}

		public static IEnumerable<T> GetAccessorCollection<T>(this Database db, string procedureName, ParameterMapperBase parameterMapper, IResultSetMapper<T> resultSetMapper,
			params object[] parameterValues) where T : new()
		{
			IEnumerable<T> collection = null;

			try
			{
				collection = db.ExecuteSprocAccessor<T>(procedureName, parameterMapper, resultSetMapper, parameterValues);

				if (collection != null)
				{
					return collection;
				}
				else
				{
					return new List<T>();
				}
			}
			catch (DbException ex)
			{
				if (parameterMapper.Command != null)
				{
					throw new StoredProcedureException(parameterMapper.Command, ex);
				}
				else
				{
					throw;
				}
			}
		}

		public static int ExecuteNonQuery(this Database db, DbCommand cmd, bool throwStoredProcedureExceptions)
		{
			int returnValue;
			try
			{
				returnValue = db.ExecuteNonQuery(cmd);
			}
			catch (DbException ex)
			{
				if (throwStoredProcedureExceptions)
				{
					throw new StoredProcedureException(cmd, ex);
				}
				else
				{
					throw;
				}
			}
			return returnValue;
		}

		public static object ExecuteScalar(this Database db, DbCommand cmd, bool throwStoredProcedureExceptions)
		{
			object returnValue;
			try
			{
				returnValue = db.ExecuteScalar(cmd);
			}
			catch (DbException ex)
			{
				if (throwStoredProcedureExceptions)
				{
					throw new StoredProcedureException(cmd, ex);
				}
				else
				{
					throw;
				}
			}
			return returnValue;
		}

		public static T ExecuteScalar<T>(this Database db, DbCommand cmd, bool throwStoredProcedureExceptions)
			where T : struct
		{
			object returnValue = db.ExecuteScalar(cmd, throwStoredProcedureExceptions);
			string stringResult = returnValue.ToString();

			T result;
			TypeParser<T>.TryParse(stringResult, out result);
			return result;
		}
	}
}