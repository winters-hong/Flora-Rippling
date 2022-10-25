using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;

public class HandPaint : MonoBehaviour
{
	GameController gameController;
	public RectTransform PenRect;
	GameObject Pen;

	[Range(0, 60)]
	public int sensitivity = 10; //绘制灵敏度
	float totalPaintTime = 0.0f;
	public GameObject PaintCanvas;
	GameObject ImagePrefab;

	int colorIndex = 1; //范围 1-5
	int brushIndex = 1; //范围 1-3
	float brushScale = 1;
	string brushSource;

	public List<GameObject> elements;

	// Start is called before the first frame update
	void Start()
	{
		gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
		Pen = PenRect.gameObject;
	}

	// Update is called once per frame
	void Update()
	{
		colorIndex = gameController.ColorIndex;
		if(gameController.HandCount == 0)
			Pen.SetActive(false);
		if (gameController.HandCount == 1)
		{
			if (gameController.thisFrame.Hands[0].IsRight)
				PaintPatterns();
			else Pen.SetActive(false);
		}

		if (gameController.HandCount == 2 && gameController.isOpenFullHand(gameController.rightHand) && gameController.isOpenFullHand(gameController.leftHand))
		{
			for (int i = 0; i < elements.Count; i++)
				Destroy(elements[i]);
			elements.Clear();
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
	}

	void PaintPatterns()
	{
		float PosX = 0, PosY = 0;
		PosX = gameController.leftHand.PalmPosition.x * 6400;
		PosY = gameController.leftHand.PalmPosition.y * 6400f - 200;
		Pen.GetComponent<RectTransform>().SetLocalX(PosX + 40);
		Pen.GetComponent<RectTransform>().SetLocalY(PosY + 40);
		Pen.SetActive(true);
		Vector3 PaintPos = new Vector3(PosX + 960, PosY + 540, 0);
		//print(PaintPos);
		if (gameController.isGrabHand(gameController.rightHand) && colorIndex != 0)
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
				elements[elements.Count - 1].GetComponent<RectTransform>().position = PaintPos;
				elements[elements.Count - 1].transform.SetParent(PaintCanvas.transform);
				elements[elements.Count - 1].GetComponent<RectTransform>().rotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360)));
				brushScale = Random.Range(0.8f, 2.0f);
				elements[elements.Count - 1].GetComponent<RectTransform>().localScale = new Vector3(brushScale, brushScale, brushScale);
				elements[elements.Count - 1].GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, Random.Range(0.1f, 0.4f));

			}
		}
	}

}