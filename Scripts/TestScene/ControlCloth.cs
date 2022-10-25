using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlCloth : MonoBehaviour
{
	GameController gameController;
	[Range(0, 20)]
	public float RotateVelosity = 3;
	float waitTime = 1.0f;
	float direction = 1;

	// Start is called before the first frame update
	void Start()
	{
		gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
	}

	// Update is called once per frame
	void Update()
	{
		if(gameController.HandCount != 1 || gameController.thisFrame.Hands[0].IsLeft || !gameController.isOpenFullHand(gameController.rightHand))
		{
			transform.localPosition = Vector3.zero;
			return;
		}
		if(!gameController.isMoving(gameController.rightHand))
		{
			float r = Random.Range(0.5f, 1.0f);
			waitTime -= Time.deltaTime;
			if(waitTime <= 0)
			{
				waitTime = 1.0f;
				direction *= -1;
			}
			transform.Translate(new Vector3( 0.1f * direction * r, 0.2f * direction * r, 0.1f * direction * r), Space.World);
		}
		else transform.localPosition = Vector3.zero;
		if(gameController.isMoveRight(gameController.rightHand))
		{
			transform.Rotate(new Vector3(0, RotateVelosity, 0), Space.Self);
		}
		if(gameController.isMoveLeft(gameController.rightHand))
		{
			transform.Rotate(new Vector3(0, -RotateVelosity, 0), Space.Self);
		}
		if(gameController.isMoveForward(gameController.rightHand))
		{
			transform.Rotate(new Vector3(RotateVelosity, 0, 0), Space.Self);
		}
		if(gameController.isMoveBack(gameController.rightHand))
		{
			transform.Rotate(new Vector3(-RotateVelosity, 0, 0), Space.Self);
		}
		if(gameController.isMoveUp(gameController.rightHand))
		{
			transform.Rotate(new Vector3(0, RotateVelosity, 0), Space.Self);
		}
		if(gameController.isMoveDown(gameController.rightHand))
		{
			transform.Rotate(new Vector3(0, -RotateVelosity, 0), Space.Self);
		}
	}
}
