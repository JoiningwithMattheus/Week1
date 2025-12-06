namespace SIS
{
    public abstract class Organization
    {
        private int id;
        private string name;
        private string address;
        private string phoneNumber;
        private string email;
        private string url;

        public int GetIdentifier()
        {
            return id;
        } 

    }
}