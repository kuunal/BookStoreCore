using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Utility
{
    public interface IEmailItemDetails
    {
        string ItemDetailHtml(string title, string author, string image, int price, int quantity);
        string OrderDetailHtml(string orderId, DateTime orderedDate, int total);    
    }
}
