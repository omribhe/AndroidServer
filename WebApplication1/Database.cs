using WebApplication1.Models;
using Microsoft.EntityFrameworkCore;
namespace WebApplication1;
public static class Database
    {
        public static List<User> users = new List<User>();


        public static bool Exist(string DBkey)
        {
                using(var db = new WebApplicationContext())
            {
                User test = db.Users.Find(DBkey);
                if(test == null)
                {
                    return false;
                }
                return true;
            }
        }

        public static List<User> Get()
        {
        using (var db = new WebApplicationContext())
        {
            var temp = db.Users.Include(x => x.Contacts).ThenInclude(x => x.messages).ToList();
            return temp;
        }
        }

        public static User Get(string userName)
        {
            using (var db = new WebApplicationContext())
            {
                var temp = db.Users.Include(x => x.Contacts).ThenInclude(x => x.messages).First(x=> x.Name == userName);
                return temp;
            }
        }

        public static void Insert(User user)
        {
            using (var db = new WebApplicationContext())
            {
            db.Users.Add(user);
            db.SaveChanges();
            }
        }

        public static void InsertContact(Contact contact)
        {
            using (var db = new WebApplicationContext())
            {
                db.Contacts.Add(contact);
                db.SaveChanges();
            }
        }

        public static void Remove(User user)
        {
            users.Remove(user);
        }
        public static void addContact(string name, Contact contact)
        {
            if(contact == null)
            {
                throw new ArgumentNullException();
            }
            if (users.Exists(x => x.Name == name))
            {
                users.Find(x => x.Name == name).Contacts.Add(contact);
            }
        }


        public static void addContactFromString(string name, String contact, String nickName, String server)
        {
            if (contact == null)
            {
                throw new ArgumentNullException();
            }
            if (Exist(name))
            {
                Contact userFriend = new Contact(contact, nickName, "", server, "",name);
                InsertContact(userFriend);
            }
        }

        public static void putContactFromString(string name, string id, String nickName, String server)
        {
            if (id == null)
            {
                throw new ArgumentNullException();
            }
        User u = Get(name);
        Contact c = u.Contacts.Find(x => x.id == id);
        if (c == null)
        {
            return;
        }
        c.name = nickName;
        c.server = server;
        using(var db = new WebApplicationContext())
        {
            db.Contacts.Update(c);
            db.SaveChanges();
        }
        }

        public static void addMessage(string name, string contactID, Message message)
        {
            if (users.Exists(x => x.Name == name))
            {
               if (users.Find(x => x.Name == name).Contacts.Exists(x=> x.id == contactID))
                {
                    users.Find(x => x.Name == name).Contacts.Find(x => x.id == contactID).messages.Add(message);
                }
            }
        }



        public static void delContact(string name, string contactName)
        {
        User u = Get(name);
        using(var db = new WebApplicationContext())
        {
            Contact c = u.Contacts.Find(x => x.id == contactName);
            db.Contacts.Remove(c);
            db.SaveChanges();
        }
        }

        public static void addTransfer(string username, string contactName, string mess , bool sent)
        {
            DateTime d = DateTime.Now;
            string time = d.ToString();
            Contact c = Database.Get(username).Contacts.Find(x => x.id == contactName);
            Message message = new Message(c.countMessages, mess, time, sent, contactName);
            c.messages.Add(message);
            c.last = mess;
            c.lastdate = time;
            c.countMessages++;
            using( var db = new WebApplicationContext())
        {
            db.Messages.Add(message);
            db.Contacts.Update(c);
            db.SaveChanges();
        }
        }

        public static void addInvitation(string username, string contactName, string server)
        {
            User u = Database.Get(username);
            Contact c = new Contact(contactName, contactName, "", server, "", username);
            using(var db = new WebApplicationContext())
        {
            db.Contacts.Add(c);
            db.SaveChanges();
        }
        }

        public static void putMessage(Message m, Contact c)
        {
            using(var db = new WebApplicationContext())
            {
                db.Messages.Update(m);
                db.Contacts.Update(c);
                db.SaveChanges();
            }
        }
        public static void delMessage(Message m, Contact c)
        {
        using ( var db = new WebApplicationContext())
            {
                db.Messages.Remove(m);
                db.Contacts.Update(c);
                db.SaveChanges();
            }
        }
    }

