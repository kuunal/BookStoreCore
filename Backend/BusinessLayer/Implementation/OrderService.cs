using BusinessLayer.Exceptions;
using BusinessLayer.Interface;
using BusinessLayer.MQServices;
using EmailService;
using ModelLayer.OrderDto;
using RepositoryLayer.Implementation;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Implementation
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repository;
        private readonly IMqServices _mqServices;
        private readonly IEmailSender _emailSender;

        public OrderService(IOrderRepository repository, IMqServices mqServices)
        {
            _repository = repository;
            _mqServices = mqServices;
        }

        public async Task<OrderResponseDto> Add(OrderRequestDto orderRequest, int userId)
        {
            try {
                OrderResponseDto order = await _repository.Add(orderRequest, userId);
                Message message = new Message(new string[] { "kunaldeshmukh2503@gmail.com" },
                "Order successfully placed!",
                $"{ItemDetailHtml(order.Book.Title, order.Book.Author, order.Book.Image, order.Book.Price, order.Book.Quantity)+ OrderDetailHtml(order.OrderId, order.OrderedDate, order.Book.Price)}");
                _mqServices.AddToQueue(message);
                return order;
            }
            catch (SqlException e) when(e.Number == SqlErrorNumbers.CONSTRAINT_VOILATION)
            {
                throw new BookstoreException("Invalid user!");
            }
        }


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
