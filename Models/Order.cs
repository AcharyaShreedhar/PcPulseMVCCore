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
- This model represents an order in the PcPulse application.
- It contains properties such as Id, FirstName, LastName, Email, Address, City, PostalCode, OrderDate, OrderNumber, OrderType, userId, CountryState, and PhoneNumber.
- Each property is decorated with data annotations to specify validation rules and display names for UI elements.
- The OrderDate property represents the date and time when the order was placed.
- The OrderNumber property holds the unique identifier of the order.
- The OrderType property indicates the type of the order (e.g., Pending, Delivered).
- The userId property represents the user who placed the order.
*/

#nullable disable
using System.ComponentModel.DataAnnotations;

namespace PcPulse.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public string Email { get; set; }

        [Required]
        [Display(Name = "Adress")]
        public string Address { get; set; }
        [Required]
        [Display(Name = "City ")]
        public string City { get; set; }
        [Required]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }
        public DateTime OrderDate { get; set; }
        public int OrderNumber { get; set; }
        public string OrderType { get; set; }
        public string userId { get; set; }
        [Required]
        [Display(Name = "Adress")]
        public string CountryState { get; set; }
        [Required]
        [Display(Name = "Mobile Number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Credit card number is required")]
        [RegularExpression(@"^[0-9]{13,16}$", ErrorMessage = "Please enter a valid credit card number")]
        public string CreditCardNumber { get; set; }

        [Required(ErrorMessage = "Expiration month is required")]
        [RegularExpression(@"^(0[1-9]|1[0-2])$", ErrorMessage = "Please enter a valid month (01-12)")]
        public string ExpMonth { get; set; }

        [Required(ErrorMessage = "Expiration year is required")]
        [RegularExpression(@"^[0-9]{4}$", ErrorMessage = "Please enter a valid year (4 digits)")]
        public string ExpYear { get; set; }

        [Required(ErrorMessage = "CVV is required")]
        [RegularExpression(@"^[0-9]{3,4}$", ErrorMessage = "Please enter a valid CVV (3 or 4 digits)")]
        public string CVV { get; set; }
    
}
}
