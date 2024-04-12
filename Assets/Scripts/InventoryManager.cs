using System.Collections;
using System.Collections.Generic;
using System.Text;
using System; 
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    public int maxStackedItems = 4;
    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;
    public static event Action<string> OnItemEquipped; 
    public static event Action<Item> OnPowerupEquipped;

    int selectedSlot = -1;

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

    private void Start() {
        //ChangeSelectedSlot(0);
    }

    private void Update() {
        if (Input.inputString != null) {
            bool isNumber = int.TryParse(Input.inputString, out int number);
            if (isNumber && number > 0 && number < 8) {
                ChangeSelectedSlot(number - 1);
            }
        }
    }

    public void SetInventoryItemPrefab(GameObject itemPrefab){
        inventoryItemPrefab = itemPrefab; 
    }

    public void ChangeSelectedSlot(int newValue) {
        if (selectedSlot >= 0) {
            inventorySlots[selectedSlot].Deselect();
        }
        //inventorySlots[newValue].Select();

        InventoryItem itemInSlot = inventorySlots[newValue].GetComponentInChildren<InventoryItem>();
        
        if (itemInSlot){
            // If the player clicked on a weapon notify listeners 
            if (itemInSlot.item.type == ItemType.Weapon){
                Debug.Log("Selecting a weapon in the inventory"); 
                OnItemEquipped?.Invoke(itemInSlot.item.name); 
            }
            if (itemInSlot.item.type == ItemType.Powerup){
                Debug.Log("powerup selected"); 
                removeExactItem(newValue); 
                OnPowerupEquipped?.Invoke(itemInSlot.item);  
            }
        }
        selectedSlot = newValue;
    }

    public void removeExactItem(int itemIndex){
            InventorySlot slot = inventorySlots[itemIndex];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            
            if (itemInSlot != null) {
                Destroy(itemInSlot.gameObject);
            }
    }

    public void RemoveItem(Item item) {
        int itemIndex = findItemIndex(item); 
        if (itemIndex != -1){
            InventorySlot slot = inventorySlots[itemIndex];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            
            if (itemInSlot != null) {
                Destroy(itemInSlot.gameObject);
            }
        }
    }

    public bool HasItem(Item item) {
        if (findItemIndex(item) != -1){
            return true; 
        } 
        return false; 
    }

    // Returns the slot index that the item is in, otherwise returns -1 
    public int findItemIndex(Item item) {
        for (int i = 0; i < inventorySlots.Length; i++) {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            
            if (itemInSlot != null &&
                itemInSlot.item == item) {
                return i; 
            }
        }
        return -1; 
    }

    public bool AddItem(Item item) {
        // Check if any slot has the same item with count lower than max
        for (int i = 0; i < inventorySlots.Length; i++) {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null &&
                itemInSlot.item == item &&
                itemInSlot.count < maxStackedItems &&
                itemInSlot.item.stackable == true) {

                itemInSlot.count++;
                itemInSlot.RefreshCount();
                return true;
            }
        }

        // Find any empty slot
        for (int i = 0; i < inventorySlots.Length; i++) {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null) {
                SpawnNewItem(item, slot);
                return true;
            }
        }

        return false;
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

    public Item GetSelectedItem(bool use) {
        InventorySlot slot = inventorySlots[selectedSlot];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
        if (itemInSlot != null) {
            Item item = itemInSlot.item;
            if (use == true) {
                itemInSlot.count--;
                if (itemInSlot.count <= 0) {
                    Destroy(itemInSlot.gameObject);
                } else {
                    itemInSlot.RefreshCount();
                }
            }

            return item;
        }

        return null;
    }

}
