namespace EPAM.Cache.Models
{
    public sealed class CacheOptions
    {
        public int SlidingExpirationInSeconds { get; set; }

        public int AbsoluteExpiration { get; set; }

        public required DistributedOptions DistributedOptions { get; set; }
    }
}
