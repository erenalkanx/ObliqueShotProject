using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPanel : MonoBehaviour
{

    public GameObject developerPanel;
    public GameObject howToPlayPanel;

    void Start()
    {
        developerPanel.SetActive(false);
        howToPlayPanel.SetActive(false);
    }

    void Update()
    {

    }

    public void OyunaBasla()
    {
        Application.LoadLevel("Scene1");
    }

    public void Cikis()
    {
        Application.Quit();
    }

    public void GelistiriciPanel()
    {
        developerPanel.active = !developerPanel.active;
        howToPlayPanel.SetActive(false);
    }

    public void NasilOynanirPanel()
    {
        howToPlayPanel.active = !howToPlayPanel.active;
        developerPanel.SetActive(false);
    }

}
