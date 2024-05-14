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
- This model represents the details of an order in the PcPulse application.
- It contains properties such as Id, Quantity, SalePrice, productId, and OrderId.
- The productId property represents the identifier of the product associated with this order detail.
- The OrderId property represents the identifier of the order to which this detail belongs.
- The Order property is a navigation property representing the order associated with this order detail.
- The ForeignKey attribute is used to specify the foreign key relationship between the OrderDetail and Order entities.
*/
#nullable disable
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PcPulse.Models
{
    public class OrderDetail
    {
        [Key]
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal SalePrice { get; set; }
        public int productId { get; set; }
        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        public Order Order { get; set; }
    }
}
