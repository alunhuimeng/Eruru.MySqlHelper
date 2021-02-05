using System;
using System.Threading.Tasks;
using Eruru.MySqlHelper;

namespace ConsoleApp1 {

	class Program {

		static void Main (string[] args) {
			Console.Title = nameof (ConsoleApp1);
			MySqlHelper mySql = new MySqlHelper ("Server=localhost;DataBase=eruru;uid=root;pwd=1234");
			for (int i = 0; i < 1000; i++) {
				Task.Run (() => {
					mySql.ExecuteReader ("select * from checkin where qq = 1", mySqlDataReader => {
						while (mySqlDataReader.Read ()) {
							Console.WriteLine (mySqlDataReader["qq"]);
						}
					});
				});
			}
			Console.ReadLine ();
		}

	}

}