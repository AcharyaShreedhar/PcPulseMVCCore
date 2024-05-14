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
- This class represents a view model used to display product items in the Electronics Store project.
- It includes properties such as Product (of type Product), Quantity, and SubTotal.
*/
#nullable disable
using PcPulse.Models;

namespace PcPulse.ViewModel
{
    public class ProductItem
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }


        private decimal _SubTotal;
        //Calulating SubToal
        public decimal SubTotal
        {
            get { return _SubTotal; }
            set { _SubTotal = Product.ProductPrice * Quantity; }
        }
    }
}
