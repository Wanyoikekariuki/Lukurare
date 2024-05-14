using ProjectBase.Repository.Transaction;
using System.Threading.Tasks;

namespace ProjectBase.Repository
{
    public interface IValidatable<T, Tentity>
    {
        Task<ExecutionResult<T>> IsValidGeneral(T model, Tentity context);

        Task<ExecutionResult<T>> IsValidInsert(T model, Tentity context);

        Task<ExecutionResult<T>> IsValidUpdate(T model, Tentity context);
    }
}
