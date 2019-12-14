using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataSource
{
    public interface IDataRetriever<T>
    {
        Task<List<T>> GetData(string parameter);
    }
}