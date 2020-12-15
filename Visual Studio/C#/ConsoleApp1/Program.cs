using System;
using Eruru.MySqlHelper;

namespace ConsoleApp1 {

	class Program {

		static void Main (string[] args) {
			Console.Title = nameof (ConsoleApp1);
			MySqlHelper mySql = new MySqlHelper ("Server=localhost;DataBase=eruru;uid=root;pwd=1234");
			mySql.ExecuteReader ("select * from checkin where qq = 1", mySqlDataReader => {
				Console.WriteLine (mySqlDataReader.HasRows);
			});
			mySql.ExecuteReader ("select * from checkin where qq = 10", mySqlDataReader => {
				Console.WriteLine (mySqlDataReader.HasRows);
			});
			Console.ReadLine ();
		}

	}

}