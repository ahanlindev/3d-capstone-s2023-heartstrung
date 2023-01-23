using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class String : MonoBehaviour
{
	// Start is called before the first frame update

	private LineRenderer lineRenderer;
	public Transform connected;

	// Use this for initialization
	void Start()
	{
		lineRenderer = transform.GetComponent<LineRenderer>();
	}

	// Update is called once per frame
	void Update()
	{
		lineRenderer.SetPosition(0, transform.position);
		lineRenderer.SetPosition(1, connected.position);
	}
}
