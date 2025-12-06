namespace SIS
{
    public class ContactPerson : IPerson
    {
        private string functionTitle;
        private string departmentName;

        public string GetContactInfo()
        {
            return email + '\n' + phoneNumber;
        }
    }
}