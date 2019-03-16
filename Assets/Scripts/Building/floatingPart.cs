using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class floatingPart : MonoBehaviour
{
    public SpriteRenderer mainSprite;

    private bool followingMouse = true;

    public void Update()
    {
        if (followingMouse)
        {
            Vector3 pos = Input.mousePosition;
            pos.z = transform.position.z - Camera.main.transform.position.z;
            transform.position = Camera.main.ScreenToWorldPoint(pos);
        }
    }

    void OnMouseDown()
    {
        followingMouse = !followingMouse;
    }
}
