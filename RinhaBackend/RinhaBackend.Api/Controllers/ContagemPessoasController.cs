using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RinhaBackend.Domain.Interfaces;

namespace RinhaBackend.Api.Controllers
{
    [Route("contagem-pessoas")]
    [ApiController]
    public class ContagemPessoasController : ControllerBase
    {
        private readonly IPessoaService _service;

        public ContagemPessoasController(IPessoaService service)
        {
            _service = service;
        }

        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetContagemPessoas()
        {
            var result = await _service.GetQuantidadePessoas();

            //var result = 0;
            return Ok(result);

        }
    }

}
