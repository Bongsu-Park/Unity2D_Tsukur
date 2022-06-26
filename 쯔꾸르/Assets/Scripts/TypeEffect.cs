using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class TypeEffect : MonoBehaviour 
{
	string targetMsg;  //표시할 대화 문자열을 따로 변수로 저장
	public int CharPerSeconds; //글자 재생 속도를 위한 변수 생성
    Text msgText;
	int index;
	float interval;
	public bool isAnimation;

	public GameObject EndCursor;

	AudioSource audioSource;
	
	private void Awake()
    {
		msgText = GetComponent<Text>();
		audioSource = GetComponent<AudioSource>();
    }

	public void SetMsg(string msg) 
	{
		if (isAnimation)
		{//Interrupt
			msgText.text = targetMsg;
			CancelInvoke();
			EffectEnd();
		}
		else
		{
			targetMsg = msg;
			EffectStart();
		}
	}
	
	void EffectStart () //시작
	{
		msgText.text = "";
		index = 0;
		EndCursor.SetActive(false);

		//Start Animation
		interval = 1.0f / CharPerSeconds; 
		//이렇게 따로 계산해줘야 제대로 계산하고 실행을 한다.
		Debug.Log(interval);

		isAnimation = true;

		Invoke("Effecting", interval);
	}

	void Effecting()  //하는중
    {
		//End Animation
		if(msgText.text == targetMsg)
        {
			EffectEnd();
			return;
        }
		msgText.text = msgText.text + targetMsg[index]; //문자열도 배열처럼 char값에 접근가능
		
		//Sound
		if(targetMsg[index] != ' ' || targetMsg[index] != '.')
			audioSource.Play();

		index++;
		Invoke("Effecting", interval);
	}

	void EffectEnd()  //끝
	{
		isAnimation = false;
		EndCursor.SetActive(true);
	}
}
