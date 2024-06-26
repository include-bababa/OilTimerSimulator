using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabController : MonoBehaviour
{
    [SerializeField]
    private TabItem[] tabItems;

    [SerializeField]
    private GameObject[] tabContents;

    private void Start()
    {
        this.SwitchTo(0);
    }

    public void SwitchTo(int tabIndex)
    {
        // switch tabs
        {
            var num = this.tabItems.Length;
            for (var index = 0; index < num; index++)
            {
                this.tabItems[index].SetHighlight(index == tabIndex);
            }
        }

        // switch contents
        {
            var num = this.tabContents.Length;
            for (var index = 0; index < num; index++)
            {
                this.tabContents[index].SetActive(index == tabIndex);
            }
        }
    }
}
