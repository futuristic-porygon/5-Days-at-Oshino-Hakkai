using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject menuCanvas;

    private bool isGiftSelectionActive;

    private void Start()
    {
        menuCanvas.SetActive(false);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (isGiftSelectionActive)
            {
                return;
            }
            if (!menuCanvas.activeSelf && PauseManager.isGamePaused)
            {
                return;
            }
            menuCanvas.SetActive(!menuCanvas.activeSelf);
            PauseManager.SetPause(menuCanvas.activeSelf);

        }
    }
    public void OpenInventoryForGift()
    {
        isGiftSelectionActive = true;
        menuCanvas.SetActive(true);
    }

    public void CloseInventoryForGift()
    {
        isGiftSelectionActive = false;
        menuCanvas.SetActive(false);
    }
}
