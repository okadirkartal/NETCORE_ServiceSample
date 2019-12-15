using System.Net.Http;
using System.Threading.Tasks;

namespace DataSource.Contracts
{
    public interface IOnlineDataRetriever
    {
        Task<HttpResponseMessage> GetDataFromApi(string parameter);
    }
}