using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;

public class ChoooseMethod : MonoBehaviour
{
	public RectTransform MethodCollider01, MethodCollider02;
	public GameObject mask01, mask02;
	GameController gameController;
	public GameObject Mouse;
	public RectTransform MouseRect;
	int grabIndex = -1; //范围0-1

	// Start is called before the first frame update
	void Start()
	{
		mask01.SetActive(false);
		mask02.SetActive(false);
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
				if(grabIndex == 0)
				{
					gameController.screenState_H = ScreenState_H.ZA_RAN;
					gameController.screenState_V = ScreenState_V.ZA_RAN;
					grabIndex = -1;
				}
				if(grabIndex == 1)
				{
					gameController.screenState_H = ScreenState_H.LA_RAN;
					gameController.screenState_V = ScreenState_V.LA_RAN;
					grabIndex = -1;
				}
				Mouse.SetActive(true);
				//Mouse.GetComponent<UnityEngine.UI.Image>().color = new Color32(255, 255, 255, 255);
				Mouse.GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load("Hands/Hand01", typeof(Sprite)) as Sprite;
				if (IsRectTransformOverlap(MethodCollider01, MouseRect))
				{
					mask01.SetActive(true);
					mask02.SetActive(false);
				}
				else if (IsRectTransformOverlap(MethodCollider02, MouseRect))
				{
					mask02.SetActive(true);
					mask01.SetActive(false);
				}
				else
				{
					mask01.SetActive(false);
					mask02.SetActive(false);
				}
				grabIndex = -1;
			}
			else
			{
				Mouse.GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load("Hands/Hand02", typeof(Sprite)) as Sprite;
				if (grabIndex == -1)
				{
					if (IsRectTransformOverlap(MethodCollider01, MouseRect))
						grabIndex = 0;
					if (IsRectTransformOverlap(MethodCollider02, MouseRect))
						grabIndex = 1;
				}
			}
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