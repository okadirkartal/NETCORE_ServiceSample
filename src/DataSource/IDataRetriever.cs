using System.Collections.Generic;

namespace DataSource
{
    public interface IDataRetriever<T>
    {
        List<T> GetData(string parameter);
    }
}