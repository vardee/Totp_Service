namespace totp_Module.Data.Entities
{
    public class User
    {
        public Guid ID { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string TOTPSecretKey { get; set; }
    }
}
