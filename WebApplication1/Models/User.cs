using System.ComponentModel.DataAnnotations;
namespace WebApplication1.Models

{
    public class User
    {
        public string Name { get; set; }
        public string? Nickname { get; set; }       
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string? Server { get; set; }  

        public string? Picture { get; set; }
        public List<Contact>? Contacts { get; set; }

        public User()
        {
            Contacts = new List<Contact>(); 
        }
        public User(string name, string nickname, string password, string server, string picture)
        {
            Name = name;      
            Nickname = nickname;    
            Password = password;    
            Server = server;    
            Picture = picture;
            Contacts = new List<Contact>();
        }
        public User(string name, string nickname, string password, string server, string picture, List<Contact> cList)
        {
            Name = name;
            Nickname = nickname;
            Password = password;
            Server = server;
            Picture = picture;
            Contacts =cList;
        }
    }
}
