using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangableZipline : Hangable
{
    [SerializeField] private Transform _ziplineStart;
    [SerializeField] private Transform _ziplineEnd;
    [SerializeField] private float _timeToComplete;

    [SerializeField] private float _releaseAccelerationFactor = 1;
    [SerializeField] private float _speedBoost = 1;

    private float _elapsedTime = 0f;
    private bool _started;
    private Rigidbody _hangingRB;

    private Vector3 oldpos;
    private Vector3 newpos;
    private Vector3 velocity;
    public override void Attach(Vector3 dir, Rigidbody hangingRB)
    {
        Debug.Log("Attached to Zipline");
        _started = true;
        _hangingRB = hangingRB;
    }

    public override void Detach()
    {
        _started = false;
        _elapsedTime = 0;
        this.transform.position = _ziplineStart.position;
        _hangingRB.velocity = velocity * _releaseAccelerationFactor;
    }

    private void Start()
    {
        oldpos = transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        if (_started)
        {
            this.transform.position = Vector3.Lerp(_ziplineStart.position, _ziplineEnd.position, (_elapsedTime / _timeToComplete));
            _elapsedTime += Time.deltaTime;
            if (Input.GetKey(KeyCode.W))
            {
                _elapsedTime += Time.deltaTime * _speedBoost;
            }
        }   

        if(_elapsedTime > _timeToComplete - 0.06)
        {
            _started = false;
        }
        newpos = transform.position;
        var media = (newpos - oldpos);
        velocity = media / Time.deltaTime;
        oldpos = newpos;
        newpos = transform.position;
    }
}
