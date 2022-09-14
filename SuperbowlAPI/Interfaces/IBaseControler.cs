using Microsoft.AspNetCore.Mvc;

namespace SuperbowlAPI.Interfaces
{
    public interface IBaseControler<T>
    {
        Task<IActionResult> Get();
        Task<IActionResult> Post(T entity);
        Task<IActionResult> Put(int key, T entity);
        Task<IActionResult> Patch(int key, T entity);
        Task<IActionResult> Delete(int key);
        Task<IActionResult> GetById(int key);
    }
}
