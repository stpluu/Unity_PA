using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public partial class Constant
{
	public enum MapOpenType
	{
		Closed = 0,
		NextStage = 1,
		Pause = 2,
		Etc = 3,
	}
}
public class MapScript : MonoBehaviour {
	private GameObject mapObject_;
	struct stageMapPos
	{
		int x;
		int y;
	}
	private Dictionary<int, stageMapPos> mapPos;
	private void Awake()
	{
		mapObject_ = GameObject.Find("Map");
		//mapObject_.transform.FindChild("MapImage").GetComponent<Image>().overrideSprite
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	private void OnEnable()
	{
		
	}
	private void OnDisable()
	{
		
	}
	public void ChangeMapSprite(StageLoader.GameMode mode)
	{
		switch(mode)
		{
			case StageLoader.GameMode.orignal:
			case StageLoader.GameMode.hard:
				{
					mapObject_.transform.Find("MapImage").GetComponent<Image>().overrideSprite
						= Resources.Load<Sprite>("Sprites/MapImages/Map_Original");
				}
				break;
			case StageLoader.GameMode.custom:
			default:
				{
					mapObject_.transform.Find("MapImage").GetComponent<Image>().overrideSprite
						= Resources.Load<Sprite>("Sprites/MapImages/Map_Original");
				}
				break;
		}
		
	}

	private Constant.MapOpenType openType_;

	private Time openTime_; 
}
