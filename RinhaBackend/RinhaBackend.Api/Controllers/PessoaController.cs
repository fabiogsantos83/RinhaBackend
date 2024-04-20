using Microsoft.AspNetCore.Mvc;
using RinhaBackend.Domain.Commands;
using RinhaBackend.Domain.Interfaces;

namespace RinhaBackend.Api.Controllers
{
    [Route("pessoas")]
    [ApiController]
    public class PessoaController : ControllerBase
    {

        private readonly IPessoaService _service;

        public PessoaController(IPessoaService service)
        {
            _service = service;
        }

        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> AddPessoa([FromBody] AddPessoaRequest request)
        {
            var result = await _service.AddPessoa(request);
            //var result = Guid.NewGuid();
            return Created($"/pessoas/{result}", null);
        }


        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetPessoasResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPessoa([FromRoute] Guid id)
        {
            var result = await _service.GetPessoa(id);

            if (result == null) return NotFound();

            //var result = new GetPessoasResponse();

            return Ok(result);

        }


        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<GetPessoasResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetPessoas([FromQuery] string t)
        {
            var result = await _service.ListPessoas(t);

            if (result == null) return NotFound();

            //var result = new List<GetPessoasResponse>();
            return Ok(result);
        }
    
    }
}
