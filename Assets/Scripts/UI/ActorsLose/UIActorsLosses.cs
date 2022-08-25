using Game.GameModes;
using Game.GameModes.Overrides;
using System.Collections;
using TMPro;
using UnityEngine;
using Zenject;

namespace UI.ActorsLose
{
    public class UIActorsLosses : MonoBehaviour
    {
        [SerializeField] private TMP_Text _lossesText;

        private MatchThreeGameMode _matchThreeGameMode;


        [Inject]
        private void Construct(GameMode gameMode)
        {
            _matchThreeGameMode = gameMode as MatchThreeGameMode;

            _matchThreeGameMode.ChangeCurrentLossesActors += OnChangeCurrentLossesActors;

            OnChangeCurrentLossesActors(0);
        }

        private void OnChangeCurrentLossesActors(int currentCount)
        {
            _lossesText.text = currentCount + " / " + _matchThreeGameMode.MaxLostActors + " ПОТЕРИ";
        }
    }
}