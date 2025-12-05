namespace Authentication_2025.Models
{
    public class LoginLogoutiInfo
    {
       
            public int Id { get; set; }
            public int UserId { get; set; }
            public DateTime LoginTime { get; set; }
            public DateTime? LogoutTime { get; set; }

            public User User { get; set; }
        

    }
}
