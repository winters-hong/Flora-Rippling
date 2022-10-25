using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;

public class ZARAN : MonoBehaviour
{
	public RectTransform cloth01, cloth02;
	public RectTransform clothCollider01, clothCollider02;
	GameController gameController;
	public GameObject Mouse;
	public RectTransform MouseRect;

	int grabIndex = 0;
	// Start is called before the first frame update
	void Start()
	{
		gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
	}

	// Update is called once per frame
	void Update()
	{
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
				grabIndex = -1;
			}
			else
			{
				Mouse.GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load("Hands/Hand02", typeof(Sprite)) as Sprite;
				if (grabIndex == -1)
				{
					if (IsRectTransformOverlap(clothCollider01, MouseRect))
					{
						grabIndex = 0;
						//界面跳转
					}
					if (IsRectTransformOverlap(clothCollider02, MouseRect))
					{
						grabIndex = 1;
						//界面跳转
					}
				}
			}
		}
		else
		{
			Mouse.SetActive(false);
		}
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
