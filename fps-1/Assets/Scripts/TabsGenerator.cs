using UnityEngine;
using UnityEngine.UI;

public class TabsGenerator : MonoBehaviour {

    public GameObject button;
    public GameObject[] panels;
    private GameObject activePanel = null;


    void Start()
    {
        GenerateTabs();
        ActivatePanel(this.panels[0]);
    }

    void GenerateTabs()
    {
        foreach (GameObject panel in this.panels)
        {
            GameObject tab = Instantiate(this.button) as GameObject;
            tab.transform.SetParent(this.transform, false);
            tab.GetComponentInChildren<Text>().text = panel.name;
            tab.GetComponent<Button>().onClick.AddListener(() => ActivatePanel(panel));
        }
    }

    void ActivatePanel(GameObject panel)
    {
        if (this.activePanel != null)
        {
            this.activePanel.SetActive(false);
        }
        panel.SetActive(true);
        this.activePanel = panel;
    }
}
