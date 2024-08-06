namespace JWTToken.Models
{
    public class Users
    {
        public int id { get; set; }
        public string? username { get; set; }
        public string? password { get; set; }
        public string? role { get; set; }
    }

    public class UserList
    {
        public static List<Users> Users = new()
        {
            new Users { id = 1,username ="nurullahotkan",password="123",role="manager"},
            new Users { id = 1,username ="nurullahotkan2",password="123",role="user"}
        };
    }
}
