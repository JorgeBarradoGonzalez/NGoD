using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleBehaviour : MonoBehaviour {

    public float speed = 1;
    public GameObject bottom;
    GameController gController;
    float gcSpeedChanger;
    public string nature;
	// Use this for initialization
	void Start () {

        gController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>(); 
        gcSpeedChanger = gController.everySpeedChanger; //gc will vary the speed of the obtacle when gameController is in the next phase of the difficulty
        speed = gController.obstacleSpeed;
        
    }
	
	// Update is called once per frame
	void Update () {

        if (gController.gameOver == false)
        {
            if (gcSpeedChanger != gController.everySpeedChanger)
            {
                gcSpeedChanger = gController.everySpeedChanger;
                speed = gController.obstacleSpeed;
            }

            transform.Translate(Vector3.down * gcSpeedChanger * speed * Time.deltaTime);
            if (transform.position.y <= bottom.transform.position.y)
            {

                if (name.Contains("God") || name.Contains("Devil"))
                {
                    //gController.gameLight.color = Color.white;
                    gController.changingLightColor = true;
                    gController.ChangeSkyOcclusion();

                }



                Destroy(gameObject);
            }
        }
        

    }
}
