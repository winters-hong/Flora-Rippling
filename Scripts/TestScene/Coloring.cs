using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;

public class Coloring : MonoBehaviour
{
	GameController gameController;
	public GameObject background;
	public UnityEngine.UI.Image colorImage;
	public UnityEngine.UI.Image colorMask;
	string imageFileName = "Patterns/p01";
	string maskFileName = "Patterns/p02";
	Color32[] colors = { new Color32(255, 255, 255, 255), new Color32(171, 136, 177, 255), new Color32(245, 178, 124, 255), new Color32(193, 148, 140, 255), new Color32(134, 181, 236, 255) };
	float[,] HSVColors = new float[5, 3];
	float[] temp = new float[3];
	public float colorDelta = 0.006f;
	float colorSpan;

	// Start is called before the first frame update
	void Start()
	{
		gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
		float h, s, v;
		for (int i = 0; i < colors.Length; i++)
		{
			Color.RGBToHSV(colors[i], out h, out s, out v);
			HSVColors[i, 0] = h;
			HSVColors[i, 1] = s;
			HSVColors[i, 2] = v;
		}

	}

	// Update is called once per frame
	void Update()
	{
		if (gameController.PatternIndex == 0)
			background.SetActive(true);
		else background.SetActive(false);
		if (gameController.isNewPatternPicked)
		{
			switch (gameController.PatternIndex)
			{
				case 0: imageFileName = "Patterns/p01"; maskFileName = "Patterns/p02"; break;
				case 1: imageFileName = "Patterns/p11"; maskFileName = "Patterns/p12"; break;
				case 2: imageFileName = "Patterns/p21"; maskFileName = "Patterns/p22"; break;
				case 3: imageFileName = "Patterns/p31"; maskFileName = "Patterns/p32"; break;
				case 4: imageFileName = "Patterns/p41"; maskFileName = "Patterns/p42"; break;
			}
			colorImage.sprite = Resources.Load(imageFileName, typeof(Sprite)) as Sprite;
			colorMask.sprite = Resources.Load(maskFileName, typeof(Sprite)) as Sprite;
			gameController.isNewPatternPicked = false;
			for (int i = 0; i < 3; i++)
				temp[i] = HSVColors[gameController.ColorIndex, i];
			colorSpan = temp[1];
			temp[1] = 0.05f;
			colorImage.color = Color.HSVToRGB(temp[0], temp[1], temp[2]);
		}
		if (gameController.isNewColorPicked)
		{
			gameController.isNewColorPicked = false;
			for (int i = 0; i < 3; i++)
				temp[i] = HSVColors[gameController.ColorIndex, i];
			colorSpan = temp[1];
			temp[1] = 0.05f;
			colorImage.color = Color.HSVToRGB(temp[0], temp[1], temp[2]);
		}
		else
		{
			if (gameController.HandCount == 1 && gameController.thisFrame.Hands[0].IsRight)
			{
				if (gameController.isMoving(gameController.rightHand) && gameController.isGrabHand(gameController.rightHand))
				{
					//颜色加深
					colorSpan -= colorDelta;
					temp[1] = HSVColors[gameController.ColorIndex, 1] - colorSpan;
					colorImage.color = Color.HSVToRGB(temp[0], temp[1], temp[2]);
				}
			}
			if (Input.GetKey(KeyCode.S))
			{
				colorSpan -= colorDelta;
				temp[1] = HSVColors[gameController.ColorIndex, 1] - colorSpan;
				colorImage.color = Color.HSVToRGB(temp[0], temp[1], temp[2]);
			}
		}
	}
}
