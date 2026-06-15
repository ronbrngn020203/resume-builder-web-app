using System;

namespace ResumeBuilderWebApp.Models
{
    public class CartItem
    {
        public int ServiceId { get; set; }
        public string Title { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; } = 1;

        public decimal Total => Price * Quantity;
    }
}
