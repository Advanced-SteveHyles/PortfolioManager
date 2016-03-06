namespace Portfolio.API.Virtual.VirtualActionResults
{
    public class OkSingleActionResult<T> : IVirtualActionResult
    {
        private readonly T _objectInstance;

        public OkSingleActionResult(T objectInstance)
        {
            _objectInstance = objectInstance;
        }

        public T ObjectInstance => _objectInstance;

    }
}