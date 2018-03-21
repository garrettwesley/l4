using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Door_Master : MonoBehaviour {


 
//Background ring will - rotate, change colors, and disappear upon door open.

//	public Door_Master[] doorID;
	public Animator[] anim_ring;  //Ring =0, Lock = 1
	public Animator[] anim_lock;
	public Animator anim_door;

	public Sprite[] sprite;  // Locked = 0, ulocked = 1
	public Image[] image_ring;
	public Image[] image_lock;
	public bool doorUnloacked;
	public AudioSource doorsound;
	public bool triggerOverlap =  false;
	private bool doorclosed = true;

	private int change2Green = Animator.StringToHash("change2Green");
	private int change2Red = Animator.StringToHash("change2Red");
	private int ring_disappear = Animator.StringToHash("ring_disappear");
	private int lock_disappear = Animator.StringToHash("lock_disappear");
	private int Open_door = Animator.StringToHash("Open_door");
	private int ring_appear = Animator.StringToHash("ring_appear");
	private int lock_appear = Animator.StringToHash("lock_appear");
	private int animBool = Animator.StringToHash("animBool");

	void Start () 
	{

	}

	//------------------------------------------------------------------------//

	void Update()

	{
		if ( Input.GetKeyDown(KeyCode.K))
		{
//			doorID[0].image_lock[0].color = Color.green;
//			doorID[0].image_lock[1].color = Color.green;
//			doorID[0].anim_ring [0].SetBool ("change2Green", true);
//			doorID[0].anim_ring [1].SetBool ("change2Green", true);//Set color of ring via animation script
//			doorID[0].image_lock[0].sprite = sprite [1];
//			doorID[0].image_lock[1].sprite = sprite [1];
//			doorID[0].doorUnloacked [doorNumber] = true;
		}
			
		if  (triggerOverlap == true && doorUnloacked == true && doorclosed == true)
			{
			StartCoroutine (OpenDoor());


			}

		if (triggerOverlap == false && doorclosed == false) 
			{
			StartCoroutine(CloseDoor());
			}
	}

	//------------------------------------------------------------------------//


	public void unlockDoor()
	{
		
			/*for (int i = 0; i <= anim_ring.Length; i++) {*/

//			doorID[doorNumber].image_lock[0].color = Color.green;
//			doorID[doorNumber].image_lock[1].color = Color.green;
//			doorID[doorNumber].anim_ring [0].SetBool ("change2Green", true);
//			doorID[doorNumber].anim_ring [1].SetBool ("change2Green", true);//Set color of ring via animation script
//			doorID[doorNumber].image_lock[0].sprite = sprite [1];
//			doorID[doorNumber].image_lock[1].sprite = sprite [1];
//			doorID[doorNumber].doorUnloacked [doorNumber] = true;

		image_lock[0].color = Color.green;
		image_lock[1].color = Color.green;
		anim_ring [0].SetBool (change2Green, true);
		anim_ring [1].SetBool (change2Green, true);//Set color of ring via animation script
		image_lock[0].sprite = sprite [1];
		image_lock[1].sprite = sprite [1];
		doorUnloacked  = true;

	}

	public void lockDoor()
	{
		image_lock[0].color = Color.red;
		image_lock[1].color = Color.red;
		anim_ring [0].SetBool (change2Red,true);
		anim_ring [1].SetBool (change2Red, true);//Set color of ring via animation script
		image_lock[0].sprite = sprite [0];
		image_lock[1].sprite = sprite [0];
		doorUnloacked  = false;

	}


	//-------------Lock - Ring - Door Animations -----------------------------------------------------------//

	public IEnumerator OpenDoor ()
	{
//		anim_door.SetBool ("Open_door", true);
//		for (int i = 0; i <= anim_ring.Length; i++) {
		anim_ring [0].SetBool (ring_disappear, true);
		anim_ring [1].SetBool (ring_disappear, true);

		anim_lock [0].SetBool (lock_disappear, true);
		anim_lock [1].SetBool (lock_disappear, true);

		doorsound.Play ();
		yield return new WaitForSeconds(1f);
		anim_door.SetBool (Open_door, true);
		doorclosed = false;
		//door.transform.Translate (0,DoorSpeed*Time.deltaTime,0);


	}

	public IEnumerator CloseDoor ()
	{
		anim_door.SetBool (Open_door, false);
		doorsound.Play ();


		yield return new WaitForSeconds(3f);

//		for (int i = 0; i <= anim_ring.Length; i++) {
			anim_ring [0].SetBool (ring_appear, true);
			anim_ring [1].SetBool (ring_appear, true);

			anim_lock [0].SetBool (lock_appear, true);
			anim_lock [1].SetBool (lock_appear, true);
		
		
		yield return new WaitForSeconds(.5f);

//		for (int i = 0; i <= anim_ring.Length; i++) {
			anim_ring [0].SetBool(ring_disappear, false);
			anim_ring [1].SetBool(ring_disappear, false);
			anim_lock [0].SetBool(lock_disappear, false);
			anim_lock [1].SetBool(lock_disappear, false);

			anim_ring [0].SetBool(ring_appear, false);
			anim_ring [1].SetBool(ring_appear, false);

			anim_lock [0].SetBool(lock_appear, false);
			anim_lock [1].SetBool(lock_appear, false);
			doorclosed = true;


	}


	//-------------Trigger  -----------------------------------------------------------//

	void OnTriggerEnter(Collider other)
	{

		triggerOverlap = true;

		if(other.tag == "Player" && doorUnloacked == false)
		{
            
            for (int i = 0; i < anim_ring.Length; i++) 
			{
				anim_ring [i].SetBool (animBool, true);
				anim_lock [i].SetBool (animBool, true);
			}
		}

	}

	void OnTriggerExit(Collider other)
	{	
		triggerOverlap = false;

		if (other.tag == "Player") 
		{
            
            for (int i = 0; i < anim_ring.Length; i++) 
			{
				anim_ring [i].SetBool (animBool, false);
				anim_lock [i].SetBool (animBool, false);
			}
		}
	}



}
