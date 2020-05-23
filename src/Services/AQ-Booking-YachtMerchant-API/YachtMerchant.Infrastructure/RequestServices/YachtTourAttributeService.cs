using YachtMerchant.Infrastructure.Database;

namespace YachtMerchant.Infrastructure.RequestServices
{
    public class YachtTourAttributeService
    {
        private YachtOperatorDbContext _db;

        public YachtTourAttributeService(YachtOperatorDbContext db)
        {
            _db = db;
        }



    }
}
