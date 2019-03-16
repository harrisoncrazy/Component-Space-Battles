using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class partUIElement : MonoBehaviour
{
    public Image mainImage;
    public Text mainText;

    [SerializeField] private GameObject floatingPartPref;

    void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        //Instanciating floating part
        GameObject prefab = Instantiate(floatingPartPref, transform.position, transform.rotation) as GameObject;
        prefab.transform.SetParent(GameObject.Find("Canvas").transform, false);
        prefab.transform.localPosition = new Vector3(0, 0);
        prefab.GetComponent<floatingPart>().mainSprite.sprite = this.mainImage.sprite;
    }
}
