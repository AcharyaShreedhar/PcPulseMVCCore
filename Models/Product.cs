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
- This model represents a product in the PcPulse application.
- It contains properties such as Id, ProductName, ProductPrice, ProductDescription, ProductPicture, ProductFile, CategoryId, and ProductQuantity.
- The ProductName property represents the name of the product.
- The ProductPrice property represents the price of the product.
- The ProductDescription property represents a short description of the product.
- The ProductPicture property stores the filename of the product picture.
- The ProductFile property is a non-mapped property used to upload product images.
- The CategoryId property represents the identifier of the category to which this product belongs.
- The Category property is a navigation property representing the category associated with this product.
- The ProductQuantity property represents the available quantity of the product.
*/

#nullable disable
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PcPulse.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }
        [Required]
        [Display(Name = "Product Price")]
        public decimal ProductPrice { get; set; }
        [Required]
        [Display(Name = "Short Description")]
        public string ProductDescription { get; set; }

        [Display(Name = "Product Picture")]
        public string ProductPicture { get; set; }
        [NotMapped]
        public IFormFile ProductFile { get; set; }
        [Required]
        [Display(Name = "Select Category")]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]



        public Category Category { get; set; }
        public int ProductQuantity { get; set; }
    }
}
