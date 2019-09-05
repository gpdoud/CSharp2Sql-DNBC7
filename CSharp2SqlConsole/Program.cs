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
            var success = Users.Delete(3);
            var user3 = Users.GetByPk(3);
            Debug.WriteLine(user3);

            var newuser = new Users();
            newuser.Username = "ABC04";
            newuser.Password = "XYZ";
            newuser.Firstname = "Normal";
            newuser.Lastname = "newuser";
            //newuser.Phone = "5135551212";
            //newuser.Email = "info@maxtrain.com";
            newuser.IsAdmin = false;
            newuser.IsReviewer = true;
            success = Users.Insert(newuser);

            var userabc = Users.GetByPk(5);
            userabc.Username = "ABC00";
            userabc.Firstname = "A";
            userabc.Lastname = "BC";
            success = Users.Update(userabc);

            conn.Close();
        }
        static void Main(string[] args) {
            var pgm = new Program();
            pgm.Run();
        }
    }
}
