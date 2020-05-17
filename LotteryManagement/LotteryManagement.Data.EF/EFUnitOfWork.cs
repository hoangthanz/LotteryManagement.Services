using LotteryManagement.Infrastructure.Interfaces;

namespace LotteryManagement.Data.EF
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private readonly LotteryManageDbContext _context;

        public EFUnitOfWork(LotteryManageDbContext context)
        {
            this._context = context;
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
