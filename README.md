# Projeto .NET - Instruções

Este arquivo contém as instruções necessárias para configurar, executar e testar o projeto localmente.

---

## Pré-requisitos

Antes de começar, certifique-se de ter o seguinte software instalado em sua máquina:

-   **.NET 9 SDK** ou superior.

---

## Como Executar o Projeto

Siga os passos abaixo para compilar e executar a aplicação principal.

1.  **Clone o repositório** (se aplicável) e navegue para o diretório raiz:
    ```sh
    https://github.com/JVRosado/DesafioWeb
    cd DesafioWeb
    ```

2.  **Restaure as dependências do projeto:**
    Este comando baixa e instala todos os pacotes NuGet necessários para a solução.
    ```sh
    dotnet restore
    ```

3.  **Compile o projeto:**
    Este comando compila o código-fonte do projeto e suas dependências.
    ```sh
    dotnet build
    ```

4.  **Execute a aplicação:**
    Este comando inicia o projeto principal (`DesafioWeb`).
    ```sh
    dotnet run --project DesafioWeb/DesafioWeb.csproj
    ```
    Após a execução, a aplicação estará disponível no endereço indicado no console (ex: `http://localhost:5000`).

---

## Como Executar os Testes

Para garantir que tudo está funcionando como esperado, execute os testes unitários com o seguinte comando na raiz do projeto:

```sh
dotnet test
```
Este comando encontrará e executará todos os projetos de teste na solução, exibindo os resultados no console.

---

## Dependências Utilizadas

O projeto utiliza as seguintes bibliotecas:

#### Projeto Principal (`DesafioWeb.csproj`)
-   `Microsoft.Data.Sqlite`: Utilizado para a comunicação com o banco de dados SQLite.

#### Projeto de Teste
-   `Microsoft.NET.Test.Sdk`: SDK base para os testes .NET.
-   `xunit` e `xunit.runner.visualstudio`: Framework e runner para a execução dos testes unitários.
-   `Moq`: Biblioteca para criação de *mocks* e simulação de dependências nos testes.
