using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManagerScript : MonoBehaviour {

    private PlayerScript playerScript_;
    private WorldScript worldScript_;
	private StageLoader stageLoader_;
	private ShopScript shopScript_;

	public GameObject fish_;
	public GameObject heart_;
	public GameObject large_hole_;
	public GameObject fish_hole_;
	public GameObject crack_;
	public GameObject rock_;
	public GameObject shop_;
	public GameObject tree_;
	public GameObject warp_;
	public GameObject wing_;
	public GameObject goal_;

	// Item / monster / bullet pool
    public GameObject[] fishPool_;

	// MapObject Pool
	public GameObject[] largeHolePool_;
	public GameObject[] fishHolePool_;
	public GameObject[] crackPool_;
	public GameObject[] rockPool_;
	public GameObject[] shopPool_;

	private const int FISH_INSCREEN_MAX_NUM = 10;
	private const int LARGE_HOLE_INSCREEN_MAX_NUM = 7;
	private const int CRACK_INSCREEN_MAX_NUM = 7;
	private const int FISH_HOLE_INSCREEN_MAX_NUM = 7;
	private const int ROCK_INSCREEN_MAX_NUM = 7;
	private const int SHOP_INSCREEN_MAX_NUM = 3;


	[SerializeField] private int fishCount_;	//player 가 소지한 fish 수

    private Text TimeText_;
    private Text FishText_;
    private Text DistanceText_;
    private Text SpeedText_;

	private Text DebugText_;
    float stageStartTime_;
	float pauseTime_;
	float stageFinishTime_;
	bool bInGoal_;
	bool bInShop_;
	bool bPause_;
	bool bDie_;

	public static int currentStage_;
	public static void setCurrentStage(int stage)
	{
		currentStage_ = stage;
	}
	public static int getCurrentStage()
	{
		return currentStage_;
	}
	public int[] itemInventory_;	//0 : none, 1: buy, 2: buy and used
	
    private void Awake()
    {
        playerScript_ = GameObject.Find("Player").GetComponent<PlayerScript>();
        worldScript_ = gameObject.GetComponent<WorldScript>();
		stageLoader_ = gameObject.GetComponent<StageLoader>();
		shopScript_ = GameObject.Find("ShopUI").GetComponent<ShopScript>();
          
        TimeText_ = GameObject.Find("UI").transform.Find("Time").GetComponent<Text>();
        FishText_ = GameObject.Find("UI").transform.Find("Fish").GetComponent<Text>();
        DistanceText_ = GameObject.Find("UI").transform.Find("Distance").GetComponent<Text>();
        SpeedText_ = GameObject.Find("UI").transform.Find("Speed").GetComponent<Text>();

        // create object instanace
        fishPool_ = new GameObject[FISH_INSCREEN_MAX_NUM];
		largeHolePool_ = new GameObject[LARGE_HOLE_INSCREEN_MAX_NUM];
		fishHolePool_ = new GameObject[FISH_HOLE_INSCREEN_MAX_NUM];
		crackPool_ = new GameObject[CRACK_INSCREEN_MAX_NUM];
		rockPool_ = new GameObject[ROCK_INSCREEN_MAX_NUM];
		shopPool_ = new GameObject[SHOP_INSCREEN_MAX_NUM];

		DebugText_ = GameObject.Find("UI").transform.Find("Debug").GetComponent<Text>();

		itemInventory_ = new int[(int)Constant.ItemDef.TOTALITEMCOUNT];
		for (int i = 0; i < (int)Constant.ItemDef.TOTALITEMCOUNT; ++i)
		{
			itemInventory_[i] = 0;
		}
		//currentStage_ = 1;
	}
    // Use this for initialization
    void Start () {
        stageStartTime_ = Time.time;
        fishCount_ = 20;
		//instantiate
        for (int i = 0; i < FISH_INSCREEN_MAX_NUM; ++i)
        {
            fishPool_[i] = Instantiate(fish_, Vector3.zero, Quaternion.identity) as GameObject;
            fishPool_[i].SetActive(false);
        }

		for (int i = 0; i < LARGE_HOLE_INSCREEN_MAX_NUM; ++i)
		{
			largeHolePool_[i] = Instantiate(large_hole_, Vector3.zero, Quaternion.identity) as GameObject;
			largeHolePool_[i].SetActive(false);
		}

		for (int i = 0; i < FISH_HOLE_INSCREEN_MAX_NUM; ++i)
		{
			fishHolePool_[i] = Instantiate(fish_hole_, Vector3.zero, Quaternion.identity) as GameObject;
			fishHolePool_[i].SetActive(false);
		}

		for (int i = 0; i < CRACK_INSCREEN_MAX_NUM; ++i)
		{
			crackPool_[i] = Instantiate(crack_, Vector3.zero, Quaternion.identity) as GameObject;
			crackPool_[i].SetActive(false);
		}

		for (int i = 0; i < ROCK_INSCREEN_MAX_NUM; ++i)
		{	
			rockPool_[i] = Instantiate(rock_, Vector3.zero, Quaternion.identity) as GameObject;
			rockPool_[i].SetActive(false);
		}

		for (int i = 0; i < SHOP_INSCREEN_MAX_NUM; ++i)
		{
			shopPool_[i] = Instantiate(shop_, Vector3.zero, Quaternion.identity) as GameObject;
			shopPool_[i].SetActive(false);
		}

		bInGoal_ = false;

		if (getCurrentStage() < 1
			|| getCurrentStage() > 2)
			setCurrentStage(1);
		//currentStage_ = 1;
		//worldScript_.loadStage(currentStage_);
		stageLoader_.LoadStage(StageLoader.StageStyle.orignal, currentStage_);
		shopScript_.SetShopStage(currentStage_);
		shopScript_.SetShopUIVisible(false);
		pauseTime_ = 0.0f;
		bInShop_ = false;
		bPause_ = false;
		bDie_ = false;
	}
	
	// Update is called once per frame
	void Update () {
        //float speed = playerScript_.speed_;
        worldScript_.updateDistance(playerScript_.speed_);
		if (bInGoal_)
			UpdateUIInGoal();
        else
			UpdateUI();
		if (isTimePauseState())
			pauseTime_ += Time.deltaTime;
	}
   
    void UpdateUI()
    {
        // speed Text
        //string speedText = "";
        //for (int i = 0; i < playerScript_.speed_; ++ i)
        //{
          //  speedText = speedText + "|";
       // }
	   if (playerScript_.speed_ > Constant.Speed_Max_Lv1)
		{
			SpeedText_.color = new Color(255, 50, 50);
		}
	   else
		{
			SpeedText_.color = new Color(255, 255, 255);
		}
        SpeedText_.text = playerScript_.speed_.ToString();

        //time text
        int remainTime = worldScript_.stageMaxTime_ * 10 - (int)((Time.time - stageStartTime_) * 10) + (int)pauseTime_;
        TimeText_.text = remainTime.ToString();

        //distanceText
        int distance = worldScript_.stageMaxDistance_ - (int)worldScript_.distance_;
        DistanceText_.text = distance.ToString();

        //fish text
        FishText_.text = fishCount_.ToString();

		//DebugText
		DebugText_.text = GameObject.Find("Player").GetComponent<PlayerScript>().characterState_.ToString();
		DebugText_.enabled = false;
	}
	void UpdateUIInGoal()
	{

	}
    public void OnCollideFish(GameObject fishObj)
    {
        fishCount_++;
		Debug.Log("Fish Collide : " + fishCount_.ToString());
		fishObj.SetActive(false);
		fishObj.transform.position = Vector3.zero;
    }
	public void OnEnterShop(GameObject shopObj, Constant.MapObjects shopType)
	{
		shopScript_.SetShopType(shopType);
		//GameObject.Find("ShopUI").SetActive(true);
		shopScript_.SetShopUIVisible(true);
		playerScript_.SetSpeed(0);
	}
	public void OnExitShop()
	{
		shopScript_.SetShopUIVisible(false);
		playerScript_.OnExitFromHole();

	}
    public GameObject GetMapObjectInstance(Constant.MapObjects objectType)
    {
		switch (objectType)
		{
			case Constant.MapObjects.CRACK:
				foreach (GameObject obj in crackPool_)
				{
					if (obj.activeSelf == false)
					{
						return obj;
					}
				}
				break;
			case Constant.MapObjects.FISH_HOLE:
				foreach (GameObject obj in fishHolePool_)
				{
					if (obj.activeSelf == false)
					{
						return obj;
					}
				}
				break;
			case Constant.MapObjects.LHOLE:
				foreach (GameObject obj in largeHolePool_)
				{
					if (obj.activeSelf == false)
					{
						return obj;
					}
				}
				break;
			case Constant.MapObjects.ROCK:
				foreach (GameObject obj in rockPool_)
				{
					if (obj.activeSelf == false)
					{
						return obj;
					}
				}
				break;
			case Constant.MapObjects.SHOP_EXPENSIVE:
			case Constant.MapObjects.SHOP_NORMAL:
			case Constant.MapObjects.SHOP_SANTA:
				foreach (GameObject obj in shopPool_)
				{
					if (obj.activeSelf == false)
					{
						return obj;
					}
				}
				break;
			default:
				break;
		}
        return null;
    }

	public GameObject GetFishInstance()
	{
		foreach (GameObject obj in fishPool_)
		{
			if (obj.activeSelf == false)
			{
				//Debug.Log("Free Fish : " + i.ToString());
				return obj;
			}
		}
		return null;
	}

	public void OnGoal()
	{
		stageFinishTime_ = Time.time;
		//GameObject.FindWithTag("Player").GetComponent<Animator>().SetTrigger("trGoal");
		playerScript_.OnGoal();
		//playerScript_.SetSpeed(0);
		bInGoal_ = true;
	}

	public void OnPauseButton()
	{
		GameObject menu = GameObject.Find("UI").transform.Find("PauseMenu").gameObject;
		if (menu.activeSelf == false)
		{
			Time.timeScale = 0;
			menu.SetActive(true);
		}
		else
		{
			OnReturnButton();
		}
		
	}

	public void OnReturnButton()
	{
		GameObject menu = GameObject.Find("UI").transform.Find("PauseMenu").gameObject;
		Time.timeScale = 1.0f;
		menu.SetActive(false);
	}

	public void OnRestratButton()
	{
		Time.timeScale = 1.0f;
		SceneManager.LoadScene("PA_MainGame");
	}
	public void OnExitButton()
	{
		Time.timeScale = 1.0f;
		SceneManager.LoadScene("TitleScreen");
	}
	public void OnFishUpButtonGM()
	{
		fishCount_ += 10;
	}
	public bool BuyItem(Constant.ItemDef item, int price)
	{
		if (fishCount_ >= price
			&& itemInventory_[(int)item] == 0)
		{
			fishCount_ -= price;
			itemInventory_[(int)item] = 1;
			string inventoryName = "Inventory_" + ((int)item).ToString();
			GameObject itemIcon = GameObject.Find("UI").transform.Find(inventoryName).gameObject;
			itemIcon.SetActive(true);
			return true;
		}
		return false;
	}

	public bool isBoughtItem(Constant.ItemDef item)
	{
		if (itemInventory_[(int)item] != 0)
			return true;
		return false;
	}

	public bool hasItem(Constant.ItemDef item)
	{
		if (itemInventory_[(int)item] == 1)
			return true;
		return false;
	}

	public bool isTimePauseState()
	{
		if (bInGoal_)
			return true;
		if (bInShop_)
			return true;
		if (bDie_)
			return true;
		return false;
	}
	public void onDie(int dieReason)
	{
		bDie_ = true;
	}
	public void loadStage(int level)
	{
		setCurrentStage(level);
		SceneManager.LoadScene("PA_MainGame");
		Time.timeScale = 1.0f;
	}
	public void goNextStage()
	{
		if (getCurrentStage() >= 2)
		{
			SceneManager.LoadScene("Title_Screen");
		}
		else
		{
			loadStage(getCurrentStage() + 1);
			
		}
		
	}
}
