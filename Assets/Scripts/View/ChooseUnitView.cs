using System;
using Controller;
using Model;
using Model.Config;
using UnityEngine;
using Utilities;

namespace View
{
    public class ChooseUnitView : MonoBehaviour
    {
        [SerializeField] private RectTransform _root;
        [SerializeField] private RectTransform _unitListParent;
        [SerializeField] private UnitCardView _unitCardPrefab;

        private IReadOnlyRuntimeModel _model;
        
        private void Start()
        {
            _model = ServiceLocator.Get<IReadOnlyRuntimeModel>();
            SetupCards(ServiceLocator.Get<Settings>());
        }
        
        private void Update()
        {
            var visible = _model.Stage == RuntimeModel.GameStage.ChooseUnit;
            if (visible != _root.gameObject.activeSelf)
                _root.gameObject.SetActive(visible);
        }

        private void SetupCards(Settings settings)
        {
            foreach (var unitConfig in settings.PlayerUnits.Keys)
            {
                var card = Instantiate(_unitCardPrefab, _unitListParent);
                card.Initialize(unitConfig, OnUnitChosen);
            }
        }

        private void OnUnitChosen(UnitConfig unit)
        {
            ServiceLocator.Get<IPlayerUnitChoosingListener>().OnPlayersUnitChosen(unit);
        }
    }
}