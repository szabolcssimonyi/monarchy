using Monarchy.Gateway.Extensibility.Model;
using System.Collections.Generic;

namespace Monarchy.Gateway.Extensibility.Interface
{
    public interface ITokenService
    {
        TokenModel Create(string email, IEnumerable<string> permissions);
        string GetEmailFromToken(string token);
    }
}
