using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;
using UnityEngine.SceneManagement;

public enum ScreenState_H
{
	STARTING, ZA_RAN, LA_RAN, RESULT
};

public enum ScreenState_V
{
	DYE_METHOD, ZA_RAN, LA_RAN, QR_CODE
};

public class GameController : MonoBehaviour
{
	LeapProvider mProvider;
	public Frame thisFrame;
	public Hand leftHand, rightHand;
	Vector leftPosition;
	Vector rightPosition;
	public float deltaVelocity; //单方向上手掌移动的速度
	public int HandCount = 0;

	float waveTime = 0.2f;
	public bool isWaveRight = false;

	public int methodIndex = 0; //范围 0-4 默认为2
	public ScreenState_H screenState_H = ScreenState_H.STARTING;
	public ScreenState_V screenState_V = ScreenState_V.DYE_METHOD;
	public GameObject[] canvas_H = new GameObject[4];
	public GameObject[] canvas_V = new GameObject[4];
	public GameObject FishPool;
	public int ColorIndex = 0; //范围 0-4
	public int PatternIndex = 0; //范围0-4
	public bool isNewPatternPicked = false;
	public bool isNewColorPicked = false;

	// Start is called before the first frame update
	void Start()
	{
		mProvider = FindObjectOfType<LeapProvider>() as LeapProvider;
		deltaVelocity = 0.1f;
		screenState_V = ScreenState_V.DYE_METHOD;
		screenState_H = ScreenState_H.STARTING;
	}

	// Update is called once per frame
	void Update()
	{
		thisFrame = mProvider.CurrentFrame; //获取当前帧
											//获得手的个数
											//左右手的识别
		foreach (var item in thisFrame.Hands)
		{
			if (item.IsLeft)
			{
				leftHand = item;
				leftPosition = leftHand.PalmPosition;
				//print("leftHandPosition" + leftPosition);
			}
			else
			{
				rightHand = item;
				rightPosition = rightHand.PalmPosition;
				//print("rightHandPosition" + rightPosition);
			}
		}
		HandCount = thisFrame.Hands.Count;
		if (HandCount == 1 && thisFrame.Hands[0].IsLeft == true)
			isXPositive(leftHand);
		//WaveRight(leftHand);
		//if(isWaveRight && screenState_H == ScreenState_H.LA_RAN)
		//{
		//	screenState_H = ScreenState_H.RESULT;
		//	screenState_V = ScreenState_V.QR_CODE;
		//}
		switch (screenState_V)
		{
			case ScreenState_V.DYE_METHOD: screenState_H = ScreenState_H.STARTING; canvas_V[0].SetActive(true); canvas_V[1].SetActive(false); canvas_V[2].SetActive(false); canvas_V[3].SetActive(false); break;
			case ScreenState_V.ZA_RAN: canvas_V[0].SetActive(false); canvas_V[1].SetActive(true); canvas_V[2].SetActive(false); canvas_V[3].SetActive(false); break;
			case ScreenState_V.LA_RAN: canvas_V[0].SetActive(false); canvas_V[1].SetActive(false); canvas_V[2].SetActive(true); canvas_V[3].SetActive(false); break;
			case ScreenState_V.QR_CODE: canvas_V[0].SetActive(false); canvas_V[1].SetActive(false); canvas_V[2].SetActive(false); canvas_V[3].SetActive(true); break;
		}

		switch (screenState_H)
		{
			case ScreenState_H.STARTING: canvas_H[0].SetActive(true); canvas_H[1].SetActive(false); canvas_H[2].SetActive(false); canvas_H[3].SetActive(false); break;
			case ScreenState_H.ZA_RAN: canvas_H[0].SetActive(false); canvas_H[1].SetActive(true); canvas_H[2].SetActive(false); canvas_H[3].SetActive(false); break;
			case ScreenState_H.LA_RAN: canvas_H[0].SetActive(false); canvas_H[1].SetActive(false); canvas_H[2].SetActive(true); canvas_H[3].SetActive(false); break;
			case ScreenState_H.RESULT: canvas_H[0].SetActive(false); canvas_H[1].SetActive(false); canvas_H[2].SetActive(false); canvas_H[3].SetActive(true); break;
		}

		if (Input.GetKeyDown(KeyCode.Space))
		{
			methodIndex = 2;
			if (screenState_H != ScreenState_H.RESULT)
				screenState_H += 1;
			if (screenState_V != ScreenState_V.QR_CODE)
				screenState_V += 1;
			else SceneManager.LoadScene("Test");
		}
		if(Input.GetKeyDown(KeyCode.Q))
		{
			ColorIndex = (ColorIndex + 1) % 5;
		}

		if(screenState_H == ScreenState_H.RESULT)
			FishPool.SetActive(true);
		else FishPool.SetActive(false);
		if(screenState_H == ScreenState_H.ZA_RAN)
		{
			ColorIndex = -1;
			PatternIndex = -1;
		}
		if(screenState_H == ScreenState_H.LA_RAN)
		{
			if(HandCount == 2 && isOpenFullHand(leftHand) && isOpenFullHand(rightHand))
			{
				screenState_H = ScreenState_H.RESULT;
				screenState_V = ScreenState_V.QR_CODE;
			}
		}
	}

	//计算两手之间的距离
	public float GetDistance()
	{
		float distance = 0f;
		if (leftPosition != Vector.Zero && rightPosition != Vector.Zero)
		{
			Vector3 leftPos = new Vector3(leftPosition.x, leftPosition.y, leftPosition.z);
			Vector3 rightPos = new Vector3(rightPosition.x, rightPosition.y, rightPosition.z);
			distance = Vector3.Distance(leftPos, rightPos);
			//print("distance" + distance);
		}
		//if(distance >= 0.2f) Debug.Log(distance + "  两手距离大于等于20cm");
		if (distance != 0)
			return distance;
		else
			return distance = 0.5f;
	}

	public void WaveRight(Hand hand)
	{
		if (isGrabHand(hand) || !isMoveRight(hand) || !isXPositive(hand))
		{
			waveTime = 0.2f;
			isWaveRight = false;
		}
		else
		{
			waveTime -= Time.deltaTime;
			if(waveTime <= 0)
			{
				waveTime = 2.0f;
				isWaveRight = true;
				//print(1);
			}
		}

	}

	/*基本判定函数*/
	//手掌握紧
	public bool isGrabHand(Hand hand) => hand.GrabStrength > 0.6f;

	//手掌全展开
	public bool isOpenFullHand(Hand hand) => hand.GrabStrength < 0.2f;

	// 手划向右边
	public bool isMoveRight(Hand hand) => hand.PalmVelocity.x > deltaVelocity;

	//手划向左边
	public bool isMoveLeft(Hand hand) => hand.PalmVelocity.x < -deltaVelocity;

	//手向上
	public bool isMoveUp(Hand hand) => hand.PalmVelocity.z < -deltaVelocity;

	//手向下
	public bool isMoveDown(Hand hand) => hand.PalmVelocity.z > deltaVelocity;

	//手向前
	public bool isMoveForward(Hand hand) => hand.PalmVelocity.y > deltaVelocity;

	//手向后
	public bool isMoveBack(Hand hand) => hand.PalmVelocity.y < -deltaVelocity;

	//是否固定不动
	public bool isMoving(Hand hand)
	{
		if (isMoveBack(hand) || isMoveDown(hand) || isMoveForward(hand) || isMoveLeft(hand) || isMoveRight(hand) || isMoveUp(hand))
			return true;
		else return false;
	}

	//判断手心是否朝向X轴正方向
	public bool isXPositive(Hand hand)
	{
		Vector3 palmnormal = hand.PalmNormal.ToVector3();
		//float delta_angle = Vector3.Angle(palmnormal, face_self);
		if (palmnormal.x >= 0.5)
		{
			//print("手心朝X轴正方向");
			return true;
		}
		else
			return false;
	}

	//判断食指是否打开
	public bool isIndexOpenToHand(Hand hand)
	{
		foreach (var finger in hand.Fingers)
		{
			if ((finger.TipPosition - hand.PalmPosition).Magnitude <= 0.06f)
				if (finger.Type == Finger.FingerType.TYPE_INDEX)
					return false;
				else continue;
			else
			{
				if (finger.Type == Finger.FingerType.TYPE_THUMB)
					continue;
				if (finger.Type != Finger.FingerType.TYPE_INDEX)
					return false;
			}
		}
		return true;
	}
}
