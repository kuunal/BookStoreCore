using AutoMapper;
using BusinessLayer.Exceptions;
using BusinessLayer.Interface;
using ModelLayer.BookDto;
using RepositoryLayer.Implementation;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Implementation
{
    public class BookService : IBookService
    {
        private readonly IBooksRepository _repository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookService"/> class.
        /// </summary>
        /// <param name="repository">The book repository object.</param>
        /// <param name="mapper">The automapper object for mapping.</param>
        public BookService(IBooksRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        /// <summary>
        /// Performs business logic while adding book.
        /// </summary>
        /// <param name="requestDto">The request dto.</param>
        /// <returns>BookResponseDto object</returns>
        /// <exception cref="BookstoreException">Invalid data</exception>
        public async Task<BookResponseDto> AddBook(BookRequestDto requestDto)
        {
            try
            {
                int id = await _repository.Insert(requestDto);
                BookResponseDto response = _mapper.Map<BookResponseDto>(requestDto);
                response.Id = id;
                return response;

            } catch (SqlException e) when (e.Number == SqlErrorNumbers.CONSTRAINT_VOILATION)
            {
                throw new BookstoreException("Invalid data");
            }
        }

        /// <summary>
        /// Performs business logic while retrieving single book.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>book information</returns>
        public async Task<BookResponseDto> Get(int id)
        {
            return await _repository.Get(id);
        }

        /// <summary>
        /// Performs business logic while retrieving the books.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="limit">The limit.</param>
        /// <param name="lastItemValue">The last item value.</param>
        /// <param name="sortby">The sortby.</param>
        /// <returns>book information</returns>
        public Task<List<BookResponseDto>> GetBooks(string field, int limit, string lastItemValue, string sortby)
        {
            PropertyInfo[] properties = typeof(BookRequestDto).GetProperties();
            if (!properties.Any(property => property.Name.ToLower().Equals(field.ToLower())))
            {
                field = "author";
            }
            if(sortby!="asc" && sortby != "desc")
            {
                sortby = "asc";
            }
            return _repository.Get(field, limit, lastItemValue, sortby);
        }

        /// <summary>
        /// Performs business logic while retrieving Deleting the specified book.
        /// </summary>
        /// <param name="id">The book identifier.</param>
        /// <returns>booolean status in 1/0 format</returns>
        public async Task<int> Delete(int id)
        {
            return await _repository.Delete(id);
        }


        /// <summary>
        /// Performs business logic while updating the specified book.
        /// </summary>
        /// <param name="id">The book identifier.</param>
        /// <param name="requestDto">The request dto.</param>
        /// <returns>book information</returns>
        /// <exception cref="BookstoreException">Invalid data</exception>
        public async Task<BookResponseDto> Update(int id, BookRequestDto requestDto)
        {
            try
            {
                return await _repository.Update(id, requestDto);
            } catch (SqlException e) when (e.Number == SqlErrorNumbers.CONSTRAINT_VOILATION)
            {
                throw new BookstoreException("Invalid data");
            }
        }


        public async Task<int> GetTotalNumberOfBooks()
        {
            return await _repository.GetTotal();
        }
    }
}
