using Microsoft.AspNetCore.Mvc;
using RinhaBackend.Domain.Cliente;

namespace RinhaBackend.Controllers
{
    [Route("clientes")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        [HttpPost("{id}/transacoes")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(AddTransacaoResponse))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public IActionResult AddTransacao([FromRoute] int id, [FromBody] AddTransacaoRequest request)
        {
            return Created("/1/extrato/", new AddTransacaoResponse(1, 1));
        }


        [HttpGet("{id}/extrato")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(GetExtratoResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetExtrato([FromRoute] int id)
        {
            return Ok(new GetExtratoResponse()
            {
                Saldo = new GetExtratoSaldoResponse()
                {
                    DataExtrato = new DateTime(),
                    Limite = 1,
                    Total = 1
                }
            });

        }
    }
}
