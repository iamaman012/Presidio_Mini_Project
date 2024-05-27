namespace Job_Portal_API.Interfaces
{
    public interface IRepository<K,T> where T : class
    {
        public  Task<T> Add(T entity);
        public  Task<T> Update(T entity);
        public  Task<T> DeleteById(K id);
        public  Task<T> GetById(K id);
        public  Task<IEnumerable<T>> GetAll();
    }
}
