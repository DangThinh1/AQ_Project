using System.ComponentModel.DataAnnotations;
using YachtMerchant.Core.Models.YachtNonBusinessDay;

namespace YachtMerchant.Core.Models.YachtNonBusinessDay
{
    public class YachtNonBusinessDayUpdateModel : YachtNonBusinessDayCreateModel
    {
        [Required]
        public int Id { get; set; }
    }
}
