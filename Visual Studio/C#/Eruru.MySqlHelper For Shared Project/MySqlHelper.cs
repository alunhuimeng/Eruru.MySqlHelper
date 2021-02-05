using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace Eruru.MySqlHelper {

	public class MySqlHelper : IDisposable {

		public MySqlConnection Connection { get; } = new MySqlConnection ();

		readonly object Lock = new object ();

		public MySqlHelper () {

		}
		public MySqlHelper (string connectionString) {
			if (connectionString is null) {
				throw new ArgumentNullException (nameof (connectionString));
			}
			Open (connectionString);
		}
		public MySqlHelper (object connectionString) : this (connectionString.ToString ()) {

		}

		public void Connect () {
			Dispose ();
			Connection.Open ();
		}
		public void Connect (string connectionString) {
			if (connectionString is null) {
				throw new ArgumentNullException (nameof (connectionString));
			}
			Open (connectionString);
		}
		public void Connect (object connectionString) {
			if (connectionString is null) {
				throw new ArgumentNullException (nameof (connectionString));
			}
			Open (connectionString.ToString ());
		}

		void Open (string connectionString) {
			if (connectionString is null) {
				throw new ArgumentNullException (nameof (connectionString));
			}
			Dispose ();
			Connection.ConnectionString = connectionString;
			Connection.Open ();
		}

		public int ExecuteNonQuery (string commandText) {
			if (commandText is null) {
				throw new ArgumentNullException (nameof (commandText));
			}
			lock (Lock) {
				using (MySqlCommand command = new MySqlCommand (commandText, Connection)) {
					return command.ExecuteNonQuery ();
				}
			}
		}

		public void ExecuteReader (string commandText, Action<MySqlDataReader> action) {
			if (commandText is null) {
				throw new ArgumentNullException (nameof (commandText));
			}
			if (action is null) {
				throw new ArgumentNullException (nameof (action));
			}
			lock (Lock) {
				using (MySqlCommand command = new MySqlCommand (commandText, Connection)) {
					using (MySqlDataReader dataReader = command.ExecuteReader ()) {
						action (dataReader);
					}
				}
			}
		}

		public object ExecuteScalar (string commandText) {
			if (commandText is null) {
				throw new ArgumentNullException (nameof (commandText));
			}
			lock (Lock) {
				using (MySqlCommand command = new MySqlCommand (commandText, Connection)) {
					return command.ExecuteScalar ();
				}
			}
		}

		public int Fill (string commandText, DataSet dataSet) {
			if (commandText is null) {
				throw new ArgumentNullException (nameof (commandText));
			}
			if (dataSet is null) {
				throw new ArgumentNullException (nameof (dataSet));
			}
			lock (Lock) {
				using (MySqlDataAdapter dataAdapter = new MySqlDataAdapter (commandText, Connection)) {
					return dataAdapter.Fill (dataSet);
				}
			}
		}

		public DataTable Fill (string commandText, DataTable dataTable, SchemaType schemaType) {
			if (commandText is null) {
				throw new ArgumentNullException (nameof (commandText));
			}
			if (dataTable is null) {
				throw new ArgumentNullException (nameof (dataTable));
			}
			lock (Lock) {
				using (MySqlDataAdapter dataAdapter = new MySqlDataAdapter (commandText, Connection)) {
					return dataAdapter.FillSchema (dataTable, schemaType);
				}
			}
		}

		#region IDisposable

		public void Dispose () {
			lock (Lock) {
				Connection.Close ();
			}
		}

		#endregion

	}

}