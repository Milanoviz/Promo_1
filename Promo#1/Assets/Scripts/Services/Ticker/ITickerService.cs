using System;

namespace Services.Ticker
{
    public interface ITickerService
    {
        event EventHandler OnTick;
    }
}