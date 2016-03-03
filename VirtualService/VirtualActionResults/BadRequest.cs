namespace Portfolio.API.Virtual.VirtualActionResults
{
    public class BadRequest : IVirtualActionResult
    {
        public object AffectedObject { get; set; }
    }

    public class Created : IVirtualActionResult
    {
        private object p;

        public Created(object p)
        {
            this.AffectedObject = p;
        }

        public object AffectedObject { get; set; }
    }
}