using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyColor : MonoBehaviour
{
    public FlexibleColorPicker fcp;
    public Material material;

    private void Start()
    {
        if (GameManager.Instance.playercolor == new Color(0, 0, 0, 0)) return;

        this.fcp.color = GameManager.Instance.playercolor;
    }

    private void Update()
    {
        material.color = fcp.color;
        GameManager.Instance.playercolor = fcp.color;
    }
}
