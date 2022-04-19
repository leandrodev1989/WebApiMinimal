using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using WebApiMinimal.Contexto;
using WebApiMinimal.Models;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();



//MEU CONTEXTO
builder.Services.AddDbContext<Contexto>(options =>
options.UseSqlServer("Data Source=DESKTOP-AC\\SQLSERVER;Initial Catalog=model;Integrated Security=True"));


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
