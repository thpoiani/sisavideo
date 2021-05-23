namespace SisAVideo
{
    class LoginRole
    {
        static public string username;
        static public string password;

        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }
    }
}
