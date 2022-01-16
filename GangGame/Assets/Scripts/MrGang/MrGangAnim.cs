using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MrGangAnim : MonoBehaviour
{

    public Transform[] Body;
    public Transform[] Anim;

    public ConfigurableJoint[] bodyJoint;

    void Start()
    {
        
    }

    void Update()
    {
        for(int i = 0; i < this.Body.Length; i++)
        {
            //this.Body[i] = this.Anim[i];
            //bodyJoint[i].targetRotation = Anim.

        }
        
    }
}
