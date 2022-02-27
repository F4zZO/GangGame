using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Hangable : MonoBehaviour
{
    public abstract void Attach(Vector3 dir, Rigidbody hangingRB);
    public abstract void Detach();
}
