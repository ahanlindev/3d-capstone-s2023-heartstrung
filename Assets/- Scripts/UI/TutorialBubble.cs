using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TutorialBubble : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Kitty;
    public Transform VirtualCamera;
    private Vector3 shift;

    private SpriteRenderer spriteRenderer;
    private bool[] called;

    private int spriteIndex = -1;

    private float localTimer;

    [SerializeField]
    public Sprite[] hintSprites;

    public HurtingBush attackedBush;

    private Tween lastcall;

    void Start()
    {
        called = new bool[4];
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);

        spriteRenderer = GameObject.Find("ChatBubble").transform.GetChild(1).GetComponent<SpriteRenderer>();
        shift = transform.position - Kitty.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Kitty.transform.position + shift;
        Vector3 forwardDirection = (transform.position - VirtualCamera.transform.position).normalized;
        transform.LookAt(transform.position + forwardDirection, VirtualCamera.transform.up);
        if (spriteIndex >= 0)
        {
            if (localTimer > 0)
            {
                localTimer -= Time.deltaTime;
                flicker();
            }
            else
            {
                doClean();
            }
        }
    }

    public void changeText(int i)
    {

        if (!called[i])
        {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(true);
            spriteIndex = 2 * i;
            if (attackedBush == null || i != 2)
                called[i] = true;
            localTimer = 6f;
        }
    }

    public void cleanText()
    {
        localTimer = 2.5f;
    }

    private void doClean()
    {
        spriteIndex = -1;
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
    }

    public void checktheBush(int i)
    {
        if (attackedBush == null || i == 2)
            called[i] = true;
    }

    private void flicker()
    {
        if (Mathf.Ceil(localTimer / 0.5f) % 2 == 0)
        {
            spriteRenderer.sprite = hintSprites[spriteIndex];
        } else
        {
            spriteRenderer.sprite = hintSprites[spriteIndex + 1];
        }
    }
}
