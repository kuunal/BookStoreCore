using BusinessLayer.Interface;
using Greeting.TokenAuthorization;
using Microsoft.AspNetCore.Mvc;
using ModelLayer;
using ModelLayer.BookDto;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace BookStore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookController : ControllerBase
    {

        private readonly IBookService _service;

        /// <summary>
        /// Getting object of BookService using DI
        /// </summary>
        /// <param name="service">BookService object for business logic</param>
        public BookController(IBookService service)
        {
            _service = service;
        }

        /// <summary>
        /// Endpoint for adding new books. Only admins are authorized to add.
        /// </summary>
        /// <param name="requestDto"> Required body in form of BookRequestDto object</param>
        /// <returns>readystate and data</returns>
        [HttpPost]
        [TokenAuthorizationFilter("admin")]
        public async Task<IActionResult> AddBook(BookRequestDto requestDto)
        {
            BookResponseDto responseDto = await _service.AddBook(requestDto);
            return Ok(new Response<BookResponseDto> { 
                StatusCode = (int) HttpStatusCode.OK,
                Message = ResponseMessage.BOOK_ADDED,
                Data = responseDto
            });
        }

        /// <summary>
        /// Endpoint for get books based on filters.
        /// </summary>
        /// <param name="field">field to be sorted on</param>
        /// <param name="limit">Number of items to be retrieved</param>
        /// <param name="lastItemValue">Last value of the previous request</param>
        /// <param name="sortby">ascending or descending</param>
        /// <returns>readystate and data</returns>
        [HttpGet]
        [TokenAuthorizationFilter]
        public async Task<IActionResult> GetAllBooks(string field, int limit, string lastItemValue, string sortby)
        {
            List<BookResponseDto> bookList = await _service.GetBooks(field, limit, lastItemValue, sortby);
            return Ok(new Response<List<BookResponseDto>>
            {
                StatusCode = (int) HttpStatusCode.OK,
                Message = ResponseMessage.SUCCESSFUL,
                Data = bookList
            });
        }

        /// <summary>
        /// Endpoint for detailed view for book
        /// </summary>
        /// <param name="id">Book id</param>
        /// <returns>readystate and data</returns>
        [HttpGet("{id}")]
        [TokenAuthorizationFilter]
        public async Task<IActionResult> GetBook(int id)
        {
            BookResponseDto book = await _service.Get(id);
            if (book == null)
            {
                return NotFound(new Response<BookResponseDto>
                {
                    StatusCode = (int)HttpStatusCode.NotFound,
                    Message = ResponseMessage.BOOK_NOT_FOUND,
                    Data = book
                });
            }
            return Ok(new Response<BookResponseDto>
            {
                StatusCode = (int)HttpStatusCode.OK,
                Message = ResponseMessage.SUCCESSFUL,
                Data = book
            });
        }

        /// <summary>
        /// Endpoint for deleting book. Only admiins are authorized to delete books
        /// </summary>
        /// <param name="id"></param>
        /// <returns>readystate or 404</returns>
        [HttpDelete("{id}")]
        [TokenAuthorizationFilter("admin")]
        public async Task<IActionResult> RemoveBook(int id)
        {
            int isDeleted = await _service.Delete(id);
            if (isDeleted <= 0)
            {
                return NotFound(new Response<object>
                {
                    StatusCode = (int)HttpStatusCode.NotFound,
                    Message = ResponseMessage.BOOK_REMOVE_FAILED,
                    Data = null
                });
            }
            return Ok(new Response<object>
            {
                StatusCode = (int)HttpStatusCode.OK,
                Message = ResponseMessage.BOOK_REMOVE,
                Data = null
            });
        }

        /// <summary>
        /// Endpoint for updating book. Only admiins are authorized to delete books
        /// </summary>
        /// <param name="id"></param>
        /// <returns>readystate and updated data or 404</returns>
        [HttpPost("{id}")]
        [TokenAuthorizationFilter("admin")]
        public async Task<IActionResult> UpdateBook(int id, BookRequestDto requestDto)
        {
            BookResponseDto book = await _service.Update(id, requestDto);
            if (book == null)
            {
                return NotFound(new Response<BookResponseDto>
                {
                    StatusCode = (int)HttpStatusCode.NotFound,
                    Message = ResponseMessage.BOOK_NOT_FOUND,
                    Data = book
                });
            }
            return Ok(new Response<BookResponseDto>
            {
                StatusCode = (int)HttpStatusCode.OK,
                Message = ResponseMessage.BOOK_UPDATED,
                Data = book
            });
        }

        [HttpGet]
        [Route("total")]
        public async Task<int> GetTotalNumberOfBooks()
        {
            return await _service.GetTotalNumberOfBooks();
        }
        
    }
}
