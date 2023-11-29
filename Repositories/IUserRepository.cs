using Microsoft.AspNetCore.Identity;

namespace Bloggie.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<IdentityUser>>GetAll();
    }
}
