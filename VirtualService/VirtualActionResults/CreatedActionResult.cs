namespace Portfolio.API.Virtual.VirtualActionResults
{
    public class CreatedActionResult : IVirtualActionResult
    {
        private object p;

        public CreatedActionResult(object p)
        {
            this.AffectedObject = p;
        }

        public object AffectedObject { get; set; }
    }
}