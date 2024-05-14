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
- This model represents the category entity in the PcPulse application.
- It contains properties such as Id, ShortName, and LongName to define a category.
- The [Key] attribute specifies that the Id property is the primary key.
- The [Required] attribute ensures that ShortName and LongName properties are required fields.
- The [Display(Name = "Short Name")] attribute specifies the display name for the ShortName property in UI.
- The [Display(Name = "Long Name")] attribute specifies the display name for the LongName property in UI.
*/


#nullable disable
using System.ComponentModel.DataAnnotations;

namespace PcPulse.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Short Name")]

        public string ShortName { get; set; }
        [Required]
        [Display(Name = "Long Name")]
        public string LongName { get; set; }
    }
}
