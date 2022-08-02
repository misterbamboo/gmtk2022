using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DiceGame.Assets.DiceGame.DiceForge
{
    public class DiceForgeManager : MonoBehaviour
    {
        [SerializeField] DragDropZonesManager dragDropZonesManager;
        [SerializeField] UIArrows leftArrows;
        [SerializeField] UIArrows rightArrows;

        private void Start()
        {
            dragDropZonesManager.OnSourceChanged += DragDropZonesManager_OnSourceChanged;
            dragDropZonesManager.OnTargetChanged += DragDropZonesManager_OnTargetChanged;
        }

        private void DragDropZonesManager_OnTargetChanged(Transform target)
        {
            UpdateArrowEventsListeners(rightArrows, target);
        }

        private void DragDropZonesManager_OnSourceChanged(Transform source)
        {
            UpdateArrowEventsListeners(leftArrows, source);
        }

        private void UpdateArrowEventsListeners(UIArrows arrows, Transform source)
        {
            arrows.onLeftClicked.RemoveAllListeners();
            arrows.onUpClicked.RemoveAllListeners();
            arrows.onRightClicked.RemoveAllListeners();
            arrows.onDownClicked.RemoveAllListeners();

            var diceSpiner3D = source?.GetComponent<UIDiceSpiner3D>();
            if (diceSpiner3D != null)
            {
                arrows.onLeftClicked.AddListener(() => diceSpiner3D.ViewLeft());
                arrows.onUpClicked.AddListener(() => diceSpiner3D.ViewUp());
                arrows.onRightClicked.AddListener(() => diceSpiner3D.ViewRight());
                arrows.onDownClicked.AddListener(() => diceSpiner3D.ViewDown());
            }
        }
    }
}
