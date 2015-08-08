using UnityEngine;
using System.Collections;

public class Parachute : MonoBehaviour
{
	
	private CharacterControls charControls;
	private float LyData;
	private float RyData;
	public float diff;
	public float force;
	public float aForce;
	public float bForce;
	public float gravChange = 1.0f;
	public bool grounded;
	public float LeftY;
	public float prevLY;
	public float curLY;
	public float RightY;
	public float prevRY;
	public float curRY;
	public float Ldelta;
	public float Rdelta;
	public float timer = 0.0f;
	public float maxTime = 5.0f;
	private bool StartTimer;
	public GameObject para;
	private bool ParaActive;
	public float ParaDrag;
	public float UKick = 100f;
	public float BKick = 10000f;
	public float Mass;
	private bool startparaTimer;
	public float paraTimer;
	public float paraMaxTime;

	// Use this for initialization


	void Start ()
	{
	
		charControls = GetComponent<CharacterControls> ();

		grounded = false;
		StartTimer = false;
		ParaActive = false;
		startparaTimer = false;

		LyData = gameObject.GetComponent<ZigSkeleton> ().LW;
		RyData = gameObject.GetComponent<ZigSkeleton> ().RW;



		//charControls.rigidbody.drag = 100.0f;0
	}

	void OnCollisionEnter (Collision c)
	{

		if (c.gameObject.tag == "Ground") {
			grounded = true;
		}
	}

	void FixedUpdate ()
	{

		LyData = gameObject.GetComponent<ZigSkeleton> ().LW;
		RyData = gameObject.GetComponent<ZigSkeleton> ().RW;
		diff = LyData - RyData;


		prevLY = curLY;
		curLY = LyData;
		
		prevRY = curRY;
		curRY = RyData;
		
		Ldelta = curLY - prevLY;
		Rdelta = curRY - prevRY;

	}
	
	// Update is called once per frame
	void Update ()
	{

		if (Input.GetKey (KeyCode.R)) {
			Application.LoadLevel (Application.loadedLevel);
		}

		if (Input.GetKey (KeyCode.O)) {
			//charControls.rigidbody.drag = 1.0f;
			StartTimer = true;
		}

		if (StartTimer == true) {
			timer += Time.deltaTime;
			print ("timer " + timer);
		}

		if (startparaTimer == true) {
			paraTimer +=Time.deltaTime;
		}





		if (timer >= maxTime) {
			print ("Timer Done");
			charControls.rigidbody.AddForce (Vector3.up * UKick);
			charControls.rigidbody.AddForce (Vector3.back * BKick);
			StartTimer = false;
			startparaTimer = true;
			timer = 0.0f;
			charControls.rigidbody.useGravity = true;
			charControls.rigidbody.mass = Mass;

			//charControls.rigidbody.drag = 100.0f;
		}


		print ("diff" + diff);
		print ("Left Wrist Y" + LyData);
		print ("Right Wrist Y" + RyData);

		print ("Force" + force);
		print ("Grounding" + grounded);
		print ("Left Delta" + Ldelta);
		print ("Right Delta" + Rdelta);

		/*if (Input.GetKey (KeyCode.P)){

			//player.GetComponent<CharacterControls> ().gravity (10.0f);
			charControls.rigidbody.AddForce(Vector3.up * force);
			charControls.gravity = gravChange; 
			force = 100.0f;
		}*/

		if (StartTimer == false) {
			//charControls.rigidbody.drag = 50.0f;
		}

		//everything that happens when falling should be put in this if statement
		if (grounded == false) {
			if (Input.GetKey (KeyCode.P) || paraTimer >= paraMaxTime) {
				if (ParaActive == false) {

					Transform t = ((GameObject)Instantiate (para, transform.position, transform.rotation)).transform;
					t.parent = transform;
					charControls.rigidbody.drag = ParaDrag;

					//player.GetComponent<CharacterControls> ().gravity (10.0f);
					charControls.rigidbody.AddForce (Vector3.up * force);
					//charControls.gravity = gravChange; 
					force = 100.0f;
					print ("parachute active");
					ParaActive = true;

					charControls.rigidbody.centerOfMass = new Vector3 (0f, 2f, 0f);
				}
			}



			if (ParaActive == true) {

				if (diff <= -0.3f || Input.GetKey (KeyCode.A)) {

					Vector3 wLeft = transform.TransformVector (Vector3.left * bForce);
					Vector3 forcePos = transform.TransformVector (new Vector3 (0f, 0.75f, 0f));
					charControls.rigidbody.AddForceAtPosition (wLeft, forcePos, ForceMode.Impulse);

					Vector3 wForward = transform.TransformVector (Vector3.forward * aForce);
					charControls.rigidbody.AddForce (wForward);

					charControls.rigidbody.AddForce (Vector3.down * 1.0f);
				}

				if (diff >= 0.3f || Input.GetKey (KeyCode.D)) {

					Vector3 wRight = transform.TransformVector (Vector3.right * bForce);
					Vector3 forcePos = transform.TransformVector (new Vector3 (0f, 0.75f, 0f));
					charControls.rigidbody.AddForceAtPosition (wRight, forcePos, ForceMode.Impulse);

					Vector3 wForward = transform.TransformVector (Vector3.forward * aForce);
					charControls.rigidbody.AddForce (wForward);

					charControls.rigidbody.AddForce (Vector3.down * 1.0f);
				}
			}
		} else if (grounded == true) {
			Destroy (GameObject.FindGameObjectWithTag("Para"));
			charControls.audio.volume = 0.60f;
			charControls.rigidbody.constraints = RigidbodyConstraints.FreezeAll;
		}
	}

}
