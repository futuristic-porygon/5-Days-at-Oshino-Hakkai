using UnityEngine;
using UnityEngine.UI;

public class TabController : MonoBehaviour
{

    public Image[] TabImages;
    public GameObject[] pages;

    void Start()
    {
        ActivateTab(0);
    }

    public void ActivateTab(int tabNo)
    {
        for (int i = 0; i < TabImages.Length; i++)
        {
            pages[i].SetActive(false);
            TabImages[i].color = Color.grey;
        }

        pages[tabNo].SetActive(true);
        TabImages[tabNo].color = Color.white;
    }


 
}
