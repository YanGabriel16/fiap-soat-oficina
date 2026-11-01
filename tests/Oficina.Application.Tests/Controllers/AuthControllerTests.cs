namespace Oficina.Application.Tests.Controllers;

public sealed class AuthControllerTests
{
    [Test]
    public async Task Cadastrar_DeveRetornarCreated()
    {
        var service = new Mock<IAuthService>();
        var id = Guid.NewGuid();
        service.Setup(x => x.CadastrarAsync("Ana", "ana@teste.com", "senha-segura", It.IsAny<CancellationToken>()))
            .ReturnsAsync(id);
        var controller = new AuthController(service.Object);

        var resultado = await controller.Cadastrar(
            new CadastrarUsuarioRequest { Nome = "Ana", Email = "ana@teste.com", Senha = "senha-segura" },
            CancellationToken.None);

        Assert.That(resultado, Is.TypeOf<ObjectResult>());
        Assert.That(((ObjectResult)resultado).StatusCode, Is.EqualTo(201));
        Assert.That(((ObjectResult)resultado).Value, Is.EqualTo(id));
    }

    [Test]
    public async Task Login_ComCredenciaisValidas_DeveRetornarOk()
    {
        var service = new Mock<IAuthService>();
        var login = new LoginDto("token", DateTimeOffset.UtcNow.AddHours(1));
        service.Setup(x => x.LoginAsync("ana@teste.com", "senha-segura", It.IsAny<CancellationToken>()))
            .ReturnsAsync(login);
        var controller = new AuthController(service.Object);

        var resultado = await controller.Login(
            new LoginRequest { Email = "ana@teste.com", Senha = "senha-segura" },
            CancellationToken.None);

        Assert.That(resultado, Is.TypeOf<OkObjectResult>());
        Assert.That(((OkObjectResult)resultado).Value, Is.EqualTo(login));
    }

    [Test]
    public async Task Login_ComCredenciaisInvalidas_DeveRetornarUnauthorized()
    {
        var service = new Mock<IAuthService>();
        service.Setup(x => x.LoginAsync("ana@teste.com", "senha-errada", It.IsAny<CancellationToken>()))
            .ReturnsAsync((LoginDto?)null);

        var resultado = await new AuthController(service.Object).Login(
            new LoginRequest { Email = "ana@teste.com", Senha = "senha-errada" },
            CancellationToken.None);

        Assert.That(resultado, Is.TypeOf<UnauthorizedResult>());
    }

    [Test]
    public async Task Cadastrar_QuandoUsuarioJaExiste_DeveRetornarConflict()
    {
        var service = new Mock<IAuthService>();
        service.Setup(x => x.CadastrarAsync("Ana", "ana@teste.com", "senha-segura", It.IsAny<CancellationToken>()))
            .ThrowsAsync(new ConflitoException("Já existe um usuário cadastrado com este e-mail."));
        var controller = new AuthController(service.Object)
        {
            ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
        };

        var resultado = await controller.Cadastrar(
            new CadastrarUsuarioRequest { Nome = "Ana", Email = "ana@teste.com", Senha = "senha-segura" },
            CancellationToken.None);

        Assert.That(resultado, Is.TypeOf<ObjectResult>());
        Assert.That(((ObjectResult)resultado).StatusCode, Is.EqualTo(StatusCodes.Status409Conflict));
    }
}
