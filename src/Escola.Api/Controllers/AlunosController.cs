using System.Diagnostics.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Escola.Domain.DTO;
using Escola.Domain.Interfaces.Services;

namespace Escola.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlunosController : ControllerBase
    {
        private readonly IAlunoServico _alunoServico;
        public AlunosController(IAlunoServico alunoServico)
        {
            _alunoServico = alunoServico;
        }
        [HttpGet]
        public IActionResult BuscarTodos()
        {
            try
            {
               return Ok(_alunoServico.ObterTodos().ToList());
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        
        [HttpGet("id")]
        public IActionResult BuscarPorId(
            [FromRoute] Guid id
        )
        {
            try
            {
                return Ok(_alunoServico.ObterPorId(id));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            } 
        }

        [HttpPost]
        public IActionResult Inserir(
            [FromBody] AlunoDTO aluno)
        {
            try{
                _alunoServico.Inserir(aluno);
            }
            catch{
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Created("api/aluno", aluno);
        }
        
        [HttpPut("id")]
        public IActionResult Alterar(
            [FromBody] AlunoDTO alunoDto,
            [FromRoute] Guid id
        )
        {
            try
            {
                alunoDto.Id = id;
                _alunoServico.Alterar(alunoDto);
            }
            catch
            {
                
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Ok();
        }

        [HttpDelete("id")]
        public IActionResult Excluir(
            [FromRoute] Guid id
        )
        {
            try
            {
                _alunoServico.Excluir(id);
                return NoContent();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}