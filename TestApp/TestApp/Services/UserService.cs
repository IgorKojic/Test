using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web;
using TestApp.Models;
using TestApp.ViewModels;

namespace TestApp.Services
{
    public class UserService
    {
        private TestDBContext testDBContext;

        public UserService()
        {
            testDBContext = new TestDBContext();
        }

        public void Register(User user)
        {
            try
            {
                User newUser = new User();

                newUser.FirstName = user.FirstName;
                newUser.LastName = user.LastName;
                newUser.Email = user.Email;
                newUser.UserName = user.UserName;
                newUser.Password = EncryptPassword(user.Password);

                testDBContext.Users.Add(newUser);
                testDBContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public User Login(LoginViewModel user)
        {
            try
            {
               string encryptedPass = EncryptPassword(user.Password);

               return testDBContext.Users.FirstOrDefault(x => x.UserName == user.UserName && x.Password == encryptedPass);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static string EncryptPassword(string password)
        {
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                UTF8Encoding utf8 = new UTF8Encoding();
                byte[] data = md5.ComputeHash(utf8.GetBytes(password));
                return Convert.ToBase64String(data);
            }
        }

        public bool IsUserExist(User user)
        {
            try
            {
                return testDBContext.Users.Any(x => x.UserName == user.UserName || x.Email == user.Email);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Dispose()
        {
            testDBContext.Dispose();
        }
    }
}