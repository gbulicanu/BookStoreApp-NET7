using System.Net;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BookStore.API.Data;
using BookStore.API.Models.Books;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    private readonly BookStoreDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<BooksController> _logger;

    public BooksController(
        BookStoreDbContext context,
        IMapper mapper,
        ILogger<BooksController> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    // GET: api/Books
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookDto>>> GetBooks()
    {
        try
        {
            if (_context.Books == null)
            {
                return NotFound();
            }
            var booksDtos = await _context.Books
                .Include(x => x.Author)
                .ProjectTo<BookDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
            return Ok(booksDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error performing GET {Method}",
                nameof(GetBooks));
            return StatusCode(
                (int)HttpStatusCode.InternalServerError,
                Messages.Error500Client);
        }
    }

    // GET: api/Books/5
    [HttpGet("{id}")]
    public async Task<ActionResult<BookDto>> GetBook(int id)
    {
        try
        {
            if (_context.Books == null)
            {
                return Problem("Entity set 'BookStoreDbContext.Books' is null.");
            }
            var bookDto = await _context.Books
                .Include(x => x.Author)
                .ProjectTo<BookDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (bookDto == null)
            {
                _logger.LogWarning(
                    "Record not found: GET {Method}(id: {Id})",
                    nameof(GetBook),
                    id);
                return NotFound();
            }

            return Ok(bookDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error performing: GET {nameof(GetBook)}");
            return StatusCode(
                (int)HttpStatusCode.InternalServerError,
                Messages.Error500Client);
        }
    }

    // PUT: api/Books/5
    // To protect from over posting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutBook(int id, BookUpdateDto bookDto)
    {
        try
        {
            if (id != bookDto.Id)
            {
                _logger.LogWarning(
                    "Update invalid: PUT {Method}(id: {Id})",
                    nameof(PutBook), id);
                return BadRequest();
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                _logger.LogWarning(
                    "{Entity} record not found: PUT {Method}(id: {Id})",
                    nameof(Book),
                    nameof(PutBook),
                    id);
                return NotFound();
            }

            _mapper.Map(bookDto, book);
            _context.Entry(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!await BookExistsAsync(id))
                {
                    _logger.LogWarning(
                        "Record not found due to concurrency: PUT {Method}(id: {Id})",
                        nameof(PutBook),
                        id);
                    return NotFound();
                }
                else
                {
                    _logger.LogError(
                        ex,
                        "Concurrency error: PUT {Method}(id: {Id})",
                        nameof(PutBook),
                        id);
                    return StatusCode(
                        (int)HttpStatusCode.InternalServerError,
                        Messages.Error500Client);
                }
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error performing PUT {Method}",
                nameof(PutBook));
            return StatusCode(
                (int)HttpStatusCode.InternalServerError,
                Messages.Error500Client);
        }
    }

    // POST: api/Books
    // To protect from over posting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<BookCreateDto>> PostBook(BookCreateDto bookDto)
    {
        try
        {
            if (_context.Books == null)
            {
                return Problem("Entity set 'BookStoreDbContext.Books' is null.");
            }
            var book = _mapper.Map<Book>(bookDto);
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetBook),
                new { id = book.Id },
                bookDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error performing: POST {nameof(PostBook)}");
            return StatusCode(
                (int)HttpStatusCode.InternalServerError,
                Messages.Error500Client);
        }
    }

    // DELETE: api/Books/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBook(int id)
    {
        try
        {
            if (_context.Books == null)
            {
                return Problem("Entity set 'BookStoreDbContext.Books' is null.");
            }
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                _logger.LogWarning(
                    "{Entity} record not found: DELETE {Method}(id: {Id})",
                    nameof(Book),
                    nameof(DeleteBook),
                    id);
                return NotFound();
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error performing: DELETE {Method}",
                nameof(DeleteBook));
            return StatusCode(
                (int)HttpStatusCode.InternalServerError,
                Messages.Error500Client);
        }
    }

    private async Task<bool> BookExistsAsync(int id)
    {
        return await _context.Books.AnyAsync(e => e.Id == id);
    }
}
