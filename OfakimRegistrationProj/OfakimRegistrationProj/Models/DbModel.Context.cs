namespace OfakimRegistrationProj.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.IO;
    using System.Linq;
    using System.Threading;
    public partial class AppDBEntities : DbContext
    {
        public virtual DbSet<User> Users { get; set; }
        public List<User> users = new List<User>();
        private bool isDBUp = false;
        private string JsonDataPath = (Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase))) + @"\UsersJson.txt").Replace("file:\\", "");

        public AppDBEntities() : base("name=AppDBEntities")
        {
            if (!File.Exists(JsonDataPath))
                File.Create(JsonDataPath).Close();
            users = GetUsersFromJson().ToList();
        }
        public bool Simplevalidations(User user)
        {
            if (user.Email == "" || user.Email == null || !user.Email.Contains("@") || !user.Email.Contains("."))
                return false;
            if (user.FullName == "" || user.FullName == null)
                return false;
            if (user.PhoneNumber == "" || user.PhoneNumber == null)
            {
                try
                {
                    int num = int.Parse(user.PhoneNumber);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            if (user.birthday == "" || user.birthday == null)
            {
                try
                {
                    DateTime Date = DateTime.Parse(user.birthday);
                }
                catch (Exception)
                {
                    //Wrong date format insereted by user
                    return false;
                }
            }
            return true;
        }
        public IEnumerable<User> UpdateAndGetUsers(User userModel, bool isDataOk)
        {
            IEnumerable<User> adaptiveUsers;
            if (isDBUp)
                adaptiveUsers = GetUsersFromDBIfFailsGetFromJson(userModel, isDataOk);
            else
                adaptiveUsers = GetUsersFromJson(userModel, isDataOk);

            return adaptiveUsers;
        }

        private IEnumerable<User> GetUsersFromJson(User userModel, bool isDataOk)
        {
            IEnumerable<User> adaptiveUsers;
            if (isDataOk)
                PutUserToJson(userModel);
            adaptiveUsers = GetUsersFromJson();
            return adaptiveUsers;
        }

        private IEnumerable<User> GetUsersFromDBIfFailsGetFromJson(User userModel, bool isDataOk)
        {
            IEnumerable<User> adaptiveUsers;
            try
            {
                if (isDataOk)
                {
                    Users.Add(userModel);
                    SaveChanges();
                }
                adaptiveUsers = Users.ToList();
            }
            catch (Exception)
            {
                adaptiveUsers = GetUsersFromJson(userModel, isDataOk);
            }

            return adaptiveUsers;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
        private IEnumerable<User> GetUsersFromJson()
        {
            string MyJsonFromFile = "";
            try
            {
                using (var r = new StreamReader(JsonDataPath))
                    MyJsonFromFile = r.ReadToEnd();
                var users = JsonConvert.DeserializeObject<List<User>>(MyJsonFromFile);
                return users;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private void PutUserToJson(User user)
        {
            try
            {
                users.Add(user);
                var jsonWrite = JsonConvert.SerializeObject(users, Formatting.Indented);
                using (var writer = new StreamWriter(JsonDataPath))
                {
                    writer.Write(jsonWrite);
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
