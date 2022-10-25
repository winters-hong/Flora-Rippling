using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class eventTest : MonoBehaviour
{
	private float t;


	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}
	public void TestFunction()
	{

		t = Random.Range(0.3f, 0.7f);
		this.GetComponent<Image>().color = new Color(1, 1, 1, t);

	}
	public void TestFunction2()
	{
		this.GetComponent<Image>().color = new Color(1, 1, 1, t - 0.1f);
	}
}
