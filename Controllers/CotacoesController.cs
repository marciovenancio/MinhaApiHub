using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinhaApiHub.Data;

namespace MinhaApiHub.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CotacoesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public CotacoesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/cotacoes
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Cotacao>>> GetCotacoes()
    {
        return await _context.Cotacoes.Include(c => c.Cliente).ToListAsync();
    }

    // GET: api/cotacoes/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Cotacao>> GetCotacao(int id)
    {
        var cotacao = await _context.Cotacoes.Include(c => c.Cliente).FirstOrDefaultAsync(c => c.Id == id);
        if (cotacao == null)
            return NotFound();
        return cotacao;
    }

    // GET: api/cotacoes/dashboard
    [HttpGet("dashboard")]
    public async Task<ActionResult<object>> GetCotacoesDashboard()
    {
        var cotacoes = await _context.Cotacoes.ToListAsync();
        var total = cotacoes.Count;
        var concluidas = cotacoes.Count(c => c.Status.ToLower() == "concluída" || c.Status.ToLower() == "concluida");
        var recusadas = cotacoes.Count(c => c.Status.ToLower() == "recusada" || c.Status.ToLower() == "recusadas");
        var emAndamento = total - concluidas - recusadas;
        return new
        {
            total,
            emAndamento,
            concluidas,
            recusadas
        };
    }

    // POST: api/cotacoes
    [HttpPost]
    public async Task<ActionResult<Cotacao>> PostCotacao(Cotacao cotacao)
    {
        var clienteExiste = await _context.Clientes.AnyAsync(c => c.Id == cotacao.ClienteId);
        if (!clienteExiste)
        {
            return BadRequest($"Cliente com id {cotacao.ClienteId} não existe.");
        }
        cotacao.DataPedido = DateTime.SpecifyKind(cotacao.DataPedido, DateTimeKind.Utc);
        _context.Cotacoes.Add(cotacao);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetCotacao), new { id = cotacao.Id }, cotacao);
    }

    // PUT: api/cotacoes/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCotacao(int id, Cotacao cotacao)
    {
        if (id != cotacao.Id)
            return BadRequest();

        var clienteExiste = await _context.Clientes.AnyAsync(c => c.Id == cotacao.ClienteId);
        if (!clienteExiste)
        {
            return BadRequest($"Cliente com id {cotacao.ClienteId} não existe.");
        }

        cotacao.DataPedido = DateTime.SpecifyKind(cotacao.DataPedido, DateTimeKind.Utc);
        _context.Entry(cotacao).State = EntityState.Modified;
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Cotacoes.Any(e => e.Id == id))
                return NotFound();
            else
                throw;
        }
        return NoContent();
    }

    // DELETE: api/cotacoes/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCotacao(int id)
    {
        var cotacao = await _context.Cotacoes.FindAsync(id);
        if (cotacao == null)
            return NotFound();
        _context.Cotacoes.Remove(cotacao);
        await _context.SaveChangesAsync();
        return NoContent();
    }
} 