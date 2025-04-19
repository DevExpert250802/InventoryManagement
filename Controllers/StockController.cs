using InventoryManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Reflection.Metadata.BlobBuilder;

namespace InventoryManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly StockContext _stockContext;

        public StockController(StockContext stockContext)
        {
            _stockContext = stockContext ?? throw new ArgumentNullException(nameof(stockContext));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Stock>>> GetStocks()
        {
            var stocks = await _stockContext.Stocks.ToListAsync();
            return stocks.Any() ? Ok(stocks) : NotFound("No books found");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Stock>> GetBook(int id)
        {
            var book = await _stockContext.Stocks.FindAsync(id);
            return book != null ? Ok(book) : NotFound("Book not found");
        }

        [HttpPost]
        public async Task<ActionResult<Stock>> PostBook([FromBody] Stock book)
        {
            if (book == null)
            {
                return BadRequest("Invalid book data");
            }
            _stockContext.Stocks.Add(book);
            await _stockContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetBook), new { id = book.ID }, book);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, Stock book)
        {
            if (id != book.ID)
            {
                return BadRequest("Book ID mismatch");
            }

            _stockContext.Entry(book).State = EntityState.Modified;

            try
            {
                await _stockContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_stockContext.Stocks.Any(b => b.ID == id))
                {
                    return NotFound("Book not found for update");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _stockContext.Stocks.FindAsync(id);
            if (book == null)
            {
                return NotFound("Book not found");
            }

            _stockContext.Stocks.Remove(book);
            await _stockContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
