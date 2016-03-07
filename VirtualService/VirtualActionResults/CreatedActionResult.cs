namespace Portfolio.API.Virtual.VirtualActionResults
{
    public class CreatedActionResult<T> : IVirtualActionResult
    {
        private T p;

        public CreatedActionResult(T p)
        {
            this.AffectedObject = p;
        }

        public object AffectedObject { get; set; }
    }
}