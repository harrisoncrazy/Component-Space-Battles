using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretComponent : BaseComponent {

    public List<GameObject> shootPoints;

    public GameObject bulletPrefab;

    public KeyCode code;

    public float turretTimerDefault = 0.25f;
    public float turretTimer;

    public Transform slotPos;

    void Start()
    {
        base.Start();
        GetPoints();
    }

	void Update()
    {
        if (slotPos != null)
        {

            this.transform.position = slotPos.transform.position;
            //look at camera
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.orthographicSize;
            Vector3 objectPos = Camera.main.WorldToScreenPoint(this.transform.position);
            mousePos.x = mousePos.x - objectPos.x;
            mousePos.y = mousePos.y - objectPos.y;
            float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
            transform.localRotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
        }
    }

    public void GetPoints()
    {
        foreach (Transform child in this.transform)
        {
            if (child.name == "point")
            {
                shootPoints.Add(child.gameObject);
            }
        }
    }

    public void Fire()
    {
        foreach(GameObject point in shootPoints)
        {
            ProjectileComponent bullet = Instantiate(bulletPrefab, point.transform.position, point.transform.rotation).GetComponent<ProjectileComponent>();
            bullet.mainRB.AddForce(transform.up * 100);
            bullet.parentShip = this.transform.parent.gameObject;
        }
    }
}
