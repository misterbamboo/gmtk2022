using UnityEngine;
using UnityEngine.Events;

namespace DiceGame
{
    public abstract class DragDropBaseComponent : MonoBehaviour
    {
        [SerializeField] public UnityEvent<DragDropBaseComponent, int, Vector2> onDrag;
        [SerializeField] public UnityEvent<DragDropBaseComponent, int, Vector2> onDrop;

        public abstract bool TryDropObjectByKey(int key, Transform dragTransform);
        public abstract Transform DragObjectByKey(int objectKey);
    }
}
