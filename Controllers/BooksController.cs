﻿using WebAPIwithMongoDB.Models;
using WebAPIwithMongoDB.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebAPIwithMongoDB.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly BooksService _booksService;

        public BooksController(BooksService booksService) =>
            _booksService = booksService;

        // Routes

        [HttpGet]
        public async Task<List<Book>> Get()
        {
            return await _booksService.GetAsync();
        }

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Book>> Get(string id)
        {
            var book = await _booksService.GetAsync(id);
            if (book == null) return NotFound();
            return book;
        }

        [HttpPost]
        public async Task<ActionResult<Book>> Post(Book newBook)
        {
            await _booksService.CreateAsync(newBook);
            return CreatedAtAction(
                nameof(Get),
                new { id = newBook.Id },
                newBook
            );
        }

        [HttpPut("{id:length(24)}")]
        public async Task<ActionResult<Book>> Put(string id, Book updatedBook)
        {
            var book = await _booksService.GetAsync(id);
            if (book == null) return NotFound();

            await _booksService.UpdateSync(id, updatedBook);
            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<ActionResult> Delete(string id)
        {
            var book = _booksService.GetAsync(id);
            if (book == null) return NotFound();

            await _booksService.RemoveAsync(id);
            return Ok("Deleted.");
        }
    }
}
