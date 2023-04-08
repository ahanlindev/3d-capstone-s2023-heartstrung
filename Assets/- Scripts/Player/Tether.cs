using UnityEngine;

public class Tether : MonoBehaviour
{
	// Start is called before the first frame update

	private LineRenderer _lineRenderer;
	public Transform connected;

	// Use this for initialization
	private void Start()
	{
		_lineRenderer = transform.GetComponent<LineRenderer>();
	}

	// Update is called once per frame
	private void Update()
	{
		_lineRenderer.SetPosition(0, transform.position);
		_lineRenderer.SetPosition(1, connected.position);
	}
}
