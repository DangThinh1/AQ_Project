namespace Identity.Core.Models.SignIn
{
    public class SignInModelBase
    {
        public string DeviceId { get; set; }
        public string DeviceMacAddress { get; set; }
        public DeviceTypeEnum DeviceType { get; set; }
        public DeviceOSEnum DeviceOS { get; set; }
    }

    public enum DeviceTypeEnum
    {
        PC = 1,
        Mobile = 2
    }

    public enum DeviceOSEnum
    {
        Windows = 1,
        MacOS = 2,
        Android = 3,
        IOS = 4
    }
}
