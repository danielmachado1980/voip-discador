[ApiController]
[Route("api/[controller]")]
public class DiscadorController : ControllerBase
{
    private readonly DiscadorService _discador;
    public DiscadorController(DiscadorService discador) => _discador = discador;

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] ListaNumerosRequest req)
    {
        var num = await _discador.DiscarNumerosAsync(req.Numeros);
        return Ok(new { NumeroAtendido = num });
    }
}
