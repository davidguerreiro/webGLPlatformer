using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceDropItem : MonoBehaviour
{
    [System.Serializable]
    public struct DroppableItem
    {
        public GameObject collectable;
        public float dropRate;
    }

    public DroppableItem[] items;

    /// <summary>
    /// Drop item into game scene. Usually
    /// called when enemy is defeated.
    /// </summary>
    public void DropItem()
    {
        int itemIndex = Random.Range(0, 2);

        DroppableItem item = items[itemIndex];

        if (Random.Range(0, 100f) < item.dropRate)
        {
            GameObject instance = Instantiate(item.collectable);

            instance.transform.position = transform.position;

            SpaceCollectable collectable = instance.GetComponent<SpaceCollectable>();
            collectable.Init();
        }
    }
}
