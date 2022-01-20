using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class PlayerUI : MonoBehaviour
{
    public Canvas playerUI;

    [Header("KeyUI")]
    public GameObject keyImagePrefab;
    public GameObject LoseScreen;
    public GameObject WinScreen;
    public float borderSize = 10;

    private int keyCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addKey(SpriteRenderer renderer)
    {
        GameObject key = Instantiate(keyImagePrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;

        key.transform.SetParent(playerUI.transform, false);

        RectTransform rectTransform = key.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(1, 0);
        rectTransform.anchorMax = new Vector2(1, 0);

        float x = rectTransform.rect.width / 2;
        float y = rectTransform.rect.height / 2;
        rectTransform.anchoredPosition = new Vector3( -1 * (x + this.borderSize + (x * 2 + this.borderSize) * keyCount), y + this.borderSize, 0);
        Image image = key.GetComponent<Image>();
        image.sprite = renderer.sprite;
        image.color = renderer.color;

        keyCount++;
    }

    public void showLoseScreen()
    {
        LoseScreen.SetActive(true);
        GameObject.FindGameObjectsWithTag("Enemy").ToList().ForEach(x => x.SetActive(false));
    }

    public void showWinScreen()
    {
        WinScreen.SetActive(true);
    }
}
