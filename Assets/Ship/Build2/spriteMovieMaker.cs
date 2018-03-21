using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.IO;


public class spriteMovieMaker : MonoBehaviour {

/*	string mypath = "/Users/RK/Documents/Nanolumina/SpaceShip Build/Animations/barWave/barWave2/";
	DirectoryInfo dir = new DirectoryInfo (mypath);
	public FileInfo[] files = dir.GetFiles("*.*");
*/


	public Sprite[] sprites;
	int leng;
	public int frameRate = 12;
	private float timer = 0f;
	private int currentFrame = 0;
	public int counter = 0;

	public Image image;




	// Use this for initialization
	void Start()
	{
		
		leng = sprites.Length;

		image = gameObject.GetComponent<Image> ();
	}


	void Update()
	{ 

//		if (counter <= leng) {
			counter++;
			counter %= leng;
			setSprite (counter);
			
//		}

//		else
//		{	counter = 0;	
//		}
//
		}


	public void setSprite(int counter)
			{
		image.sprite = sprites[counter];
			}


}



/*/*	public Material playerMaterial; 
	public Texture[] playerImages; 
	public string imagePrefix; 
	public int frameRate = 12;

	private float timer = 0f; 
	private int currentFrame = 0;

	public void Start(){ 
		if (playerMaterial.Equals(null))
		{ 
			playerMaterial = gameObject.GetComponent<Renderer>().material; 
		}

		if (playerImages.Length == 0) 
		{ 
			Object[] allImages = Resources.LoadAll (imagePrefix); 
			playerImages = new Texture[allImages.Length]; 
		}
	}

	public void Update()
				{ 
					timer += Time.deltaTime;

					


					if (timer > 1f/frameRate)
						{ 
						 
						currentFrame ++;
						currentFrame = currentFrame % playerImages.Length;
						SetFrame(currentFrame); 
						} 
				}

	public void SetFrame(int currentFrame)
				{ 
					playerMaterial.mainTexture = playerImages[currentFrame]; 
					} 
				}*///



