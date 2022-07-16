using UnityEngine;

public class DecisionScreenLoad : MonoBehaviour
{
    [SerializeField] private GameObject prefab;

    public void Load()
    {
        Instantiate(prefab, GameObject.Find("Canvas").transform);
    }
}
