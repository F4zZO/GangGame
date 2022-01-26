using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class SandStorm : MonoBehaviour
{
    [SerializeField] private float forceNormal;
    [SerializeField] private float forceStrong;

    [SerializeField] private float forceStrongTime;

    [SerializeField] private float currentForce;

    private Timer timerStrong;

    void Start()
    {
        this.timerStrong = new Timer();
        this.timerStrong.Elapsed += this.EndForceStrong;
    }

    void Update()
    {
        
    }

    public void EndForceStrong(object source, ElapsedEventArgs e)
    {
        this.currentForce = this.forceStrong;
        this.timerStrong.Stop();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            Rigidbody rig = other.gameObject.GetComponent<Rigidbody>();
            rig.AddForce((this.transform.position - other.gameObject.transform.position) * this.currentForce);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            this.timerStrong.Interval = this.forceStrongTime;
            this.timerStrong.Start();
            this.currentForce = this.forceNormal;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            this.timerStrong.Stop();
        }
    }
}
