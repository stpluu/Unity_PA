using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Constant
{
	public enum ItemState
	{
		None = 0,
		Have = 1,
		Used = 2,
	};
	
}

public class ItemInventoryScript : MonoBehaviour {
	public static Constant.ItemState[] itemInventory_; //0 : none, 1: buy, 2: buy and used
	public static void SetItemState(Constant.ItemDef itemName, Constant.ItemState state)
	{
		itemInventory_[(int)itemName] = state;
	}
	public static Constant.ItemState GetItemState(Constant.ItemDef itemName)
	{
		return itemInventory_[(int)itemName];
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public static void ResetAllInventory()
	{
		for (int i = 0; i < (int)Constant.ItemDef.TOTALITEMCOUNT; ++i)
		{

		}
		
	}

	public static float GetItemAbility(Constant.ItemDef itemName)
	{
		switch(itemName)
		{
			case Constant.ItemDef.Ring:
				break;
			default:
				return 0.0f;
		}
		return 0.0f;
	}
}
