using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] private PressurePlateAction _targetBehaviour;
    [SerializeField] private GameObject _targetGameObject;

    private bool _pressed = false;
    private Vector3 _pressedTarget;
    private Vector3 _releasedTarget;

    private void Start()
    {
        _pressedTarget = this.transform.position;
        _pressedTarget.y -= .1f;
        _releasedTarget = this.transform.position;
    }
    private void Update()
    {
        if (_pressed)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, _pressedTarget, 0.005f);
        }
        else
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, _releasedTarget, 0.005f);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        _pressed = true;
        if (_targetBehaviour == null)
        {
            Debug.LogError("Pressure Plate target misses beahviour");
        }
        else
        {
            _targetBehaviour.Started();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        _pressed = false;
    }
}
