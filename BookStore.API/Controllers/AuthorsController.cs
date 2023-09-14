using System.Net;
using AutoMapper;
using BookStore.API.Data;
using BookStore.API.Models.Authors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthorsController : ControllerBase
{
    private readonly BookStoreDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<AuthorsController> _logger;

    public AuthorsController(
        BookStoreDbContext context,
        IMapper mapper,
        ILogger<AuthorsController> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    // GET: api/Authors
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAuthors()
    {
        try
        {
            if (_context.Authors == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<IEnumerable<AuthorDto>>(await _context.Authors.ToListAsync()));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error performing GET {Method}", nameof(GetAuthors));
            return StatusCode((int)HttpStatusCode.InternalServerError, Messages.Error500Client);
        }
    }

    // GET: api/Authors/5
    [HttpGet("{id}")]
    public async Task<ActionResult<AuthorDto>> GetAuthor(int id)
    {
        try
        {
            if (_context.Authors == null)
            {
                return Problem("Entity set 'BookStoreDbContext.Authors' is null.");
            }
            var author = await _context.Authors.FindAsync(id);

            if (author == null)
            {
                _logger.LogWarning("Record not found: GET {Method}(id: {Id})", nameof(GetAuthor), id);
                return NotFound();
            }

            return Ok(_mapper.Map<AuthorDto>(author));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error performing: GET {nameof(GetAuthor)}");
            return StatusCode((int)HttpStatusCode.InternalServerError, Messages.Error500Client);
        }
    }

    // PUT: api/Authors/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutAuthor(int id, AuthorUpdateDto authorDto)
    {
        try
        {
            if (id != authorDto.Id)
            {
                _logger.LogWarning("Update invalid: PUT {Method}(id: {Id})", nameof(PutAuthor), id);
                return BadRequest();
            }

            var author = await _context.Authors.FindAsync(id);
            if (author == null)
            {
                _logger.LogWarning("{Entity} record not found: PUT {Method}(id: {Id})", nameof(Author), nameof(PutAuthor), id);
                return NotFound();
            }

            _mapper.Map(authorDto, author);
            _context.Entry(author).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!await AuthorExistsAsync(id))
                {
                    _logger.LogWarning("Record not found due to concurrency: PUT {Method}(id: {Id})", nameof(PutAuthor), id);
                    return NotFound();
                }
                else
                {
                    _logger.LogError(ex, "Concurrency error: PUT {Method}(id: {Id})", nameof(PutAuthor), id);
                    return StatusCode((int)HttpStatusCode.InternalServerError, Messages.Error500Client);
                }
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error performing PUT {Method}", nameof(PutAuthor));
            return StatusCode((int)HttpStatusCode.InternalServerError, Messages.Error500Client);
        }
    }

    // POST: api/Authors
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<AuthorCreateDto>> PostAuthor(AuthorCreateDto authorDto)
    {
        try
        {
            if (_context.Authors == null)
            {
                return Problem("Entity set 'BookStoreDbContext.Authors' is null.");
            }
            var author = _mapper.Map<Author>(authorDto);
            await _context.Authors.AddAsync(author);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAuthor), new { id = author.Id }, authorDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error performing: POST {nameof(PostAuthor)}");
            return StatusCode((int)HttpStatusCode.InternalServerError, Messages.Error500Client);
        }
    }

    // DELETE: api/Authors/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAuthor(int id)
    {
        try
        {
            if (_context.Authors == null)
            {
                return Problem("Entity set 'BookStoreDbContext.Authors' is null.");
            }
            var author = await _context.Authors.FindAsync(id);
            if (author == null)
            {
                _logger.LogWarning("{Entity} record not found: DELETE {Method}(id: {Id})", nameof(Author), nameof(DeleteAuthor), id);
                return NotFound();
            }

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error performing: DELETE {Mathod}", nameof(DeleteAuthor));
            return StatusCode((int)HttpStatusCode.InternalServerError, Messages.Error500Client);
        }
    }

    private async Task<bool> AuthorExistsAsync(int id)
    {
        return await _context.Authors.AnyAsync(e => e.Id == id);
    }
}
