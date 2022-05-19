using System.Collections;
using System.Collections.Generic;
using Hero;
using Systems.Healths;
using TMPro;
using UnityEngine;

namespace UI.Displaying
{
    public class ButtonDisplayer : MonoBehaviour
    {
        [SerializeField] private InformationBlock informationBlock;
        [SerializeField] private TextMeshProUGUI textMeshPro;

        private HeroDoorInteraction _heroDoorInteraction;

        private void OnDestroy()
        {
            if (_heroDoorInteraction != null)
                _heroDoorInteraction.ShowUIMessage -= UpdateView;
        }

        public void Construct(HeroDoorInteraction heroDoorInteraction)
        {
            _heroDoorInteraction = heroDoorInteraction;
            _heroDoorInteraction.ShowUIMessage += UpdateView;
        }

        private void UpdateView(bool isDisplay, string text)
        {
            if (isDisplay)
            { 
                informationBlock.gameObject.SetActive(true);
                textMeshPro.text = text;
            }
            else
            {
                informationBlock.gameObject.SetActive(false);
            }
        }
    }
}
