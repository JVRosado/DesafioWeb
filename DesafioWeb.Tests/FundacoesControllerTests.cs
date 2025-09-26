using DesafioWeb.Controllers;
using DesafioWeb.Data;
using DesafioWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace DesafioWeb.Tests
{
    public class FundacoesControllerTests
    {
        // Método auxiliar para criar uma fundação válida para os testes
        private Fundacao CriarFundacaoValida() => new Fundacao
        {
            Nome = "Fundação Teste",
            CNPJ = "27865757000102",
            Email = "contato@teste.org",
            Telefone = "11999999999",
            InstituicaoApoiada = "Instituição X"
        };

        // Testa se ao criar uma fundação com CNPJ inválido, a mensagem de erro é exibida
        [Fact]
        public void CriarFundacao_CnpjInvalido_DeveMostrarMensagemErro()
        {
            
            var mockDb = new Mock<IDatabase>();
            var controller = new FundacoesController(mockDb.Object);
            var fundacao = CriarFundacaoValida();
            fundacao.CNPJ = "12345678901234";

            
            var result = controller.Create(fundacao) as ViewResult;

            
            Assert.NotNull(result);
            Assert.True(controller.ViewBag.Mensagem.ToString()!.Contains("inválido"));
            mockDb.Verify(db => db.Inserir(It.IsAny<Fundacao>()), Times.Never);
        }

        // Testa se ao criar uma fundação com CNPJ já existente, a mensagem de duplicado é exibida
        [Fact]
        public void CriarFundacao_CnpjDuplicado_DeveMostrarMensagemErro()
        {
            
            var mockDb = new Mock<IDatabase>();
            var existente = CriarFundacaoValida();
            mockDb.Setup(db => db.BuscarPorCNPJ(existente.CNPJ)).Returns(existente);
            var controller = new FundacoesController(mockDb.Object);

            
            var result = controller.Create(existente) as ViewResult;

            
            Assert.NotNull(result);
            Assert.Contains("já está cadastrado", controller.ViewBag.Mensagem.ToString()!);
            mockDb.Verify(db => db.Inserir(It.IsAny<Fundacao>()), Times.Never);
        }

        // Testa se ao criar uma fundação válida, a função Inserir é chamada e a mensagem de sucesso é exibida
        [Fact]
        public void CriarFundacao_Valida_DeveInserirEDarMensagemSucesso()
        {
            
            var mockDb = new Mock<IDatabase>();
            mockDb.Setup(db => db.BuscarPorCNPJ(It.IsAny<string>())).Returns((Fundacao?)null);
            var controller = new FundacoesController(mockDb.Object);
            var fundacao = CriarFundacaoValida();

            
            var result = controller.Create(fundacao) as ViewResult;

            
            Assert.NotNull(result);
            Assert.Equal("Cadastro efetuado com sucesso!", controller.ViewBag.Mensagem);
            mockDb.Verify(db => db.Inserir(It.Is<Fundacao>(f => f.CNPJ == fundacao.CNPJ)), Times.Once);
        }

        // Testa se ao buscar uma fundação para edição que não existe, é exibida a mensagem "Fundação não encontrada"
        [Fact]
        public void BuscarFundacaoParaEdicao_NaoEncontrada_DeveMostrarMensagem()
        {
            
            var mockDb = new Mock<IDatabase>();
            mockDb.Setup(db => db.BuscarPorCNPJ("00000000000000")).Returns((Fundacao?)null);
            var controller = new FundacoesController(mockDb.Object);

            
            var result = controller.BuscarFundacaoParaEdicao("00000000000000") as ViewResult;

            
            Assert.NotNull(result);
            Assert.Equal("Edit", result.ViewName); 
            Assert.Equal("Fundação não encontrada.", controller.ViewBag.Mensagem);
        }

        [Fact]
        public void BuscarFundacaoParaEdicao_Encontrada_DeveRetornarModelo()
        {
            
            var mockDb = new Mock<IDatabase>();
            var fundacao = CriarFundacaoValida();
            mockDb.Setup(db => db.BuscarPorCNPJ(fundacao.CNPJ)).Returns(fundacao);
            var controller = new FundacoesController(mockDb.Object);

            
            var result = controller.BuscarFundacaoParaEdicao(fundacao.CNPJ) as ViewResult;

            
            Assert.NotNull(result);
            Assert.Equal("Edit", result.ViewName);
            Assert.Same(fundacao, result.Model);
        }
    }
}

