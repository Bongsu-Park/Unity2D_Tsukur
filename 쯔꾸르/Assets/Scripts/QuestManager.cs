using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour 
{
	public int questId;
	public int questActionIndex; //퀘스트 대화순서 변수 생성
	public GameObject[] questObject;//퀘스트 오브젝트를 저장할 변수 생성

	Dictionary<int, QuestData> questList;
	
	void Awake () 
	{
		questList = new Dictionary<int, QuestData>();
		GenerateData();
	}
	
	void GenerateData() 
	{
		questList.Add(10, new QuestData("마을 사람들과 대화하기", new int[] { 1000, 2000 }));
		//int[]에는 해당 퀘스트에 연관된 NPC Id를 입력
		questList.Add(20, new QuestData("루도의 동전 찾아주기", new int[] { 5000, 2000 }));

		questList.Add(30, new QuestData("퀘스트 올 클리어!", new int[] { 0 }));

	}

	public int GetQuestTalkIndex(int id) //npc id를 받고 퀘스트번호를 반환하는 함수 생성
	{
		return questId + questActionIndex; //퀘스트번호 + 퀘스트 대화순서 = 퀘스트 대화id
	}

	public string CheckQuest(int id)
	//대화 진행을 위해 퀘스트 대화순서를 올리는 함수 생성
	{
		//Next Talk Target
		if(id == questList[questId].npcId[questActionIndex])
			questActionIndex++;

		//Control Quest Object
		ControlObject();

		//Talk Complete & Next Quest
		if (questActionIndex == questList[questId].npcId.Length)
			//퀘스트 대화순서가 끝에 도달했을때 퀘스트번호 증가
			NextQuest();

		//Quest Name
		return questList[questId].questName;
	}

	public string CheckQuest()
	//대화 진행을 위해 퀘스트 대화순서를 올리는 함수 생성
	{
		//Quest Name
		return questList[questId].questName;
	}
	//함수이름은 같지만 매개변수에 따라 무슨 함수를 사용할지가 달라진다.
	//오버로딩(Overloading) : 매개변수에 따라 함수 호출

	void NextQuest()
	//다음 퀘스트를 위한 함수 생성
	{
		questId = questId + 10;
		questActionIndex = 0;
	}

	public void ControlObject()
	//퀘스트 오브젝트를 관리할 함수 생성
	{
		switch(questId)
		{
			case 10:
				if (questActionIndex == 2)
					questObject[0].SetActive(true);
				break;
			case 20:
				//불러오기 했을 당시의 퀘스트 순서와 연결된 오브젝트 관리 추가
				if (questActionIndex == 0)
					questObject[0].SetActive(true);
				else if (questActionIndex == 1)
					questObject[0].SetActive(false);
				break;
		}
	}
}
