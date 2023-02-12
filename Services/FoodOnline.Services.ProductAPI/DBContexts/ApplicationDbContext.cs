using Microsoft.EntityFrameworkCore;

namespace FoodOnline.Services.ProductAPI.DBContexts
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {

        }
    }
}
