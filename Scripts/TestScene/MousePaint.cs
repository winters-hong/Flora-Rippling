using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MousePaint : MonoBehaviour
{
	[Range(0, 60)]
	public int sensitivity = 10; //绘制灵敏度
	float totalPaintTime = 0.0f;
	public GameObject mousePaintCanvas;
	GameObject ImagePrefab;

	int colorIndex = 1;
	int brushIndex = 1;
	float brushScale = 1;
	string brushSource;
	GameController gameController;
	public List<GameObject> elements;

	// Start is called before the first frame update
	void Start()
	{
		gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
	}

	// Update is called once per frame
	void Update()
	{
		if (gameController.screenState_H != ScreenState_H.LA_RAN)
			return;
		if (Input.GetMouseButton(0))
		{
			totalPaintTime += Time.deltaTime;
			if (totalPaintTime >= 1.0f / sensitivity)
			{
				totalPaintTime = 0.0f;

				brushSource = "Prefabs/Brushes/brush" + colorIndex;
				brushIndex = Random.Range(1, 4);
				brushSource += brushIndex;
				ImagePrefab = (GameObject)Resources.Load(brushSource);

				elements.Add(Instantiate(ImagePrefab, Vector3.zero, ImagePrefab.transform.rotation) as GameObject);
				elements[elements.Count - 1].GetComponent<RectTransform>().position = Input.mousePosition;
				elements[elements.Count - 1].transform.SetParent(mousePaintCanvas.transform);
				elements[elements.Count - 1].GetComponent<RectTransform>().rotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360)));
				brushScale = Random.Range(0.4f, 1.5f);
				//elements[elements.Count - 1].GetComponent<RectTransform>().localScale = new Vector3(brushScale, brushScale, brushScale);
				elements[elements.Count - 1].GetComponent<Image>().color = new Color(1, 1, 1, Random.Range(0.3f, 0.7f));
				/*
								elements.Add(Instantiate(ImagePrefab, Vector3.zero, ImagePrefab.transform.rotation) as GameObject);
								elements[elements.Count - 1].GetComponent<RectTransform>().position = new Vector3(2048, 2048, 0) - Input.mousePosition;
								elements[elements.Count - 1].transform.SetParent(mousePaintCanvas.transform);
								elements[elements.Count - 1].GetComponent<RectTransform>().rotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360)));
								brushScale = Random.Range(0.5f, 2.5f);
								//elements[elements.Count - 1].GetComponent<RectTransform>().localScale = new Vector3(brushScale, brushScale, brushScale);
								elements[elements.Count - 1].GetComponent<Image>().color = new Color(1, 1, 1, Random.Range(0.3f, 0.7f));

								elements.Add(Instantiate(ImagePrefab, Vector3.zero, ImagePrefab.transform.rotation) as GameObject);
								float x = 0,y = 0;
								x = 2048 - Input.mousePosition.x;
								y = Input.mousePosition.y;
								elements[elements.Count - 1].GetComponent<RectTransform>().position = new Vector3(x, y, 0);
								elements[elements.Count - 1].transform.SetParent(mousePaintCanvas.transform);
								elements[elements.Count - 1].GetComponent<RectTransform>().rotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360)));
								brushScale = Random.Range(0.5f, 2.5f);
								//elements[elements.Count - 1].GetComponent<RectTransform>().localScale = new Vector3(brushScale, brushScale, brushScale);
								elements[elements.Count - 1].GetComponent<Image>().color = new Color(1, 1, 1, Random.Range(0.3f, 0.7f));

								elements.Add(Instantiate(ImagePrefab, Vector3.zero, ImagePrefab.transform.rotation) as GameObject);
								x = Input.mousePosition.x;
								y = 2048 - Input.mousePosition.y;
								elements[elements.Count - 1].GetComponent<RectTransform>().position = new Vector3(x, y, 0);
								elements[elements.Count - 1].transform.SetParent(mousePaintCanvas.transform);
								elements[elements.Count - 1].GetComponent<RectTransform>().rotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360)));
								brushScale = Random.Range(0.5f, 2.5f);
								//elements[elements.Count - 1].GetComponent<RectTransform>().localScale = new Vector3(brushScale, brushScale, brushScale);
								elements[elements.Count - 1].GetComponent<Image>().color = new Color(1, 1, 1, Random.Range(0.3f, 0.7f));				
				*/
			}
		}
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			for (int i = 0; i < elements.Count; i++)
				Destroy(elements[i]);
			elements.Clear();
		}
		if (Input.GetKey(KeyCode.Z) && elements.Count > 0)
		{
			Destroy(elements[elements.Count - 1]);
			elements.Remove(elements[elements.Count - 1]);
		}
		if (Input.GetKeyDown(KeyCode.Q))
			colorIndex = colorIndex % 5 + 1;
		if (Input.GetKeyDown(KeyCode.E))
			brushIndex = brushIndex % 3 + 1;
	}
}
