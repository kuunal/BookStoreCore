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

        [HttpGet("id")]
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

        [HttpDelete("id")]
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


        [HttpPost("id")]
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

    }
}
