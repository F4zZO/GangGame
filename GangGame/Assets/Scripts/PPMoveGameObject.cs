using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PPMoveGameObject : PressurePlateAction
{
    [SerializeField] private float _moveSpeed = .5f;
    [SerializeField] private Vector3 _startPos;

    private Vector3 _target;
    private bool _started = false;
    // Start is called before the first frame update
    void Start()
    {
        _target = this.transform.position;
        this.transform.position = _target - _startPos;
    }

    // Update is called once per frame
    void Update()
    {
        if(_started)
            this.transform.position = Vector3.MoveTowards(this.transform.position, _target, _moveSpeed);
    }

    public override void Started()
    {
        _started = true;
    }
}
