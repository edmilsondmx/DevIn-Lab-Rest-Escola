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
using Microsoft.Extensions.Caching.Memory;
using Escola.Api.Config;
using Microsoft.AspNetCore.JsonPatch;

namespace Escola.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlunosController : ControllerBase
    {
        private readonly IAlunoServico _alunoServico;
        private readonly IMemoryCache _cache;
        private readonly CacheService<AlunoDTO> _alunoCache;
        public AlunosController(
            IAlunoServico alunoServico, 
            IMemoryCache cache, 
            CacheService<AlunoDTO> alunoCache)
        {
            alunoCache.Config("aluno", new TimeSpan(0,2,50));
            _alunoServico = alunoServico;
            _cache = cache;
            _alunoCache = alunoCache;
        }

        [HttpGet]
        public IActionResult BuscarTodos(int skip = 0, int take = 20)
        {
            var uri = $"{Request.Scheme}://{Request.Host}";
            var paginacao = new Paginacao(take, skip);
            var totalRegistros = _alunoServico.ObterTotal();

            Response.Headers.Add("x-Paginacao-TotalRegistros", totalRegistros.ToString());

            //Response.Cookies.Append("TesteCookie", - cria cookie
                        //Newtonsoft.Json.JsonConvert.SerializeObject(paginacao));
            var alunos = new BaseDTO<IList<AlunoDTO>>(){
                Data = _alunoServico.ObterTodos(paginacao).ToList(),
                Links = GetHateoasForAll(uri, take, skip, totalRegistros)
            };
            foreach (var aluno in alunos.Data)
            {
                aluno.Links = GetHateoas(aluno, uri);
            }
            
            return Ok(alunos);
        }
        
        [HttpGet("{id}")]
        public IActionResult BuscarPorId(
            [FromRoute] Guid id
        )
        {
            //var cookie = Request.Cookies["TesteCookie"]; - pega cookie
           // var aluno = _cache.Get<AlunoDTO>($"aluno:{id}");
           var uri = $"{Request.Scheme}://{Request.Host}";
            AlunoDTO aluno;

            if(!_alunoCache.TryGetValue($"{id}", out aluno))
                aluno = _alunoServico.ObterPorId(id);
                _alunoCache.Set($"{id}", aluno);
                aluno.Links = GetHateoas(aluno, uri);
                
            return Ok(aluno);
            
        }

        [HttpPost]
        public IActionResult Inserir(
            [FromBody] AlunoDTO aluno)
        {
            _alunoServico.Inserir(aluno);

            return StatusCode(StatusCodes.Status201Created);
        }
        
        [HttpPut("{id}")]
        public IActionResult Alterar(
            [FromBody] AlunoDTO alunoDto,
            [FromRoute] Guid id
        )
        {
            alunoDto.Id = id;
            _alunoServico.Alterar(alunoDto);
            _alunoCache.Set($"{id}", alunoDto);
            return Ok();
            
        }
        [HttpPatch("{id}")]
        public IActionResult Patch(
            [FromBody] JsonPatchDocument<AlunoDTO> alunoDto,
            [FromRoute] Guid id
        )
        {
            var alunoDb = _alunoServico.ObterPorId(id);

            alunoDto.ApplyTo(alunoDb, ModelState);

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            alunoDb.Id = id;
            _alunoServico.Alterar(alunoDb);
            _alunoCache.Set($"{id}", alunoDb);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Excluir(
            [FromRoute] Guid id
        )
        {
            _alunoServico.Excluir(id);
            _alunoCache.Remover($"{id}");
            return NoContent();
            
        }
        private List<HateoasDTO> GetHateoas(AlunoDTO aluno, string baseUri)
        {
            var hateoas =  new List<HateoasDTO>(){
                new HateoasDTO(){
                    Rel = "self",
                    Type = "Get",
                    URI = $"{baseUri}/api/alunos/{aluno.Id}"
                },
                new HateoasDTO(){
                    Rel = "aluno",
                    Type = "Put",
                    URI = $"{baseUri}/api/alunos/{aluno.Id}"
                },
                new HateoasDTO(){
                    Rel = "aluno",
                    Type = "Delete",
                    URI = $"{baseUri}/api/alunos/{aluno.Id}"
                }
            };
            if((DateTime.Now.Year - aluno.DataNascimento.Year) >= 24)
            {
                hateoas.Add(
                    new HateoasDTO(){
                    Rel = "MatricularAluno",
                    Type = "Post",
                    URI = $"{baseUri}/api/alunos/{aluno.Id}/matricular"
                }
                );
            }
            return hateoas;
        }
        private List<HateoasDTO> GetHateoasForAll( string baseUri, int take, int skip, int ultimo)
        {
            var hateoas =   new List<HateoasDTO>(){
                new HateoasDTO(){
                    Rel = "self",
                    Type = "Get",
                    URI = $"{baseUri}/api/alunos?skip={skip}&take={take}"
                },
                new HateoasDTO(){
                    Rel = "aluno",
                    Type = "Post",
                    URI = $"{baseUri}/api/alunos/"
                }
            };
            var razao = take - skip;
            if(skip != 0)
            {
                var newSkip = skip - razao;
                if(newSkip < 0)
                {
                    newSkip = 0;
                }
                hateoas.Add(new HateoasDTO()
                    {
                        Rel = "prev",
                        Type = "Get",
                        URI = $"{baseUri}/api/alunos?skip={newSkip}&take={take - razao}"
                    }
                );
            }

            if(take < ultimo)
            {
                hateoas.Add(new HateoasDTO()
                    {
                        Rel = "next",
                        Type = "Get",
                        URI = $"{baseUri}/api/alunos?skip={skip + razao}&take={take + razao}"
                    }
                );
            }

            return hateoas;
        }
    }
}