/*
Topic Name: Electronics Store
Project Name: PcPulse
Group Name: SoftByte
Group Members:
    Shree Dhar Acharya: 8899288
    Karamjot Singh: 8869324
    Prashant Sahu: 8877584
    Jovan Sandhu: 8873660

Description:
- This class represents a view model used to display user information in the Electronics Store project.
- It includes properties such as userId, FirstName, LastName, Email, and UserRole.
*/
#nullable disable
namespace PcPulse.ViewModel
{
    public class UserViewModel
    {
        public string userId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserRole { get; set; }
    }
}
