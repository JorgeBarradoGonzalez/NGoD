using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{

    private int row = 1;
    Touch touch;
    public float speed = 1; //63
    Vector2 startPos;
    Vector2 endPos;
    //float startPosY,endPosY;
    int YLimit = 80;

    public GameObject posLeft, posMid, posRight;
    Renderer rend;
    public string natureType;
    GameController gController;

    public GameObject[] wingsNature;
    public GameObject[] aureolaType;

    //
    public Animator anim;
    // Use this for initialization
    void Start()
    {
        rend = GetComponent<Renderer>();
        gController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        natureType = "Both";
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.touchCount > 0 && gController.playPushed == true) 
        {


            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {

                if (gController.gameStarted == false)
                {
                    gController.gameStarted = true;
                    gController.StartGameTime();
                    wingsNature[1].GetComponent<Animator>().SetBool("GameOn", true);
                    gController.StartCoroutine("ReleaseObstacles", gController.obstacleCooldown);
                    gController.StartCoroutine("CheckStageAndMoveOn");
                    gController.StartCoroutine("ElementObstacleDrop");
                    
                }
                //Debug.Log("START");
                startPos = Input.GetTouch(0).position;
            }

            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                //Debug.Log("MOVED");
            }

            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                //Debug.Log("END");
                endPos = Input.GetTouch(0).position;

                if (startPos.y > endPos.y)
                {

                    //Debug.Log("ABAJO");
                    if (Mathf.Abs(startPos.y - endPos.y) > YLimit)
                    {
                        switch (natureType)
                        {
                            case "God":
                                //rend.material.color = Color.white;
                                natureType = "Both";
                                //
                                wingsNature[2].SetActive(false);
                                wingsNature[1].SetActive(true);

                                aureolaType[2].SetActive(false);
                                aureolaType[1].SetActive(true);
                                break;
                            case "Both":
                                //rend.material.color = Color.red;
                                natureType = "Devil";
                                //
                                wingsNature[1].SetActive(false);
                                wingsNature[0].SetActive(true);

                                aureolaType[1].SetActive(false);
                                aureolaType[0].SetActive(true);
                                break;

                            case "Devil":
                                //rend.material.color = Color.blue;
                                natureType = "God";
                                //
                                wingsNature[0].SetActive(false);
                                wingsNature[2].SetActive(true);

                                aureolaType[0].SetActive(false);
                                aureolaType[2].SetActive(true);
                                break;
                        }
                        //Debug.Log("Abajo es: " + Mathf.Abs(startPos.y - endPos.y));
                    }
                    else { PlayerMovement(); }

                }
                else
                { //Debug.Log("ARRIBA");

                    if (Mathf.Abs(startPos.y - endPos.y) > YLimit)
                    {
                        switch (natureType)
                        {
                            case "God":
                                //rend.material.color = Color.red;
                                natureType = "Devil";
                                //
                                wingsNature[2].SetActive(false);
                                wingsNature[0].SetActive(true);

                                aureolaType[2].SetActive(false);
                                aureolaType[0].SetActive(true);
                                break;
                            case "Both":
                                
                                if (gController.gameStarted == true) //this was coded bc it kept changing to God Nature whenever play was pressed
                                {
                                    //rend.material.color = Color.blue;
                                    natureType = "God";
                                    //
                                    wingsNature[1].SetActive(false);
                                    wingsNature[2].SetActive(true);

                                    aureolaType[1].SetActive(false);
                                    aureolaType[2].SetActive(true);

                                }
                                break;

                            case "Devil":
                                //rend.material.color = Color.white;
                                natureType = "Both";
                                //
                                wingsNature[0].SetActive(false);
                                wingsNature[1].SetActive(true);

                                aureolaType[0].SetActive(false);
                                aureolaType[1].SetActive(true);
                                break;
                        }
                    }
                    else { PlayerMovement(); }
                }

            }


        }

    }

    private void PlayerMovement()
    {
        if (startPos.x > endPos.x)
        {

            //Debug.Log("LEFT");
            switch (row)
            {

                case 0:
                    break;

                case 1:
                    transform.position = Vector3.MoveTowards(transform.position, posLeft.transform.position, speed * Time.deltaTime);
                    row = 0;
                    break;
                case 2:
                    transform.position = Vector3.MoveTowards(transform.position, posMid.transform.position, speed * Time.deltaTime);
                    row = 1;
                    break;
            }

        }

        else if (startPos.x < endPos.x)
        {
            //Debug.Log("RIGHT");

            switch (row)
            {

                case 0:
                    transform.position = Vector3.MoveTowards(transform.position, posMid.transform.position, speed * Time.deltaTime);
                    row = 1;
                    break;

                case 1:
                    transform.position = Vector3.MoveTowards(transform.position, posRight.transform.position, speed * Time.deltaTime);
                    row = 2;
                    break;
                case 2:
                    break;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name.Contains("Obstacle"))
        {
            gController.GameOff();
            Destroy(gameObject);
            //Debug.Log("normal");
        }
        else
        {
            {
                if (natureType == col.GetComponent<ObstacleBehaviour>().nature)
                {
                    Debug.Log("its okay");
                }
                else
                {
                    gController.GameOff();
                    Destroy(gameObject);
                }

                //Debug.Log("element");
            }
        }

        
       
        
    }

    public void ChangeToBoth()
    {
        natureType = "Both";
        wingsNature[0].SetActive(false);
        wingsNature[1].SetActive(true);
        aureolaType[0].SetActive(false);
        aureolaType[1].SetActive(true);
    }

}

