using BusinessLayer.Interface;
using Microsoft.AspNetCore.Mvc;
using ModelLayer;
using ModelLayer.BookDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BookStore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookController : ControllerBase
    {
        private readonly IBookService _service;

        public BookController(IBookService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> AddBook(BookRequestDto requestDto)
        {
            BookResponseDto responseDto = await _service.AddBook(requestDto);
            return Ok(new Response<BookResponseDto> { 
                StatusCode = (int) HttpStatusCode.OK,
                Message = ResponseMessage.BOOK_ADDED,
                Data = responseDto
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBooks()
        {
            List<BookResponseDto> bookList = await _service.GetBooks();
            return Ok(new Response<List<BookResponseDto>>
            {
                StatusCode = (int) HttpStatusCode.OK,
                Message = ResponseMessage.SUCCESSFUL,
                Data = bookList
            });
        }
    }
}
