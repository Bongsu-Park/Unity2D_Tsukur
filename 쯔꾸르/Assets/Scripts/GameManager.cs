using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public Animator talkPanel;
	//public Text talkText;
	public TypeEffect talk;
	public GameObject scanObject;
	public bool isAction; //상태 저장용 변수

	public Image portraitImg;
	public Animator portraitAnimator;
	public Sprite prevPortrait;

	public TalkManager talkManager;
	public int talkIndex;

	public QuestManager questManager;

	public GameObject menuSet;

	public Text questTalk;

	public GameObject player;

	void Start()
	{
		GameLoad();
		questTalk.text = questManager.CheckQuest();
	}

	void Update()
	{
		//Sub Menu
		if (Input.GetButtonDown("Cancel"))
			if(menuSet.activeSelf)
				menuSet.SetActive(false);
			else
				menuSet.SetActive(true);
		
	}

	public void Action(GameObject scanObj)
    {
		scanObject = scanObj;
		//talkText.text = "이것의 이름은 " + scanObject.name + "이라고 한다.";
		ObjData objData = scanObject.GetComponent<ObjData>();
		Talk(objData.id, objData.isNpc);
	
		//Visible Talk for Action
		talkPanel.SetBool("isShow", isAction);
		
	}
	//대화 매니저를 변수로 선언 후, 함수 사용
	void Talk(int id, bool isNpc)
	{
		//Set Talk Data
		int questTalkIndex = 0;
		string talkData = "";
		if (talk.isAnimation)
		{
			talk.SetMsg("");
			return;
		}
		else
		{
			questTalkIndex = questManager.GetQuestTalkIndex(id);
			talkData = talkManager.GetTalk(id + questTalkIndex, talkIndex);
			//퀘스트번호 + npc id = 퀘스트 대화 데이터 id
		}

		//End Talk
		if (talkData == null)
		{
			isAction = false;
			talkIndex = 0;
			questTalk.text = questManager.CheckQuest(id);
			return;   //void함수에서 return은 강제 종료 역할을한다.
		}

		//Continue Talk
		if(isNpc)
		{
			talk.SetMsg(talkData.Split(':')[0]); 
			//Split():구분자를 통하여 배열로 나눠주는 문자열 함수

			//Show Portrait
			portraitImg.sprite = talkManager.GetPortrait(id, int.Parse(talkData.Split(':')[1]));
			//Parse() : 문자열을 해당 타입으로 변환해주는 함수
			//단, 문자열 내용이 타입과 맞지 않으면 오류 발생(ex 숫자만들어있는문자열은 int형 parse가 가능)
			portraitImg.color = new Color(1, 1, 1, 1);

			//Animation Portrait
			if (prevPortrait != portraitImg.sprite)
			{
				portraitAnimator.SetTrigger("doEffect");
				prevPortrait = portraitImg.sprite;
			}
		}
		else
		{
			talk.SetMsg(talkData);

			portraitImg.color = new Color(1, 1, 1, 0);
		}

		isAction = true;
		talkIndex++;
	}

	public void GameSave()
	{
		//PlayerPrefs : 간단한 데이터 저장 기능을 지원하는 클래스
		PlayerPrefs.SetFloat("PlayerX", player.transform.position.x);
		PlayerPrefs.SetFloat("PlayerY", player.transform.position.y);
		PlayerPrefs.SetInt("QuestId", questManager.questId);
		PlayerPrefs.SetInt("QuestActionIndex", questManager.questActionIndex);

		PlayerPrefs.Save();

		menuSet.SetActive(false);
	}

	public void GameLoad()
	{
		//최초 게임 실행했을 땐 데이터가 없으므로 예외처리 로직 작성
		if (!PlayerPrefs.HasKey("PlayerX"))
			return;

		float x = PlayerPrefs.GetFloat("PlayerX");
		float y = PlayerPrefs.GetFloat("PlayerY");
		int questId = PlayerPrefs.GetInt("QuestId");
		int questActionIndex = PlayerPrefs.GetInt("QuestActionInex");

		player.transform.position = new Vector3(x, y, 0);
		questManager.questId = questId;
		questManager.questActionIndex = questActionIndex;
		questManager.ControlObject();

	}

	public void GameExit()
	{
		Application.Quit(); //Application은 최상위 클래스이다.
	}

}
