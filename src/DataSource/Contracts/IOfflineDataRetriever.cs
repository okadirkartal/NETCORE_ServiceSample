using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataSource.Contracts
{
    public interface IOfflineDataRetriever<T>
    {
        Task<List<T>> GetDataFromFile(string parameter);
    }
}