namespace Shared.Repository;

public interface IRepository<T> where T : class
{
    public Task<bool> Create(List<T> items);
    public Task<bool> Create(T item);
    public Task<List<T>> Get(List<string> ids);
    public Task<T> Get(string id);
    public Task<List<T>> GetAll();
    public Task<bool> Update(T item);
    public Task<bool> Delete(string id);
    public Task<bool> Delete(List<string> ids);
}
