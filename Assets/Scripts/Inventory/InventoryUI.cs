using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [Header("List Settings")]
    
    [SerializeField] private Transform contentParent;
    [SerializeField]private GameObject buttonPrefab;
    
    [Header("Display Area")]
    [SerializeField] private TextMeshProUGUI itemName;

    [SerializeField] private TextMeshProUGUI itemDescription;

    [SerializeField] private Button readButton;
    [SerializeField] private Note noteSystem;


    public void RefreshUI()
    {
        // Clearing old list
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);

        // Building new list
        foreach (ItemData item in InventoryManager.Instance.items)
        {
            GameObject newBtn = Instantiate(buttonPrefab, contentParent);
            newBtn.GetComponentInChildren<TextMeshProUGUI>().text = item.itemName;

            newBtn.GetComponent<Button>().onClick.AddListener(() => SelectItem(item));
        }

        itemName.text = "";
        itemDescription.text = "";

        Canvas.ForceUpdateCanvases();
    }

    private void SelectItem(ItemData item)
    {
        itemName.text = item.itemName;
        itemDescription.text = item.itemDescription;

        if (item.type == ItemType.Note && !string.IsNullOrEmpty(item.fullNoteContent))
        {
            readButton.gameObject.SetActive(true);
            readButton.onClick.RemoveAllListeners();
            readButton.onClick.AddListener(() =>
            {
                InventoryManager.Instance.inventoryPanel.SetActive(false);
                noteSystem.OpenFromInventory(item);
            });
        }
        
        else
        {
            readButton.gameObject.SetActive(false);
        }
    }
}
