using dusicyon_midnight_tribes_backend.Contexts;
using Microsoft.EntityFrameworkCore;

namespace dusicyon_midnight_tribes_backend.Services.Repositories
{
    public class GenericRepository : IGenericRepository
    {
        private readonly IContext _context;

        public GenericRepository(IContext context)
        {
            _context = context;
        }

        public bool Save()
        {
            int result = _context.SaveChanges();

            if (result == 0)
            {
                return false;
            }
            return true;
        }
        
    }
}