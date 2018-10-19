using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBallScript : MonsterScript {

	// Use this for initialization
	void Start () {
		zMoveSpeed_ = -3.0f;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	/*
	public override void UpdateInterpolatePos(float frameMovedDist)
	{
		playerMoveInterpolatedPos_ += frameMovedDist;
		base.UpdateInterpolatePos(frameMovedDist);
		return;
	}
	*/
	public override float CalcYPos()
	{
		return CalcZPos() * 0.3f * 0.25f;
	}
	public override float CalcZPos()
	{
		return (Constant.Distance_ObjectAppear_          //interpolate world scale
					+ zMoveSpeed_ * (Time.time - enabledTime_)) * 4.0f; // monster move speed, character move
	}
}
