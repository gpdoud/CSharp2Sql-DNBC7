﻿using System;
using CSharp2SqlLibrary;
using System.Diagnostics;

namespace CSharp2SqlConsole {
    class Program {

        void RunProductsTest() {
            var conn = new Connection(@"localhost\sqlexpress", "PrsDb7");
            conn.Open();
            Products.Connection = conn;
            Vendors.Connection = conn;

            var product = new Products() {
                PartNbr = "XYZ001", Name = "XYZ Part", Price = 10, Unit = "Each", PhotoPath = null,
                VendorId = Vendors.GetByCode("AMAZ").Id
            };
            try {
                Console.WriteLine("Product list for Vendor Amazon:");
                var productList = Vendors.GetProducts("AMAZ");
                foreach(var prod in productList) {
                    Console.WriteLine(prod);
                }
                // insert
                //var success = Products.Insert(product);
                // update product.id = 3
                //var p = Products.GetByPartNbr("XYZ001");
                //p.Name = "Greg New Part XYZ";
                //p.VendorId = Vendors.GetByCode("XXX").Id;
                //success = Products.Update(p);

                //p = Products.GetByPartNbr("XYZ001");
                //Console.WriteLine(p);
                //success = Products.Delete(p.Id);
            } catch(Exception ex) {
                Console.WriteLine($"Exception occurred: {ex.Message}");
            }

            var products = Products.GetAll();
            foreach(var p in products) {
                Console.WriteLine($"Product {p.Name} from Vendor {p.Vendor.Name} is priced at {p.Price}");
            }

            conn.Close();
        }
        void RunVendorsTest() {
            var conn = new Connection(@"localhost\sqlexpress", "PrsDb7");
            conn.Open();
            Vendors.Connection = conn;

            // Insert
            var vendor = new Vendors {
                Code = "TARG", Name = "Target",
                Address = "123 Any St.", City = "Minneapolis", State = "MN", Zip = "12345",
                Phone = null, Email = null
            };
            var success = Vendors.Insert(vendor);
            Console.WriteLine($"Insert worked: {success}");

            Console.WriteLine("======================================");

            var amazon = Vendors.GetByPk(3);
            Console.WriteLine($"Vendor 3 found: {amazon}");
            var notfound = Vendors.GetByPk(333);
            Console.WriteLine(notfound?.ToString() ?? "Vendor 333 not found");

            Console.WriteLine("======================================");

            var vendors = Vendors.GetAll();
            foreach(var v in vendors) {
                Console.WriteLine(v.Name);
            }

            conn.Close();
        }

        void RunUsersTest() {
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
            newuser.Username = "ABC08";
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
            //pgm.RunUsersTest();
            //pgm.RunVendorsTest();
            pgm.RunProductsTest();
        }
    }
}
