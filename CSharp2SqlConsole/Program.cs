using System;
using CSharp2SqlLibrary;
using System.Diagnostics;

namespace CSharp2SqlConsole {
    class Program {

        void Run() {
            var conn = new Connection(@"localhost\sqlexpress", "PrsDb7");
            conn.Open();
            Users.Connection = conn;
            var userLogin = Users.Login("sa", "sa");
            Console.WriteLine(userLogin);
            var userFailedLogin = Users.Login("xx", "Xx");
            Console.WriteLine(userFailedLogin?.ToString() ?? "Not found");
            var users = Users.GetAll();
            foreach(var u in users) {
                Console.WriteLine(u);
            }
            var user = Users.GetByPk(2);
            Debug.WriteLine(user);
            var usernf = Users.GetByPk(222);
            conn.Close();
        }
        static void Main(string[] args) {
            var pgm = new Program();
            pgm.Run();
        }
    }
}
