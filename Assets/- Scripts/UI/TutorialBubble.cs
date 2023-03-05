using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TutorialBubble : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Kitty;

    [SerializeField]
    public Transform VirtualCamera;

    private Vector3 shift;

    private SpriteRenderer spriteRenderer;
    private bool[] _called;

    private int _spriteIndex = -1;

    private float _localTimer;

    [SerializeField]
    public Sprite[] hintSprites;

    public HurtingBush attackedBush;

    //private Tween lastcall;

    void Start()
    {
        _called = new bool[4];
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
        if (_spriteIndex >= 0)
        {
            if (_localTimer > 0)
            {
                _localTimer -= Time.deltaTime;
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
        //Debug.Log("enter changeText");
        //foreach (bool b in _called)
        //{
        //    Debug.Log(b);
        //}
        if (!_called[i])
        {
            //Debug.Log("show content");
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(true);
            _spriteIndex = 2 * i;
            if (attackedBush == null || i != 2)
                _called[i] = true;
            _localTimer = 6f;
        }
    }

    public void cleanText()
    {
        _localTimer = 2.5f;
    }

    private void doClean()
    {
        _spriteIndex = -1;
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
    }

    public void checktheBush(int i)
    {
        //if (attackedBush == null || i == 2)
        //    _called[i] = true;
    }

    private void flicker()
    {
        if (Mathf.Ceil(_localTimer / 0.5f) % 2 == 0)
        {
            spriteRenderer.sprite = hintSprites[_spriteIndex];
        } else
        {
            spriteRenderer.sprite = hintSprites[_spriteIndex + 1];
        }
    }
}
