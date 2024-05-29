namespace Job_Portal_API.Interfaces
{
    public interface IRangeRepository<K,T> : IRepository<K,T> where T : class
    {
        Task<IEnumerable<T>> AddRange(IEnumerable<T> entities);
    }
}
