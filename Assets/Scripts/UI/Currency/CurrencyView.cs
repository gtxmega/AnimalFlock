using Services.Currency;
using TMPro;
using UnityEngine;
using Zenject;

namespace UI.Currency
{
    public class CurrencyView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _currencyText;

        private ICurrencyVaultState _currencyVaultState;

        [Inject]
        private void Construct(ICurrencyVaultState currencyVaultState)
        {
            _currencyVaultState = currencyVaultState;

            _currencyVaultState.OnChange += OnChangeCurrency;

            OnChangeCurrency(_currencyVaultState.CurrentCount);
        }

        private void OnChangeCurrency(int currentCount)
        {
            _currencyText.text = currentCount.ToString();
        }
    }
}