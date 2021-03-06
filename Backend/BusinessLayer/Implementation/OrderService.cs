﻿using BusinessLayer.Exceptions;
using BusinessLayer.Interface;
using BusinessLayer.MQServices;
using BusinessLayer.Utility;
using Caching;
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
        private readonly IEmailItemDetails _emailItems;
        private readonly ICacheRepository _cacheRepository;
        private readonly IEmailSender _emailSender;

        public OrderService(IOrderRepository repository
            , IMqServices mqServices
            , IEmailItemDetails details
            , ICacheRepository cacheRepository)
        {
            _repository = repository;
            _mqServices = mqServices;
            _emailItems = details;
            _cacheRepository = cacheRepository;
        }

        public async Task<OrderResponseDto> Add(OrderRequestDto orderRequest, int userId)
        {
            try {
                var guid = Guid.NewGuid();
                OrderResponseDto order = await _repository.Add(userId, orderRequest.bookId, orderRequest.quantity, orderRequest.addressId, guid.ToString());
                await _cacheRepository.DeleteAsync(userId.ToString(), orderRequest.bookId);
                Message message = new Message(new string[] { order.User.Email },
                "Order successfully placed!",
                $"{_emailItems.ItemDetailHtml(order.Book.Title, order.Book.Author, order.Book.Image, order.Book.Price, order.Book.Quantity)+ _emailItems.OrderDetailHtml(order.OrderId, order.OrderedDate, order.Book.Price)}");
                _mqServices.AddToQueue(message);
                return order;
            }
            catch (SqlException e) when(e.Number == SqlErrorNumbers.CONSTRAINT_VOILATION)
            {
                throw new BookstoreException("Invalid user!");
            }
        }
    }
}
