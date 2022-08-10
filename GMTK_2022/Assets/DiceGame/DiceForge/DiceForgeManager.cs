using UnityEngine;
using UnityEngine.UI;

namespace DiceGame.Assets.DiceGame.DiceForge
{
    public class DiceForgeManager : MonoBehaviour
    {
        [SerializeField] DragDropZonesManager dragDropZonesManager;
        [SerializeField] UIArrows leftArrows;
        [SerializeField] UIArrows rightArrows;
        [SerializeField] Button forgeButton;

        private DiceCube targetDice;
        private DiceCube sourceDice;


        private void Start()
        {
            dragDropZonesManager.OnSourceChanged += DragDropZonesManager_OnSourceChanged;
            dragDropZonesManager.OnTargetChanged += DragDropZonesManager_OnTargetChanged;
            forgeButton.onClick.AddListener(ForgeDice);
        }

        public void ForgeDice()
        {
            var sourceFaceSide = sourceDice.GetDisplayDiceFaceSide();
            var targetFaceSide = targetDice.GetDisplayDiceFaceSide();

            var face = sourceDice.GetDiceFace(sourceFaceSide);
            targetDice.ReplaceFace(targetFaceSide, face);
            print(sourceFaceSide + " => " + targetFaceSide);
        }

        private void DragDropZonesManager_OnTargetChanged(Transform target)
        {
            UpdateArrowEventsListeners(rightArrows, target);
            UpdateDiceSelected(ref targetDice, target?.gameObject);
            UpdateForgeButtonState();
        }

        private void DragDropZonesManager_OnSourceChanged(Transform source)
        {
            UpdateArrowEventsListeners(leftArrows, source);
            UpdateDiceSelected(ref sourceDice, source?.gameObject);
            UpdateForgeButtonState();
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

        private void UpdateDiceSelected(ref DiceCube dice, GameObject gameObject)
        {
            if (gameObject == null)
            {
                dice = null;
            }
            else
            {
                dice = gameObject?.GetComponent<DiceCube>();
            }
        }

        private void UpdateForgeButtonState()
        {
            bool interactable = targetDice != null && sourceDice != null;
            forgeButton.interactable = interactable;
        }
    }
}
