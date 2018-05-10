using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileComponent : MonoBehaviour
{
    public GameObject parentShip;

    public Rigidbody2D mainRB;
    public float projectileSpeed = 2;
    public float maxVelocity = 3;
    public float damageValue = 2;
    public string damageType = "base";

    private float deletionTimer = 5.0f;

    void Start()
    {
        mainRB = this.GetComponent<Rigidbody2D>();

        mainRB.AddForce(transform.up * projectileSpeed, ForceMode2D.Impulse);
    }

    void FixedUpdate()
    {
        //MoveForward();

        deletionTimer -= Time.deltaTime;

        if (deletionTimer <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void MoveForward()
    {
        mainRB.AddForce(transform.up * projectileSpeed);

        ClampVelocity();
    }

    public void ClampVelocity()
    {
        float x = Mathf.Clamp(mainRB.velocity.x, -maxVelocity, maxVelocity);
        float y = Mathf.Clamp(mainRB.velocity.y, -maxVelocity, maxVelocity);

        mainRB.velocity = new Vector2(x, y);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<BaseComponent>())
        {
            if (other.transform.parent.gameObject != parentShip)
            {
                other.GetComponent<BaseComponent>().takeDamage(damageValue,damageType);
                Destroy(this.gameObject);
            }
        }
    }
}
