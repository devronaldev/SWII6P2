# SWII6P2 - WEB API - Gerenciador de Produtos

## Descrição do Projeto
Este projeto é uma API desenvolvida para gerenciar produtos, utilizando o padrão de arquitetura MVC com ASP.NET Core. O objetivo principal é permitir a criação, leitura, atualização e exclusão de produtos e usuários, com o suporte de um banco de dados MySQL para persistência dos dados. A API oferece todos os recursos necessários para a gestão de usuários e produtos, com autenticação via JWT e controle de status dos usuários e produtos (ativo/inativo).

## Funcionalidades
A API permite as seguintes funcionalidades:

- **Usuário:**
  - Criar um novo usuário com nome, senha e status (ativo/inativo).
  - Listar usuários cadastrados no sistema.
  - Visualizar detalhes de um usuário específico.
  - Editar informações de um usuário existente (nome, senha, status).
  - Excluir um usuário.

- **Produto:**
  - Criar um novo produto com nome, preço, status (ativo/inativo), e informações sobre o usuário responsável pela criação e atualização.
  - Listar produtos cadastrados.
  - Visualizar detalhes de um produto específico.
  - Editar informações de um produto existente.
  - Excluir um produto.

## Tecnologias Utilizadas
- **ASP.NET Core API:** Usado para criar a API que expõe endpoints para interação com o banco de dados.
- **Entity Framework Core:** ORM utilizado para interagir com o banco de dados MySQL e realizar as operações CRUD.
- **JWT (JSON Web Tokens):** Usado para autenticação e autorização dos usuários.
- **MySQL:** Banco de dados utilizado para armazenar as informações dos usuários e produtos.
- **Swagger:** Usado para documentação da API, permitindo testar e visualizar os endpoints de forma fácil.

## Banco de Dados
A aplicação utiliza um banco de dados MySQL com duas entidades principais: **Usuário** e **Produto**. As tabelas possuem os seguintes campos:

- **Usuário:**
  - `Id` (int)
  - `Nome` (string)
  - `Senha` (string)
  - `Status` (ativo/inativo) (boolean)

- **Produto:**
  - `Id` (int)
  - `Nome` (string)
  - `Preço` (float)
  - `Status` (ativo/inativo) (boolean)
  - `IdUsuarioCadastro` (int) — Usuário que cadastrou o produto.
  - `IdUsuarioUpdate` (int) — Usuário que atualizou o status do produto.

## Créditos
<table>
  <tr>
    <td>
      Prontuário
    </td>
    <td>
      Nome Completo
    </td>
  </tr>
  <tr>
    <td>
      CB3011836
    </td>
    <td>
      Ketheleen Cristine Simão dos Santos
    </td>
  </tr>
  <tr>
    <td>
      CB3020282
    </td>
    <td>
      Ronald Pereira Evangelista
    </td>
  </tr>
</table>
<h2>LINKS</h2>
<p><a href="https://youtu.be/G0Br5foI3g0" target="_blank">Demonstração Swagger</a></p>
