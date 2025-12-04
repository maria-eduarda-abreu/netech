# Netech - Ecossistema Digital para Mobilidade Sustentável

<p align="center"\>
<img src="http://img.shields.io/static/v1?label=Framework&message=ASP.NET 8 &color=purple&style=for-the-badge"/>
<img src="http://img.shields.io/static/v1?label=ORM&message=Entity-Framework-Core-8 &color=purple&style=for-the-badge"/>
<img src="http://img.shields.io/static/v1?label=Framework&message=.NET &color=purple&style=for-the-badge"/>
<img src="http://img.shields.io/static/v1?label=Steck&message=csharp&color=purple&style=for-the-badge"/>
<img src="http://img.shields.io/static/v1?label=Deploy&message=Docker&color=blue&style=for-the-badge"/>
<img src="http://img.shields.io/static/v1?label=License&message=MIT&color=green&style=for-the-badge"/>
<img src="https://img.shields.io/static/v1?label=STATUS&message=EM%20DESENVOLVIMENTO&color=RED&style=for-the-badge"/>
</p\>

> Status do Projeto: :warning: Em desenvolvimento

### Tópicos

:small\_blue\_diamond: [Descrição do projeto](https://www.google.com/search?q=%23descri%C3%A7%C3%A3o-do-projeto)

:small\_blue\_diamond: [Funcionalidades](https://www.google.com/search?q=%23funcionalidades)

:small\_blue\_diamond: [Pré-requisitos](https://www.google.com/search?q=%23pr%C3%A9-requisitos)

:small\_blue\_diamond: [Como rodar a aplicação](https://www.google.com/search?q=%23como-rodar-a-aplica%C3%A7%C3%A3o-arrow_forward)

:small\_blue\_diamond: [Configuração de Banco de Dados](https://www.google.com/search?q=%23iniciandoconfigurando-banco-de-dados)

:small\_blue\_diamond: [JSON e Dados](https://www.google.com/search?q=%23json-floppy_disk)

:small\_blue\_diamond: [Tecnologias Utilizadas](https://www.google.com/search?q=%23linguagens-dependencias-e-libs-utilizadas-books)

## Descrição do projeto

<p align="justify"\>
O Netech é uma API RESTful robusta desenvolvida em .NET 8, focada nos pilares de ESG (Environmental, Social, and Governance). O objetivo da aplicação é calcular e registrar a economia de carbono gerada pela escolha de modais de transporte sustentáveis (como bicicletas elétricas e caminhada) em detrimento de veículos a combustão.
</p\>
\<p align="justify"\>
O projeto utiliza \<b\>Clean Architecture\</b\> para separar responsabilidades entre Domínio, Infraestrutura e API, implementando padrões avançados como Keyset Pagination para alta performance em grandes volumes de dados de telemetria.
</p\>

## Funcionalidades

:heavy\_check\_mark: **Cálculo de Economia de CO2:** Algoritmo que compara a emissão do modal escolhido contra um modal base (carro a gasolina).

:heavy\_check\_mark: **Registro de Viagens (LogTrip):** Endpoint para input de dados de deslocamento com validação de regras de negócio.

:heavy\_check\_mark: **Histórico com Paginação Keyset:** Consulta otimizada de histórico de viagens utilizando cursores (Data + ID) para evitar lentidão em grandes datasets.

:heavy\_check\_mark: **Tratamento Global de Erros:** Implementação de `IExceptionHandler` para respostas HTTP padronizadas (RFC 7807) sem expor dados sensíveis da infraestrutura.

:heavy\_check\_mark: **Documentação Automática:** Integração com Swagger UI para testes e visualização dos endpoints.

## Layout ou Deploy da Aplicação :dash:

> A aplicação pode ser testada localmente via Swagger UI.


## Pré-requisitos

:warning: [baixe e instale o .NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

:warning: [SQL Server Express (ou LocalDB)](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)

:warning: [Docker Desktop](https://www.docker.com/products/docker-desktop) (Opcional, caso queira rodar via container)

## Como rodar a aplicação :arrow\_forward:

No terminal, clone o projeto:

```bash
git clone https://github.com/seu-usuario/netech.git
cd netech
```

Restaure as dependências do projeto:

```bash
dotnet restore
```

Configure a string de conexão no arquivo `appsettings.json` na pasta `netech/`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=NetechDb;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```

Rode a aplicação:

```bash
cd netech
dotnet run
```

Acesse `https://localhost:7134/swagger` (ou a porta indicada no console) para ver a documentação.

## Iniciando/Configurando banco de dados

O projeto utiliza **Entity Framework Core** com abordagem Code-First. Para criar o banco de dados e as tabelas iniciais (Trips e CarbonFactors), execute os comandos abaixo na raiz da solução:

1.  Criar a migração inicial:

<!-- end list -->

```bash
dotnet ef migrations add InitialCreate --project netech.Infrastructure --startup-project netech.Api
```

2.  Aplicar a migração ao banco:

<!-- end list -->

```bash
dotnet ef database update --project netech.Infrastructure --startup-project netech.Api
```

*Nota: Isso irá semear (seed) automaticamente a tabela `CarbonFactors` com os dados de referência (Carro, E-Bike, etc).*

## Casos de Uso

A API é projetada para ser consumida por aplicativos móveis ou web dashboards de ESG.

1.  **Usuário finaliza uma viagem de bicicleta:** O app envia um POST para `/api/trips`.
2.  **API calcula:** Identifica que E-Bike emite 9g/km contra 271g/km do Carro.
3.  **Resultado:** Registra a economia positiva e retorna os "Pontos de Carbono" para gamificação.

## JSON :floppy\_disk:

### Exemplo de Requisição (POST /api/trips):

```json
{
  "transportModeId": 2,
  "distanceMeters": 15000,
  "startDateTime": "2023-10-27T08:00:00-03:00",
  "endDateTime": "2023-10-27T08:45:00-03:00"
}
```

### Exemplo de Resposta (201 Created):

```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "mode": "E-Bike",
  "distanceKm": 15.0,
  "co2SavedGrams": 3930.0,
  "date": "2023-10-27T08:00:00-03:00"
}
```

## Linguagens, dependencias e libs utilizadas :books:

  - [ASP.NET Core 8 Web API](https://learn.microsoft.com/en-us/aspnet/core/?view=aspnetcore-8.0)
  - [Entity Framework Core 8 (SQL Server)](https://learn.microsoft.com/en-us/ef/core/)
  - [Swashbuckle (Swagger)](https://github.com/domaindrivendev/Swashbuckle.AspNetCore) - Para documentação da API.
  - [Microsoft.VisualStudio.Azure.Containers.Tools.Targets](https://www.nuget.org/packages/Microsoft.VisualStudio.Azure.Containers.Tools.Targets/) - Suporte a Docker.

## Resolvendo Problemas :exclamation:

Em [issues](https://www.google.com/search?q=) foram abertos alguns problemas gerados durante o desenvolvimento desse projeto e como foram resolvidos.

## Tarefas em aberto

:memo: Implementar autenticação via JWT (Identity API).

:memo: Criar testes unitários para o `CarbonCalculatorService`.

:memo: Implementar sistema de gamificação (Badges e Níveis).

## Desenvolvedores/Contribuintes :octocat:

| [\<img src="https://avatars.githubusercontent.com/u/0?v=4" width=115\><br>\<sub\>Maria Eduarda Abreu\</sub\>](https://www.google.com/search?q=https://github.com/maria-eduarda-abreu) |
| :---: |

## Licença

The [MIT License](https://www.google.com/search?q=) (MIT)

Copyright :copyright: 2024 - Netech Mobilidade Sustentável
