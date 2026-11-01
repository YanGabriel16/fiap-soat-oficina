using Oficina.Domain.Entities;

namespace Oficina.Domain.Interfaces.Services;

public interface ITokenService
{
    string Gerar(Usuario usuario);
}
