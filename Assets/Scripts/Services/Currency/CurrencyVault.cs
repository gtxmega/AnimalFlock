using System;
using UnityEngine;

namespace Services.Currency
{
    public class CurrencyVault : ICurrencyVault, ICurrencyVaultState
    {
        public event Action<int> OnChange;
        public int CurrentCount => _currentCount;

        [SerializeField] private int _currentCount;

        public void ChangeCurrency(int amount)
        {
            _currentCount += amount;

            OnChange?.Invoke(_currentCount);
        }
    }
}