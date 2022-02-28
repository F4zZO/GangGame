using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyColor : MonoBehaviour
{
    public FlexibleColorPicker fcp;
    public Material material;

    private void Start()
    {
        if (GameManager.Instance.playerColor == new Color(0, 0, 0, 0)) return;

        this.fcp.color = GameManager.Instance.playerColor;
    }

    private void Update()
    {
        material.color = fcp.color;
        GameManager.Instance.playerColor = fcp.color;
    }
}
