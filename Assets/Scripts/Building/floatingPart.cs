﻿using System.Collections;
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

    public string partID;
    public bool isFlippedX;
    public bool isFlippedY;

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

                //Flipping object in x
                if (Input.GetKeyDown(KeyCode.X))
                {
                    Vector3 tempContainer = transform.localScale;
                    transform.localScale = new Vector3(tempContainer.x * -1, tempContainer.y, tempContainer.z);

                    isFlippedX = !isFlippedX;
                }

                //Flipping object in y
                if (Input.GetKeyDown(KeyCode.C))
                {
                    Vector3 tempContainer = transform.localScale;
                    transform.localScale = new Vector3(tempContainer.x, tempContainer.y*-1, tempContainer.z);

                    isFlippedY = !isFlippedY;
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

                //Flipping object in x
                if (Input.GetKeyDown(KeyCode.X))
                {
                    Vector3 tempContainer = transform.parent.transform.localScale;
                    transform.parent.transform.localScale = new Vector3(tempContainer.x * -1, tempContainer.y, tempContainer.z);

                    foreach (Transform child in transform.parent.transform)
                    {
                        child.GetComponent<floatingPart>().isFlippedX = !child.GetComponent<floatingPart>().isFlippedX;
                    }
                }

                //Flipping object in y
                if (Input.GetKeyDown(KeyCode.C))
                {
                    Vector3 tempContainer = transform.parent.transform.localScale;
                    transform.parent.transform.localScale = new Vector3(tempContainer.x, tempContainer.y*-1, tempContainer.z);

                    foreach (Transform child in transform.parent.transform)
                    {
                        child.GetComponent<floatingPart>().isFlippedY = !child.GetComponent<floatingPart>().isFlippedY;
                    }
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
