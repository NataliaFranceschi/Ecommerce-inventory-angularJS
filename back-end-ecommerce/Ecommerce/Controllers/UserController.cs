using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Controller respons�vel por lidar com solicita��es de login e autentica��o de usu�rios.
/// </summary>
[ApiController]
[Route("/login")]
public class LoginController : ControllerBase
{
    private readonly IUserRepository _repository;

    public LoginController(IUserRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Retorna um token.
    /// </summary>
    /// <response code="200">Retorna um token caso usu�rio for autenticado com sucesso.</response>
    /// <response code="401">N�o autorizado.</response>
    [HttpPost]
    public IActionResult Login(LoginDTO login)
    {
        try
        {
            var user = _repository.Login(login);
            var token = new TokenGenerator().Generate();
            return Ok(new { token });
        }
        catch (Exception ex)
        {
            return Unauthorized(new { ex.Message });
        }
    }
}
