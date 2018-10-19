using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterScript : MonoBehaviour {

	public float zMoveSpeed_;
	public float enabledTime_;
	public float playerMoveInterpolatedPos_;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//UpdatePosition();
	}
	public virtual void UpdateInterpolatePos(float frameMovedDist)
	{
		//playerMoveInterpolatedPos_ = 0.0f;
		//Debug.Log("Monster - Update InterpolatePos :" + playerMoveInterpolatedPos_.ToString());
	}
	public virtual float CalcYPos()
	{
		return 0.0f;
	}
	public virtual float CalcZPos()
	{
		return 0.0f;
	}
	
	private void OnEnable()
	{
		Debug.Log("Monster - Enabled");
		GetComponent<BoxCollider>().enabled = true;
		playerMoveInterpolatedPos_ = 0.0f;
		enabledTime_ = Time.time;
	}
	private void OnDisable()
	{
		GetComponent<BoxCollider>().enabled = false;
		enabledTime_ = 0.0f;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			Debug.Log("Monster : Player col ");
			GameObject.Find("Player").GetComponent<PlayerScript>().OnCollideRock(gameObject);
			GetComponent<BoxCollider>().enabled = false;
		}

	}

	public void OnCollideBullet(Collider other)
	{
		// DIE MOTION
		if (gameObject.GetComponent<Animator>())
		{
			gameObject.GetComponent<Animator>().SetTrigger("trDamaged");
		}
	}
}
