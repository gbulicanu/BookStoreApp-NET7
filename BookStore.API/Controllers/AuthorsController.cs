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

    public AuthorsController(BookStoreDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    // GET: api/Authors
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAuthors()
    {
        if (_context.Authors == null)
        {
            return NotFound();
        }
        return Ok(_mapper.Map<IEnumerable<AuthorDto>>(await _context.Authors.ToListAsync()));
    }

    // GET: api/Authors/5
    [HttpGet("{id}")]
    public async Task<ActionResult<AuthorDto>> GetAuthor(int id)
    {
      if (_context.Authors == null)
      {
          return NotFound();
      }
        var author = await _context.Authors.FindAsync(id);

        if (author == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<AuthorDto>(author));
    }

    // PUT: api/Authors/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutAuthor(int id, AuthorUpdateDto authorDto)
    {
        if (id != authorDto.Id)
        {
            return BadRequest();
        }

        var author = await _context.Authors.FindAsync(id);
        if (author == null)
        {
            return NotFound();
        }

        _mapper.Map(authorDto, author);
        _context.Entry(author).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await AuthorExistsAsync(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/Authors
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<AuthorCreateDto>> PostAuthor(AuthorCreateDto authorDto)
    {
        if (_context.Authors == null)
        {
            return Problem("Entity set 'BookStoreDbContext.Authors'  is null.");
        }
        var author = _mapper.Map<Author>(authorDto);
        await _context.Authors.AddAsync(author);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAuthor), new { id = author.Id }, authorDto);
    }

    // DELETE: api/Authors/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAuthor(int id)
    {
        if (_context.Authors == null)
        {
            return NotFound();
        }
        var author = await _context.Authors.FindAsync(id);
        if (author == null)
        {
            return NotFound();
        }

        _context.Authors.Remove(author);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> AuthorExistsAsync(int id)
    {
        return await _context.Authors.AnyAsync(e => e.Id == id);
    }
}
