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
			if (MySqlHelperAPI.IsNullOrWhiteSpace (connectionString)) {
				throw new ArgumentException ($"“{nameof (connectionString)}”不能为 Null 或空白", nameof (connectionString));
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
			if (MySqlHelperAPI.IsNullOrWhiteSpace (connectionString)) {
				throw new ArgumentException ($"“{nameof (connectionString)}”不能为 Null 或空白", nameof (connectionString));
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
			if (MySqlHelperAPI.IsNullOrWhiteSpace (connectionString)) {
				throw new ArgumentException ($"“{nameof (connectionString)}”不能为 Null 或空白", nameof (connectionString));
			}
			Dispose ();
			Connection.ConnectionString = connectionString;
			Connection.Open ();
		}

		public int ExecuteNonQuery (string commandText) {
			if (MySqlHelperAPI.IsNullOrWhiteSpace (commandText)) {
				throw new ArgumentException ($"“{nameof (commandText)}”不能为 Null 或空白", nameof (commandText));
			}
			lock (Lock) {
				using (MySqlCommand command = new MySqlCommand (commandText, Connection)) {
					return command.ExecuteNonQuery ();
				}
			}
		}

		public void ExecuteReader (string commandText, Action<MySqlDataReader> action) {
			if (MySqlHelperAPI.IsNullOrWhiteSpace (commandText)) {
				throw new ArgumentException ($"“{nameof (commandText)}”不能为 Null 或空白", nameof (commandText));
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
			if (MySqlHelperAPI.IsNullOrWhiteSpace (commandText)) {
				throw new ArgumentException ($"“{nameof (commandText)}”不能为 Null 或空白", nameof (commandText));
			}
			lock (Lock) {
				using (MySqlCommand command = new MySqlCommand (commandText, Connection)) {
					return command.ExecuteScalar ();
				}
			}
		}

		public int Fill (string commandText, DataSet dataSet) {
			if (MySqlHelperAPI.IsNullOrWhiteSpace (commandText)) {
				throw new ArgumentException ($"“{nameof (commandText)}”不能为 Null 或空白", nameof (commandText));
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
			if (MySqlHelperAPI.IsNullOrWhiteSpace (commandText)) {
				throw new ArgumentException ($"“{nameof (commandText)}”不能为 Null 或空白", nameof (commandText));
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