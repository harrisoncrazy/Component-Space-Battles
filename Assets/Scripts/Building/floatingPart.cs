using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class floatingPart : MonoBehaviour
{
    public SpriteRenderer mainSprite;

    private bool followingMouse = true;
    private bool mouseOnObject = true;

    public List<GameObject> installSlots;

    private float timer = 1.0f;

    public void Update()
    {
        //if object hovered, and not following mouse, 1 second delay to pick up
        if (Input.GetMouseButton(0) && mouseOnObject && !followingMouse)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                followingMouse = true;
            }
        }
        else 
        {
            //resetting timer if not held
            timer = 1.0f;
        }

        if (followingMouse)
        {
            if (transform.parent == null)//if object is alone
            {
                Vector3 pos = Input.mousePosition;
                pos.z = transform.position.z - Camera.main.transform.position.z;
                transform.position = Camera.main.ScreenToWorldPoint(pos);

                //putting object down
                if (Input.GetMouseButtonDown(0))
                {
                    followingMouse = false;
                }
            }
            else
            {
                Vector3 pos = Input.mousePosition;
                pos.z = transform.position.z - Camera.main.transform.position.z;
                transform.parent.position = Camera.main.ScreenToWorldPoint(pos);

                //putting object down
                if (Input.GetMouseButtonDown(0))
                {
                    followingMouse = false;
                }
            }
        }
    }

    void OnMouseOver()
    {
        mouseOnObject = true;
    }

    void OnMouseExit()
    {
        mouseOnObject = false;
    }

    public InstallSlot[] slots;

    public void getSlots()
    {
        //getting all children install slot objects
        slots = this.transform.GetComponentsInChildren<InstallSlot>();

        foreach (InstallSlot slot in slots)
        {
            slot.InstallJoint();
        }
    }
}
