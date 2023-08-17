using System.Threading.Tasks;

namespace Grain.Interfaces
{
    public interface IUserGrain: Orleans.IGrainWithStringKey
    {
        Task<bool> Exist();
    }
}