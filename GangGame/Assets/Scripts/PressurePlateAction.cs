using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PressurePlateAction : MonoBehaviour
{
    public abstract void Started();
    public virtual void Stopped() { return; }
}
