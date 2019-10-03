using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetBackground : MonoBehaviour {
	SpriteRenderer backgroundSprite;
    static bool background_created = false;

    // Use this for initialization
    void Awake () {
		//DontDestroyOnLoad (this);

            if (!background_created)
            {
                DontDestroyOnLoad(this.gameObject);
            background_created = true;
            }
            else
            {
                Destroy(this.gameObject);
            }
        //DontDestroyOnLoad(GameObject.FindGameObjectWithTag("MainCamera"));

        backgroundSprite = gameObject.GetComponent<SpriteRenderer> ();

		backgroundSprite.transform.localScale = new Vector3 (2.0f, 3.0f, 1.0f);

		int background = Random.Range (0, 6);
		switch (background) {
		case 0:
			backgroundSprite.sprite = Resources.Load<Sprite> ("Sprites/bluepurplewhite_gradient");
			break;
		case 1:
			backgroundSprite.sprite = Resources.Load<Sprite> ("Sprites/gradientBack");
			break;
		case 2:
			backgroundSprite.sprite = Resources.Load<Sprite> ("Sprites/browngradient");
			break;
		case 3:
			backgroundSprite.sprite = Resources.Load<Sprite> ("Sprites/blue_gradient");
			break;
		case 4:
			backgroundSprite.sprite = Resources.Load<Sprite> ("Sprites/greyred_gradient");
			break;
		case 5:
			backgroundSprite.sprite = Resources.Load<Sprite> ("Sprites/blue_gradient");
			break;
		}


	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
