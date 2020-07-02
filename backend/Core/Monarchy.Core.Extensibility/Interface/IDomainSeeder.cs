using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Monarchy.Core.Extensibility.Interface
{
    public interface IDomainSeeder
    {
        Task SeedAsync<T>(T context) where T : DbContext;
    }
}
