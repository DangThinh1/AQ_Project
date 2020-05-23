namespace AQDiningPortal.Core.Models.RestaurantBusinessDayOperations
{
    public class RestaurantBusinessDayOperationModel
    {
        public string WorkingStartTime { get; set; }
        public double? WorkingStartTimeValue { get; set; }
        public string WorkingEndTime { get; set; }
        public double? WorkingEndTimeValue { get; set; }
        public string WorkingTimeDisplay { get; set; }
        public bool HaveBreakTime { get; set; }
        public string BreakTimeStart { get; set; }
        public double? BreakTimeStartValue { get; set; }
        public string BreakTimeEnd { get; set; }
        public double? BreakTimeEndValue { get; set; }
        public string BreakTimeDisplay { get; set; }
    }
}
