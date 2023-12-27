# .NET_Core_MVC_Locadora
![Badge de Encerrado](https://img.shields.io/badge/status-Encerrado-red)

Repositório para o versionamento do sistema web "Locadora" destinado para a locação de filmes online.

## Descrição
Projeto de site da web pessoal feito com o intuito de aplicar os conhecimentos obtidos no curso técnico em Informática para Internet do Senac, implementando a utilização de web api.

O software tem a proposta de ser uma locadora online de filmes, com uma estrutura de gestão de filmes disponíveis, usuários e processos de locação, assim simulando todos os aspectos de uma locadora online. O sistema utiliza Web Api para se comunicar com o banco de dados e para se ligar ao app "Locadora Mobile", assim a Web Api permite que tanto o sistema web quanto o app tenham acesso aos mesmos dados e permitam ao usuário a mesma experiência, porém em plataformas diferentes.

## Instalação
1. Baixe o arquivo .zip do Projeto "Locadora".
2. Baixe o arquivo .zip do Projeto "LocadoraAPI" disponível no repositório ".NET_Core_LocadoraAPI".
3. Extraia ambos os projetos e abra-os em diferentes guias do seu editor de código-fonte.
4. Abra o XAMPP e clique em "start" para os módulos "Apache" e "MySQL".
5. Execute no terminal do projeto "LocadoraAPI" o comando `dotnet run`.
6. De volta na aba do XAMPP, clique em "Admin" no módulo "MySQL" e verifique no PhpMyAdmin se o banco de dados da Web API "LocadoraAPI" foi criado.
7. De volta ao editor de código, na aba do projeto "Locadora" execute no terminal `dotnet run` e confira.
