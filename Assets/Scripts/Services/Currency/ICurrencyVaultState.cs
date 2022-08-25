using System;

namespace Services.Currency
{
    public interface ICurrencyVaultState
    {
        int CurrentCount { get; }

        event Action<int> OnChange;
    }
}