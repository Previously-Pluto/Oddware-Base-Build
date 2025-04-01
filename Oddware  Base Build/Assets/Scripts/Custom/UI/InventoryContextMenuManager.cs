using NaninovelInventory;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryContextMenuManager : MonoBehaviour
{
    [SerializeField] private InputActionReference inventoryContextInput;

    public static InventoryContextMenuManager Instance;
    private ContextMenuManager menuManager;
    private InventoryGrid inventoryGrid;
    
    private void Awake()
    {
        inventoryGrid = FindObjectOfType<InventoryGrid>();
        Instance = this;
    }

    private void OnEnable()
    {
        inventoryContextInput.action.Enable();
        inventoryContextInput.action.started += ToggleContextMenu;
    }
    
    private void OnDisable()
    {
        inventoryContextInput.action.Disable();
        inventoryContextInput.action.started -= ToggleContextMenu;
    }

    public void ToggleInput(bool toggle)
    {
        if (toggle)
            inventoryContextInput.action.Enable();
        else
            inventoryContextInput.action.Disable();
    }

    private void ToggleContextMenu(InputAction.CallbackContext callbackContext)
    {
        if(menuManager.SelectedItem == null)
            menuManager.CloseContextMenu();
        else
            menuManager.OpenContextMenu(menuManager.SelectedItem);
    }
}