using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTexture : MonoBehaviour
{
	Color32[] colors = { new Color32(0, 0, 0, 255), new Color32(245, 178, 124, 255), new Color32(134, 181, 236, 255), new Color32(171, 136, 177, 255), new Color32(193, 148, 140, 255) };
	GameController gameController;
	public Material material;
	public Texture[] textures = new Texture[4];
	int textureIndex = 0;

	// Start is called before the first frame update
	void Start()
	{
		gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.W))
			textureIndex = (textureIndex + 1) % 4;
		material.SetTexture("_MainTex", textures[textureIndex]);
	}
}
