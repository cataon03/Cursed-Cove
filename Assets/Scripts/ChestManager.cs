using System.Collections;
using System.Collections.Generic;
using System.Text;
using System; 
using UnityEngine;

public class ChestManager : MonoBehaviour
{
    public GameObject inventoryItemPrefab; 
    public static ChestManager instance;
    Item itemInChest; 
    public InventorySlot inventorySlot;
    public GameObject insideChest; 
    

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void ShowInsideChest(){
        insideChest.SetActive(true); 
    }

    public void HideInsideChest(){
        insideChest.SetActive(false); 
    }

    public void LoadItemIntoChest(Item item) {
        itemInChest = item; 
        SpawnNewItem(item, inventorySlot); 
    }

    public void TakeItem(){
        InventoryManager.instance.AddItem(itemInChest); 
        HideInsideChest(); 
    }

    void SpawnNewItem(Item item, InventorySlot slot) {
        GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        RectTransform rectTransform = newItemGo.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(100, 100);

        rectTransform.pivot = new Vector2(0.5f, 0.5f);

        // Set anchors to the center
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.anchoredPosition = Vector2.zero; // Center in slot
        rectTransform.localScale = Vector3.one; // Ensure it's not scaled weirdly

        inventoryItem.InitialiseItem(item);
    }

}
