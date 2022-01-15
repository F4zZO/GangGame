using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    public float Speed;
    public float JumpForce;
    private Rigidbody body;
    private bool onGround;
    // Start is called before the first frame update
    void Start()
    {
         body = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (transform.position.y < -20) 
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
        if (Input.GetKey(KeyCode.W))
        {
            this.transform.position += new Vector3(Speed/100,0,0);
        }

        if (Input.GetKey(KeyCode.A))
        {
            this.transform.position += new Vector3(0,0,Speed/100);
        }

        if (Input.GetKey(KeyCode.S))
        {
            this.transform.position += new Vector3(-Speed/100,0,0);
        }

        if (Input.GetKey(KeyCode.D))
        {
            this.transform.position += new Vector3(0,0,-Speed/100);
        }
         if (Input.GetKeyDown(KeyCode.Space))
        {
            if (onGround)
            {
                this.body.AddForce(Vector3.up * JumpForce);
                onGround = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Floor")
        {
            onGround = true;
        }    
    }
}
