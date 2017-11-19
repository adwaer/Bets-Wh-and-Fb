using System.Threading.Tasks;
using Bets.Domain;

namespace Bets.Selenium
{
    public interface IBettingPage
    {
        Task<bool> SetTotal(StatViewModel model, decimal amount, bool more);
        Task<bool> SetHc(StatViewModel model, decimal amount, bool more);
    }
}
