using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buildComponent : MonoBehaviour {

    public InstallSlot[] listSlots;
    [SerializeField] private GameObject slotPointPrefab;
    public bool followCursor = false;

    // Use this for initialization
    void Start () {
        StartCoroutine(delay());
	}
	
    IEnumerator delay()
    {
        yield return new WaitForSeconds(0.5f);
        listSlots = this.gameObject.GetComponent<BaseComponent>().slots;
        GeneratePoints();
    }

	void GeneratePoints()
    {
        foreach(InstallSlot slot in listSlots)
        {
            GameObject point = GameObject.Instantiate(slotPointPrefab, slot.transform.position, slot.transform.rotation);
            point.transform.parent = slot.gameObject.transform;
        }
    }

    void Update()
    {
        if (followCursor == true)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10.0f;
            this.gameObject.transform.position = Camera.main.ScreenToWorldPoint(mousePos);
        }
    }
}
