using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;

public class LaRan : MonoBehaviour
{
	GameController gameController;
	public GameObject Mouse;
	public RectTransform BasketRect;
	public UnityEngine.UI.Image pickedPattern;
	public UnityEngine.UI.Image pickedColor;
	public GameObject pickImage;
	public RectTransform[] Rectangles = new RectTransform[8];
	public RectTransform MouseRect;

	int grabIndex = -1; //范围 0-8
	public int colorIndex = 0; //范围0-4
	public int patternIndex = 0; //范围0-4

	// Start is called before the first frame update
	void Start()
	{
		gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.A))
		{
			patternIndex = (patternIndex + 1) % 5;
			grabIndex = patternIndex;
			gameController.isNewPatternPicked = true;
			gameController.PatternIndex = patternIndex;
		}
		if (Input.GetKeyDown(KeyCode.D))
		{
			colorIndex = (colorIndex + 1) % 5;
			if (colorIndex == 0)
				grabIndex = 0;
			else
				grabIndex = colorIndex + 4;
			gameController.ColorIndex = colorIndex;
			gameController.isNewColorPicked = true;
		}
		if (gameController.HandCount == 1 && gameController.thisFrame.Hands[0].IsLeft)
		{
			float PosX = 0, PosY = 0;
			PosX = gameController.leftHand.PalmPosition.x * 6400;
			PosY = gameController.leftHand.PalmPosition.y * 6400f - 200;
			Mouse.GetComponent<RectTransform>().SetLocalX(PosX);
			Mouse.GetComponent<RectTransform>().SetLocalY(PosY);
			if (!gameController.isGrabHand(gameController.leftHand))
			{
				Mouse.SetActive(true);
				//Mouse.GetComponent<UnityEngine.UI.Image>().color = new Color32(255, 255, 255, 255);
				Mouse.GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load("Hands/Hand01", typeof(Sprite)) as Sprite;
				pickImage.SetActive(false);
				for (int i = 0; i < 8; i++)
					Rectangles[i].localScale = Vector3.one;
				for (int i = 0; i < 8; i++)
				{
					if (IsRectTransformOverlap(Rectangles[i], MouseRect))
					{
						Rectangles[i].localScale = new Vector3(1.2f, 1.2f, 1.2f);
						break;
					}
				}

				if (grabIndex != -1 && grabIndex < 4 && IsRectTransformOverlap(MouseRect, BasketRect))
				{
					patternIndex = grabIndex + 1;
					gameController.isNewPatternPicked = true;
					gameController.PatternIndex = patternIndex;
					//print("选择了一个纹理 " + patternIndex);
				}
				if (grabIndex >= 4 && IsRectTransformOverlap(MouseRect, BasketRect))
				{
					colorIndex = grabIndex - 3;
					gameController.ColorIndex = colorIndex;
					gameController.isNewColorPicked = true;
					//print("选择了一个颜色 " + colorIndex);
				}
				grabIndex = -1;
			}
			else
			{
				Mouse.GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load("Hands/Hand02", typeof(Sprite)) as Sprite;
				if (grabIndex == -1)
					for (int i = 0; i < 8; i++)
					{
						if (IsRectTransformOverlap(MouseRect, Rectangles[i]))
						{
							Rectangles[i].localScale = new Vector3(1.2f, 1.2f, 1.2f);
							grabIndex = i;
							string fileName = "LaRan/0" + (i + 1).ToString();
							pickImage.GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load(fileName, typeof(Sprite)) as Sprite;
							pickImage.SetActive(true);
							break;
						}
					}
				else
				{
					pickImage.SetActive(true);
				}
			}
		}
		else
		{
			Mouse.SetActive(false);
		}
		if (colorIndex == 0)
			pickedColor.sprite = Resources.Load("LaRan/00", typeof(Sprite)) as Sprite;
		else
			pickedColor.sprite = Resources.Load("LaRan/0" + (colorIndex + 4).ToString(), typeof(Sprite)) as Sprite;
		pickedPattern.sprite = Resources.Load("LaRan/0" + patternIndex.ToString(), typeof(Sprite)) as Sprite;
	}

	public bool IsRectTransformOverlap(RectTransform rect1, RectTransform rect2)
	{
		float rect1MinX = rect1.position.x - rect1.rect.width / 2;
		float rect1MaxX = rect1.position.x + rect1.rect.width / 2;
		float rect1MinY = rect1.position.y - rect1.rect.height / 2;
		float rect1MaxY = rect1.position.y + rect1.rect.height / 2;
		float rect2MinX = rect2.position.x - rect2.rect.width / 2;
		float rect2MaxX = rect2.position.x + rect2.rect.width / 2;
		float rect2MinY = rect2.position.y - rect2.rect.height / 2;
		float rect2MaxY = rect2.position.y + rect2.rect.height / 2;
		return rect1MinX < rect2MaxX && rect2MinX < rect1MaxX && rect1MinY < rect2MaxY && rect2MinY < rect1MaxY;
	}
}
