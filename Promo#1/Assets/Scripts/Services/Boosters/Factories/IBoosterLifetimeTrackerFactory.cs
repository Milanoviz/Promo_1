using Services.Boosters.Enums;
using Services.Boosters.Trackers;

namespace Services.Boosters.Factories
{
    public interface IBoosterLifetimeTrackerFactory
    {
        IBoosterLifetimeTracker CreateTracker(BoosterType boosterType, BoosterLifetimeType lifetimeType, string value);
    }
}