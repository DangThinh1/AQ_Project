using AQBooking.YachtPortal.Core.Models.Yachts;
using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.YachtPortal.Core.Models.RedisCaches
{
    public class RedisCacheYacthRequestModel
    {
        public string HashKey { get; set; }
        public string Key { get; set; }
        public string YachtId { get; set; }
    }
    public class RedisCachesModel
    {
        public string HashKey { get; set; }
        public string Key { get; set; }
        public List<YachtPackageServiceModel> CartStorage { get; set; }
    }

    public class RedisCachesYachtModel: RedisCacheYacthRequestModel
    {
        public RedisCachesYachtCartStorageModel CartStorage { get; set; }
    }
    
    public class RedisCachesYachtCartStorageModel
    {
        public string YachtId { get; set; }
        public MerchantProductInventoriesModel ProductPackage { get; set; }
    }
    public class RedisCachesYachRemoveModel: RedisCacheYacthRequestModel
    {
       public string productInventoryFId { get; set; }
        public bool isDeleteEntireYacht { get; set; }
    }

    //public class RedisCachesViewModel
    //{
    //    public string HashKey { get; set; }
    //    public string Key { get; set; }
    //    public List<YachtCartStorageViewModel> CartStorage { get; set; }
    //}
}
