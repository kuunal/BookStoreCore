using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Utility
{
    public class EmailItemDetails : IEmailItemDetails
    {
        public string OrderDetailHtml(string orderId, DateTime orderedDate, int total)
        {
            return @$"<p>OrderId: {orderId}</p>
                       <p>Date: {orderedDate}</p>
                       <p>Total: {total}</p>";
        }

        public string ItemDetailHtml(string title, string author, string image, int price, int quantity)
        {
            return @$"<p>Title: {title}</
                    <p>author: {author}</p>
                    <p> Image: <img src = '{image}'></p>
                    <p> price: {price}</p>
                    <p>Quantity: {quantity}</p>";
        }
    }
}
