using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Escola.Domain.DTO;
using Escola.Domain.Models;
using Escola.Domain.Interfaces.Repositories;
using Escola.Domain.Interfaces.Services;

namespace Escola.Domain.Services
{
    public class AlunoServico : IAlunoServico
    {
        private readonly IAlunoRepositorio _alunoRepositorio;
        public AlunoServico(IAlunoRepositorio alunoRepositorio)
        {
            _alunoRepositorio = alunoRepositorio;
        }

        public void Alterar(AlunoDTO aluno, Guid id)
        {
            Aluno alunoExistente = _alunoRepositorio.ObterPorId(id);

            alunoExistente.Nome = aluno.Nome;
            alunoExistente.Sobrenome = aluno.Sobrenome;
            alunoExistente.Email = aluno.Email;
            alunoExistente.DataNascimento = aluno.DataNascimento;

            _alunoRepositorio.Alterar(alunoExistente);
        }

        public void Excluir(Guid id)
        {
            var aluno = _alunoRepositorio.ObterPorId(id);
            _alunoRepositorio.Excluir(aluno);
        }

        public void Inserir(AlunoDTO aluno)
        {
            //ToDo: Validar se já consta matricula.

            _alunoRepositorio.Inserir(new Aluno(aluno));
        }

        public AlunoDTO ObterPorId(Guid id)
        {
            var aluno = _alunoRepositorio.ObterPorId(id);

            return new AlunoDTO(aluno);
        }

        public IList<AlunoDTO> ObterTodos()
        {   
            var listaAlunos = _alunoRepositorio.ObterTodos().ToArray();

            List<AlunoDTO> listaAlunosDto = new List<AlunoDTO>();

            for (int i = 0; i < listaAlunos.Length ; i++)
            {
                listaAlunosDto.Add(new AlunoDTO(listaAlunos[i]));
            }

            return listaAlunosDto;
        }
    }
}