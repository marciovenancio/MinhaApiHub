using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinhaApiHub.Data;

namespace MinhaApiHub.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ComissoesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ComissoesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/comissoes
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Comissao>>> GetComissoes()
    {
        return await _context.Comissoes.Include(c => c.Cliente).ToListAsync();
    }

    // GET: api/comissoes/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Comissao>> GetComissao(int id)
    {
        var comissao = await _context.Comissoes.Include(c => c.Cliente).FirstOrDefaultAsync(c => c.Id == id);
        if (comissao == null)
            return NotFound();
        return comissao;
    }

    // GET: api/comissoes/dashboard
    [HttpGet("dashboard")]
    public async Task<ActionResult<object>> GetComissoesDashboard()
    {
        var comissoes = await _context.Comissoes.ToListAsync();
        var totalComissoes = comissoes.Sum(c => c.ComissaoValor);
        var comissoesRecebidas = comissoes.Where(c => c.Status.ToLower() == "pago").Sum(c => c.ComissaoValor);
        var aReceber = totalComissoes - comissoesRecebidas;
        return new
        {
            totalComissoes,
            comissoesRecebidas,
            aReceber
        };
    }

    // POST: api/comissoes
    [HttpPost]
    public async Task<ActionResult<Comissao>> PostComissao(Comissao comissao)
    {
        var clienteExiste = await _context.Clientes.AnyAsync(c => c.Id == comissao.ClienteId);
        if (!clienteExiste)
        {
            return BadRequest($"Cliente com id {comissao.ClienteId} não existe.");
        }
        comissao.Data = DateTime.SpecifyKind(comissao.Data, DateTimeKind.Utc);
        _context.Comissoes.Add(comissao);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetComissao), new { id = comissao.Id }, comissao);
    }

    // PUT: api/comissoes/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> PutComissao(int id, Comissao comissao)
    {
        if (id != comissao.Id)
            return BadRequest();
        var clienteExiste = await _context.Clientes.AnyAsync(c => c.Id == comissao.ClienteId);
        if (!clienteExiste)
        {
            return BadRequest($"Cliente com id {comissao.ClienteId} não existe.");
        }
        comissao.Data = DateTime.SpecifyKind(comissao.Data, DateTimeKind.Utc);
        _context.Entry(comissao).State = EntityState.Modified;
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Comissoes.Any(e => e.Id == id))
                return NotFound();
            else
                throw;
        }
        return NoContent();
    }

    // DELETE: api/comissoes/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteComissao(int id)
    {
        var comissao = await _context.Comissoes.FindAsync(id);
        if (comissao == null)
            return NotFound();
        _context.Comissoes.Remove(comissao);
        await _context.SaveChangesAsync();
        return NoContent();
    }
} 