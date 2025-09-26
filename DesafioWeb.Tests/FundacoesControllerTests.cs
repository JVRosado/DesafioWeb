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
        private Fundacao BuildValidFundacao() => new Fundacao
        {
            Nome = "Fundação Teste",
            CNPJ = "27865757000102", // CNPJ válido de exemplo
            Email = "contato@teste.org",
            Telefone = "11999999999",
            InstituicaoApoiada = "Instituição X"
        };

        [Fact]
        public void Create_InvalidCnpj_ShowsMensagemErro()
        {
            // Arrange
            var mockDb = new Mock<IDatabase>();
            var controller = new FundacoesController(mockDb.Object);
            var fundacao = BuildValidFundacao();
            fundacao.CNPJ = "12345678901234"; // CNPJ inválido (verificação dígitos)

            // Act
            var result = controller.Create(fundacao) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.True(controller.ViewBag.Mensagem.ToString()!.Contains("inválido"));
            mockDb.Verify(db => db.Inserir(It.IsAny<Fundacao>()), Times.Never);
        }

        [Fact]
        public void Create_DuplicateCnpj_ShowsMensagemDuplicado()
        {
            // Arrange
            var mockDb = new Mock<IDatabase>();
            var existente = BuildValidFundacao();
            mockDb.Setup(db => db.BuscarPorCNPJ(existente.CNPJ)).Returns(existente);
            var controller = new FundacoesController(mockDb.Object);

            // Act
            var result = controller.Create(existente) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Contains("já está cadastrado", controller.ViewBag.Mensagem.ToString()!);
            mockDb.Verify(db => db.Inserir(It.IsAny<Fundacao>()), Times.Never);
        }

        [Fact]
        public void Create_ValidFundacao_InserirChamadoEMensagemSucesso()
        {
            // Arrange
            var mockDb = new Mock<IDatabase>();
            mockDb.Setup(db => db.BuscarPorCNPJ(It.IsAny<string>())).Returns((Fundacao?)null);
            var controller = new FundacoesController(mockDb.Object);
            var fundacao = BuildValidFundacao();

            // Act
            var result = controller.Create(fundacao) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Cadastro efetuado com sucesso!", controller.ViewBag.Mensagem);
            mockDb.Verify(db => db.Inserir(It.Is<Fundacao>(f => f.CNPJ == fundacao.CNPJ)), Times.Once);
        }

        [Fact]
        public void SearchFoundationToEdit_NotFound_ShowsMensagem()
        {
            // Arrange
            var mockDb = new Mock<IDatabase>();
            mockDb.Setup(db => db.BuscarPorCNPJ("00000000000000")).Returns((Fundacao?)null);
            var controller = new FundacoesController(mockDb.Object);

            // Act
            var result = controller.SearchFoundationToEdit("00000000000000") as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Edit", result.ViewName); // explicit view
            Assert.Equal("Fundação não encontrada.", controller.ViewBag.Mensagem);
        }

        [Fact]
        public void SearchFoundationToEdit_Found_ReturnsModel()
        {
            // Arrange
            var mockDb = new Mock<IDatabase>();
            var fundacao = BuildValidFundacao();
            mockDb.Setup(db => db.BuscarPorCNPJ(fundacao.CNPJ)).Returns(fundacao);
            var controller = new FundacoesController(mockDb.Object);

            // Act
            var result = controller.SearchFoundationToEdit(fundacao.CNPJ) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Edit", result.ViewName);
            Assert.Same(fundacao, result.Model);
        }
    }
}

