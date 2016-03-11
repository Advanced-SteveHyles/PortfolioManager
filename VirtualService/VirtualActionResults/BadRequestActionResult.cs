namespace Portfolio.API.Virtual.VirtualActionResults
{
    
    public class BadRequestActionResult : IVirtualActionResult
    {
        public object AffectedObject { get; set; }
    }
}