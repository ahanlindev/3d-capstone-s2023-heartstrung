using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeConstructor : MonoBehaviour
{

    [SerializeField]
    bool reset, spawn;

    [SerializeField]
    GameObject partPrefab, emptyPrefab;

    GameObject parentObject;

    [SerializeField]
    [Range(1, 10)]
    int length = 1;

    [SerializeField]
    float partDistance = 1f;




    Vector3 lastPos;

    // Start is called before the first frame update
    void Start()
    {
        GameObject parent = Instantiate(emptyPrefab, transform.position, Quaternion.identity);
        parentObject = parent;

        GameObject tmp = Instantiate(partPrefab, transform.position, Quaternion.identity, parentObject.transform);
        tmp.name = "constructionCubes0 ";
        tmp.tag = "Construction";
    }

    // Update is called once per frame
    void Update()
    {
        EditorBoxCollisionEnter();
        if (reset)
        {
            foreach (GameObject tmp in GameObject.FindGameObjectsWithTag("Construction"))
            {
                Destroy(tmp);
            }

            reset = false;
        }
        if (spawn)
        {
            Spawn();
            spawn = false;
        }

    }

    public void Spawn()
    {
        GameObject tmp;
        int dirIndex = 0;
        float maxDistanceInAxis = 0;

        // handwritten argmax
        for (int i = 0; i < 3; i++) {
            float distanceInAxis = transform.position[i] - lastPos[i];
            if (Mathf.Abs(distanceInAxis) > Mathf.Abs(maxDistanceInAxis))
            {
                dirIndex = i;
                maxDistanceInAxis = distanceInAxis;
            } 
        }

        Vector3 nextPos = lastPos;
        nextPos[dirIndex] += length;

        tmp = Instantiate(partPrefab, nextPos, Quaternion.identity, parentObject.transform);
        tmp.name = "constructionCubes " + parentObject.transform.childCount.ToString();
        tmp.tag = "Construction";


    }
    // will probably remove this, not necessary
    //private void OnCollisionEnter(Collision collision)
    //{
    //    Transform collidedObj = collision.collider.transform;
    //    if (collidedObj.tag == "Construction")
    //    {
    //        lastPos = collidedObj.position;
    //    }
    //}
    private void EditorBoxCollisionEnter()
    {
        Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position, transform.localScale / 2, Quaternion.identity, ~LayerMask.GetMask("Construction"));
        Debug.Log(hitColliders.Length);
        if (hitColliders.Length == 1)
        {
            lastPos = hitColliders[0].transform.position;
            Debug.Log(lastPos);
        } else if (hitColliders.Length == 0)
        {
            Spawn();
        }
        
        
    }



}
