using System.Collections;
using System.Collections.Generic;
using System.Text;
using System; 
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class ShopManager : MonoBehaviour
{
    public GameObject inventoryItemPrefab; 
    public static ShopManager instance;
    Item[] itemsInShop; 
    public bool[] isBought; 
    public InventorySlot[] inventorySlots;
    public TextMeshProUGUI[] priceTexts; 
    public TextMeshProUGUI[] itemNames; 
    public TextMeshProUGUI successFailMessage; 
    public Button[] buyButtons; 

    public GameObject shopUI; 
    

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

    public void ShowShop(Item[] items){
        itemsInShop = items;
        for (int i = 0; i < items.Length; i++){
            InventoryItem itm = inventorySlots[i].GetComponentInChildren<InventoryItem>();
            if (itm == null && items[i] != null){
                SpawnNewItem(items[i], inventorySlots[i]); 
                priceTexts[i].text = "Price: " + (items[i].price).ToString(); 
                itemNames[i].text = items[i].itemName.ToString(); 
            }
        }
        shopUI.SetActive(true); 
    }

    
    public void CloseShop(){
        shopUI.SetActive(false); 
    } 

    public void UnstockItem(int slotIdx){
        buyButtons[slotIdx].gameObject.SetActive(false); // Disable the button's GameObject
        priceTexts[slotIdx].gameObject.SetActive(false); // Disable the price text's GameObject
        itemNames[slotIdx].gameObject.SetActive(false); // Disable the item name text's GameObject
        InventoryItem itm = inventorySlots[slotIdx].GetComponentInChildren<InventoryItem>();
        if (itm != null){
            itm.gameObject.SetActive(false); // Disable the inventory item's GameObject
        }
        itemsInShop[slotIdx] = null; 
    }   

    public void BuyItem(int slotIdx){
        successFailMessage.text = "";
        if (CoinCounter.Instance.currentCoins >= itemsInShop[slotIdx].price){
            successFailMessage.text = "Item purchased!"; 
            successFailMessage.color = Color.green;
            InventoryManager.instance.AddItem(itemsInShop[slotIdx]); 
            UnstockItem(slotIdx); 
        }
        else {
            Debug.Log("not enough"); 
            successFailMessage.text = "Not enough coins!"; 
            successFailMessage.color = Color.red;
        }
        StartCoroutine(showSuccessFailMessage()); 
        //CloseShop();  
    }

    private IEnumerator showSuccessFailMessage(){
        successFailMessage.enabled = true; 
        yield return new WaitForSeconds(3f);
        successFailMessage.enabled = false; 
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
