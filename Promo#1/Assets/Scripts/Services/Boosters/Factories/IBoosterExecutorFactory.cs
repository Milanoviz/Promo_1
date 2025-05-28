using Services.Boosters.Data;
using Services.Boosters.Executors;

namespace Services.Boosters.Factories
{
    public interface IBoosterExecutorFactory
    {
        IBoosterExecutor CreateExecutor(BoosterItemData boosterItemData);
    }
}