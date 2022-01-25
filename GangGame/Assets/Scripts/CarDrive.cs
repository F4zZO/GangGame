using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDrive : MonoBehaviour
{
    private Rigidbody body;
    private float timer = 5;
    private float speed;
    private float force;

    void Start()
    {
        this.body = this.GetComponent<Rigidbody>();
        this.speed = Random.Range(1f, 2.5f);
        this.force = Random.Range(20, 80);
    }

    void Update()
    {
        this.timer -= Time.deltaTime;

        if (this.timer <= 0f)
        {
            Destroy(this.gameObject);
        }
    }

    void FixedUpdate()
    {
        this.body.MovePosition(this.transform.position - (Vector3.forward / this.speed));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            collision.gameObject.GetComponent<Rigidbody>().AddForce((collision.gameObject.transform.position - this.transform.position).normalized * this.force, ForceMode.Impulse);
        }
        
        if (collision.collider.tag == "Death")
        {
            Destroy(this.gameObject);
        }
    }
}
