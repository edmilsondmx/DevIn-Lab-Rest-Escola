using System.Diagnostics.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Escola.Domain.DTO;
using Escola.Domain.Interfaces.Services;
using Escola.Domain.Exceptions;
using Escola.Domain.Models;
using AutoMapper;

namespace Escola.Api.Controllers
{
    [ApiController]
    [Route("api/v2/alunos")]
    public class AlunosV2Controller : ControllerBase
    {
        private readonly IMapper  _mapper;
        private readonly IAlunoServico _alunoServico;
        public AlunosV2Controller(IAlunoServico alunoServico, IMapper  mapper)
        {
            _alunoServico = alunoServico;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult BuscarTodos(int skip = 0, int take = 20)
        {
            try
            {
                var paginacao = new Paginacao(take, skip);
                var totalRegistros = _alunoServico.ObterTotal();

                Response.Headers.Add("x-Paginacao-TotalRegistros", totalRegistros.ToString());
                return Ok(_mapper.Map<IList<AlunoV2DTO>>(_alunoServico.ObterTodos(paginacao).ToList()));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        
        [HttpGet("{id}")]
        public IActionResult BuscarPorId(
            [FromRoute] Guid id
        )
        {
            try
            {
                return Ok(_mapper.Map<AlunoV2DTO>(_alunoServico.ObterPorId(id)));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            } 
        }

        [HttpPost]
        public IActionResult Inserir(
            [FromBody] AlunoV2DTO aluno)
        {
            _alunoServico.Inserir(_mapper.Map<AlunoDTO>(aluno));

            return StatusCode(StatusCodes.Status201Created);
        }
        
        [HttpPut("{id}")]
        public IActionResult Alterar(
            [FromBody] AlunoV2DTO aluno,
            [FromRoute] Guid id
        )
        {
            try
            {
                aluno.Id = id;
                _alunoServico.Alterar(_mapper.Map<AlunoDTO>(aluno));
            }
            catch
            {
                
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Ok();
        }

        [HttpDelete("{id}")]
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