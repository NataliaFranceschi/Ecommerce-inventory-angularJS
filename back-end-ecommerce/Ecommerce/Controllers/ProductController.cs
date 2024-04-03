using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Controller utilizado para operações de CRUD de produtos.
/// Requer autenticação para acessar suas funcionalidades.
/// </summary>
[ApiController]
[Route("/product")]
[Authorize]
public class ProductController : ControllerBase
{
    private readonly IProductRepository _repository;

    public ProductController(IProductRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Retorna todos os produtos cadastrados.
    /// </summary>
    /// <response code="200">Retorna todos os produtos ou uma lista vazia caso não tenha produtos cadastrados.</response>
    /// <response code="401">Não autorizado.</response>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _repository.GetAll();

        return Ok(products);
    }

    /// <summary>
    /// Retorna as informacoes sobre o produto com id específico.
    /// </summary>
    /// <param name="id">Id do produto.</param>
    /// <response code="200">Retorna os dados do produto, quando encontrado.</response>
    /// <response code="404">Produto não encontrado.</response>
    /// <response code="401">Não autorizado.</response>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var product = await _repository.GetById(id);
            return Ok(product);
        }
        catch (Exception ex)
        {
            return NotFound(new { ex.Message });
        }

    }

    /// <summary>
    /// Retorna as informações sobre os produtos com o nome correspondente ao parametro fornecido.
    /// </summary>
    /// <param name="name">Palavra que deseja ser procurada.</param>
    /// <response code="200">Retorna os dados dos produtos cujo nome corresponde a procura, caso não tenha correspondência retorna uma lista vazia.</response>
    /// <response code="401">Não autorizado.</response>
    [HttpGet("search/{name}")]
    public async Task<IActionResult> GetByName(string name)
    {
        var products = await _repository.GetByName(name);
        return Ok(products);
    }

    /// <summary>
    /// Cria um novo produto no banco de dados.
    /// </summary>
    /// <param name="product">Dados do produto.</param>
    /// <response code="201">Produto foi criado com sucesso.</response>
    /// <response code="400">Dados da requisição são inválidos.</response>
    /// <response code="401">Não autorizado.</response>
    [HttpPost]
    public IActionResult Create(ProductRequest product)
    {
        _repository.Create(product);

        return Created("", product);
    }

    /// <summary>
    /// Atualiza um produto com o id específico.
    /// </summary>
    /// <param name="product">Dados atualizados do produto.</param>
    /// <response code="200">Produto foi atualizado com sucesso.</response>
    /// <response code="400">Dados da requisição são inválidos.</response>
    /// <response code="404">Produto não encontrado.</response>
    /// <response code="401">Não autorizado.</response>
    [HttpPut]
    public IActionResult Update(ProductDTO product)
    {
        try
        {
            _repository.Update(product);
        } 
        catch (Exception ex)
        {
            return NotFound(new { ex.Message });
        }

        return Ok();
    }

    /// <summary>
    /// Deleta o produto com id específico/>
    /// </summary>
    /// <param name="id">Id do produto.</param>
    /// <response code="204">Produto deletado com sucesso.</response>
    /// <response code="404">Produto não encontrado.</response>
    /// <response code="401">Não autorizado.</response>
    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        try
        {
            _repository.Delete(id);
        }
        catch (Exception ex)
        {
            return NotFound(new { ex.Message });
        }

        return NoContent();
    }
}