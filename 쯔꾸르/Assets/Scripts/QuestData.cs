using System.Collections;
using System.Collections.Generic;
//코드에 불러서 쓸거기때문에 monobehaviour, using unityengine이 필요없다
public class QuestData
{
	public string questName;
	public int[] npcId;

	//구조체 생성을 위해 매개변수 생성자를 작성
	public QuestData(string name, int[] npc)
	{
		questName = name;
		npcId = npc;
	}
	
}
