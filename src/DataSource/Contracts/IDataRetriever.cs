using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataSource.Contracts
{
    public interface IDataRetriever<T>
    {
        Task<List<T>> GetData(string parameter = null);
    }
}