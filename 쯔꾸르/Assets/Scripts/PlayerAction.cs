using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour 
{
	public GameManager manager;

	float h;
	float v;
	public float Speed;

	bool isHorizonMove;

	Animator anim;

	Vector3 dirVec; //현재 바라보고 있는 방향 값을 가진 변수

	GameObject scanObject;



	Rigidbody2D rigid;

	void Awake () 
	{
		rigid = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
	}


	void Update()
	{
		//Move Value
		h = manager.isAction ? 0 : Input.GetAxisRaw("Horizontal");
		v = manager.isAction ? 0 : Input.GetAxisRaw("Vertical");
		//manager.isAction ? 0 :을 통해서 대화중일때는 움직임을 제한시킨다.

		//Check Button Down & Up
		//수평, 수직 이동 버튼이벤트를 변수로 저장
		bool hDown = manager.isAction ? false : Input.GetButtonDown("Horizontal");
		bool vDown = manager.isAction ? false : Input.GetButtonDown("Vertical");
		bool hUp = manager.isAction ? false : Input.GetButtonUp("Horizontal");
		bool vUp = manager.isAction ? false : Input.GetButtonUp("Vertical");

		//Check Horizontal Move
		//버튼다운으로 수평이동 체크 & 버튼업으로도 수평이동 체크
		if (hDown) //수평이동          //원래는 hdown || vup이였다.  오류발생
			isHorizonMove = true; 
		else if (vDown) //수직이동    //원래는 vdown || hup이였다.  오류발생
			isHorizonMove = false;
		else if (hUp || vUp)
			//양쪽버튼누른상태로 하나만 버튼 업이면 발생하는 문제 해결방법
			isHorizonMove = h != 0;

		//Animation
		//방향전환이 바로 되도록 Any State사용
		//수직, 수평값을 받을 매개변수 hAxisRaw, vAxisRaw 생성
		if (anim.GetInteger("hAxisRaw") != h)
		//버튼을 꾹 누를시 walk애니메이션이 실행안되는걸 해결하는방법
		//transition을 연속적으로 태우면 애니메이션이 작동하지않는다.
		//해결할려면 transition을 딱 한번만줘야한다.
		{
			anim.SetBool("isChange", true); 
			//parameters에 값을 넣어주면 다른값을 넣기전까지는 변하지않는다.
			//그러므로 방향 변화 매개변수를 추가하여 한번만 실행되도록 변경한다.
			anim.SetInteger("hAxisRaw", (int)h);
		}
		else if (anim.GetInteger("vAxisRaw") != v)
		{
			anim.SetBool("isChange", true);
			anim.SetInteger("vAxisRaw", (int)v);
		}
		else
			anim.SetBool("isChange", false);

		//Direction
		if (vDown && v == 1)
			dirVec = Vector3.up;
		else if (vDown && v == -1)
			dirVec = Vector3.down;
		else if (hDown && h == 1)
			dirVec = Vector3.right;
		else if (hDown && h == -1)
			dirVec = Vector3.left;

		//Scan Object
		if (Input.GetButtonDown("Jump") && scanObject != null)
		{
			//Debug.Log("this is :" + scanObject.name);
			manager.Action(scanObject);
		}
	}

	void FixedUpdate()
	{
		//Move
		//플래그 변수 하나로 수평, 수직이동을 결정
		Vector2 moveVec = isHorizonMove ? new Vector2(h, 0) : new Vector2(0, v);
		rigid.velocity = moveVec * Speed;

		//Ray
		Debug.DrawRay(rigid.position, dirVec * 0.7f, new Color(0, 1, 0));
		RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, dirVec, 0.7f, 
			                                    LayerMask.GetMask("Object"));
		if (rayHit.collider != null)
			scanObject = rayHit.collider.gameObject; //RayCast된 오브젝트를 변수로 저장하여 활용
		else
			scanObject = null;

	}
}
