# Ecommerce Invertory
Desenvolvimento de um módulo para gerenciar o inventário dos produtos de um ecommerce.
## Backend
### Tecnologias Utilizadas
•	C# ASP.NET Core <br>
•	Swagger para documentação da API <br>
•	Autenticação JWT <br>
•	Banco de dados SQLite <br>
### Instruções de Instalação
1.	Clone o repositório do projeto em sua máquina local.
2.	Certifique-se de ter o .NET Core SDK instalado em sua máquina. Se não, você pode baixá-lo [aqui](https://dotnet.microsoft.com/en-us/download).
3.	Navegue até o diretório do backend do projeto.
4.	Execute o comando <strong> dotnet restore </strong>para instalar as dependências do projeto.
5.	Execute o comando <strong> dotnet ef database </strong> update para aplicar as migrações do banco de dados.
6.	Adicione no User Secrets uma chave "Secret" que será utilizada para assinar e verificar a autenticidade do token. Recomenda-se um comprimento mínimo de 256 bits (32 bytes) para o valor do secret.
7.	Execute o comando <strong> dotnet run </strong> para iniciar o servidor backend. Por padrão, ele estará disponível em https://localhost:5150.
### Documentação da API
A documentação da API pode ser acessada através do Swagger. Visite https://localhost:7137/swagger/index.html para visualizar todas as rotas disponíveis, seus parâmetros e especificações de retorno.
### Autenticação
A autenticação na API é feita utilizando tokens JWT. Para obter acesso às rotas protegidas, você deve incluir o token JWT no cabeçalho Authorization nas requisições HTTP. Um usuário administrador com as credenciais admin@admin.com e senha admin foi adicionado automaticamente ao banco de dados para fins de teste.
## Frontend
### Tecnologias Utilizadas
• AngularJS para o desenvolvimento da interface do usuário <br>
### Utilização
Inicie o servidor de desenvolvimento frontend, ele estará disponível em http://localhost:5501/.
Após, você poderá acessar a interface de usuário básica através do navegador. Uma página de login irá aparecer e utilizando o usuário para teste citado acima será possível realizar operações CRUD nos produtos do inventário.

