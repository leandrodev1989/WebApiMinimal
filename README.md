# Criação de Uma MinimalApi

### TECNOLOGIAS USADAS

* Visual Studio 2022
* ASP .NET Core 6
* Swagger
* Entity FrameworkCore
* C#


---

### O QUE É MINIMAL API?

Com a chegada do .NET 6, a Microsoft disponibilizou o recurso das Minimal APIs, que foram pensadas para permitir a criação de APIs HTTP que utilizem menos recursos, sendo uma ótima solução para implementação de microsserviços e aplicativos que consumam o mínimo de dependências do
[ASP .NET Core.](https://docs.microsoft.com/pt-br/aspnet/core/?view=aspnetcore-6.0) 
Então, Assim Criamos uma API  escrevendo o mínimo de linhas de código o possível.


------
#### Neste Exemplo foi Criado Uma Classe Produto Para Criar um novo Produto, Adicionar, Deletar, Listar.

 ```
 public class Produto
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
    }
 ```
 
 
 #### Classe de Contexto Usando o EnsureCreated para verificar e criar o banco sem a necessidade de fazer o Migration.
 
```
public class Contexto : DbContext
    {
        public Contexto(DbContextOptions<Contexto> options)
            : base(options) => Database.EnsureCreated();
        

        

        public DbSet<Produto> Produto { get; set; }
    }

```

### Class Program Aonde Foi Colocado todo codigo de Metodos é atributos

```
var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();

//MEU CONTEXTO
builder.Services.AddDbContext<Contexto>(options =>
options.UseSqlServer("MeuBanco"));


//Implementando o SwaggerGen
builder.Services.AddSwaggerGen();

var app = builder.Build();

//app Swagger
app.UseSwagger();


//metodo Post da api
app.MapPost("AdicionaProduto", async (Produto produto, Contexto contexto) =>
{
    contexto.Produto.Add(produto);
    await contexto.SaveChangesAsync();
});

//metodo de exclusão
app.MapPost("ExcluirProduto/{id}", async (int id, Contexto contexto) =>
{
    var produtoExcluir =  await contexto.Produto.FirstOrDefaultAsync(p => p.Id == id);

    if(produtoExcluir != null)
    {
        contexto.Produto.Remove(produtoExcluir);
        await contexto.SaveChangesAsync();
    }
    
});

//Listagem
app.MapPost("ListarProdutos", async (Contexto contexto) =>
{
    return await contexto.Produto.ToListAsync();

});


//ObterProduto
app.MapPost("ObterProduto/{id}", async (int id, Contexto contexto) =>
{
   return await contexto.Produto.FirstOrDefaultAsync(p => p.Id == id);


});



//app SwaggerUI
app.UseSwaggerUI();

app.Run();
```
### Fazendo As Consultas No Swagger

![Swagger](https://user-images.githubusercontent.com/83560879/164003184-bf81fc21-c2a7-4d1e-8ed1-54b518133116.png)
