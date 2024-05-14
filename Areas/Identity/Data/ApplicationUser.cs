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
- ApplicationUser: Represents application users and includes profile data such as first name and last name.
*/


#nullable disable
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace PcPulse.Areas.Identity.Data;

// Adding  profile data such as firtName and LastName
// for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    [Display(Name = "First Name")]
    [Required]
    public string FirstName { get; set; }
    [Display(Name = "Last Name")]
    [Required]
    public string LastName { get; set; }
}

