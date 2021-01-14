using BusinessLayer.Exceptions;
using BusinessLayer.Interface;
using BusinessLayer.MQServices;
using BusinessLayer.Utility;
using Caching;
using EmailService;
using ModelLayer.CartDto;
using ModelLayer.OrderDto;
using RepositoryLayer.Implementation;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Implementation
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _repository;
        private readonly IOrderRepository _orderRepository;
        private readonly IMqServices _mqservice;

        public IMqServices IMqServices { get; }

        private readonly IEmailItemDetails _emailItems;
        private readonly ICacheRepository _cacheRepository;

        public CartService(ICartRepository repository
            , IOrderRepository orderRepository
            , IMqServices mqServices
            , IEmailItemDetails details
            , ICacheRepository cacheRepository)
        {
            _repository = repository;
            _orderRepository = orderRepository;
            _mqservice = mqServices;
            _emailItems = details;
            _cacheRepository = cacheRepository;
        }

        /// <summary>
        /// Inserts the specified cart.
        /// </summary>
        /// <param name="cart">The cart.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>Added item info</returns>
        /// <exception cref="BookstoreException">
        /// Invalid user id!
        /// or
        /// Already in cart
        /// </exception>
        public async Task<CartResponseDto> Insert(CartRequestDto cart, int userId)
        {
            try
            {
                CartResponseDto responseDto = await _repository.Insert(cart, userId);
                await _cacheRepository.AddAsync(userId.ToString(), responseDto);
                return responseDto;
            }catch(SqlException e) when(e.Number == SqlErrorNumbers.CONSTRAINT_VOILATION)
            {
                throw new BookstoreException("Invalid user id!");
            }
            catch (SqlException e) when (e.Number == SqlErrorNumbers.DUPLICATEKEY)
            {
                throw new BookstoreException("Already in cart");
            }
        }

        /// <summary>
        /// Deletes the specified book identifier from  cart.
        /// </summary>
        /// <param name="bookId">The book identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>Boolean result in 1/0 format</returns>
        public async Task<int> Delete(int bookId, int userId)
        {
            int result = await _repository.Delete(bookId, userId);
            await _cacheRepository.DeleteAsync(userId.ToString(), bookId);
            return result;
        }

        /// <summary>
        /// Updates the specified cart.
        /// </summary>
        /// <param name="cart">The cart.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>Updated cart item infomartion</returns>
        public async Task<CartResponseDto> Update(CartRequestDto cart, int userId)
        {
            try
            {
                CartResponseDto responseDto = await _repository.Update(cart, userId);
                await _cacheRepository.UpdateAsync(userId.ToString(), responseDto, responseDto.Book.Id);
                return responseDto;
            }
            catch (SqlException e) when (e.Number == 50000)
            {
                throw new BookstoreException(e.Message);
            }
        }

        /// <summary>
        /// Gets the cart using specified user identifier.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public async Task<CartDetailedResponseDto> Get(int userId)
        {
            List<CartResponseDto> carts = await _repository.Get(userId);
            CartDetailedResponseDto responseDto = new CartDetailedResponseDto
            {
                cartItems = carts,
                Total = carts.Aggregate(0, (seed, item) => item.Cost + seed)
            };
            await _cacheRepository.Get(userId.ToString(), responseDto);
            return responseDto;
        }
        
        public delegate void callbackDelegate(List<OrderResponseDto> orders, List<CartResponseDto> cartItems, string guid, int userId);

        public async Task<List<OrderResponseDto>> PlaceOrder(int userId, int addressId)
        {
            var cartItems = await _repository.Get(userId);
            string guid = Guid.NewGuid().ToString();
            List<OrderResponseDto> orders = new List<OrderResponseDto>();
            //orders.AddRange(await Task.WhenAll(cartItems.Select(item =>
            //   _orderRepository.Add(userId, item.Book.Id, item.ItemQuantity, addressId, guid
            //    )))     
            cartItems.ForEach(async (item)=> {
                await AddToListAsync(orders, userId, item.Book.Id, item.ItemQuantity, addressId, guid);
                if(orders.Count == cartItems.Count)
                {
                    //callbackDelegate @delegate = new callbackDelegate(callback);
                    callback(orders, cartItems, guid, userId);
                }
            });  
            return orders;
        }   

        public async Task AddToListAsync(List<OrderResponseDto> orders, int userId, int bookId, int ItemQuantity, int addressId, string guid)
        {
            orders.Add(
               await _orderRepository.Add(userId, bookId, ItemQuantity, addressId, guid));
        }

        public void callback(List<OrderResponseDto> orders, List<CartResponseDto> cartItems, string guid, int userId)
        {
            string emailHtmlMessage = "";
            orders.ForEach(item =>
               emailHtmlMessage += _emailItems.ItemDetailHtml(item.Book.Title, item.Book.Author, item.Book.Image, item.Book.Price, item.Book.Quantity)
           );
            int total = cartItems.Aggregate(0, (sum, item) =>
                sum + (item.Book.Price * item.ItemQuantity)
            );
            string orderDetails = _emailItems.OrderDetailHtml(guid, orders[0].OrderedDate, total);
            Message message = new Message(new string[] { "kunaldeshmukh2503@gmail.com" },
                "Order successfully placed!",
                $"{emailHtmlMessage + orderDetails}");
            cartItems.ForEach(item => _cacheRepository.DeleteAsync(userId.ToString(), item.Book.Id));
            _mqservice.AddToQueue(message);
        }
    }
}
