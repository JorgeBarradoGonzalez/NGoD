using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameController : MonoBehaviour
{
    //public PlayerBehaviour player;
    public GameObject[] obstSpawnPos;
    public GameObject obstPrefab;
    //public GameObject EObstPrefab;
    public GameObject bottom;
    float timeToSpawn;
    public float obstacleSpeed = 2;
    public float obstacleCooldown = 2;
    public float everySpeedChanger = 1; //will vary all the speed in the game so they all scale the same
    List<int> randomRows = new List<int>();

    public bool gameStarted = false;
    public bool playPushed = false;
    public float timer = 0;
    float nextStageTimer = 10;
    int stage = 0;
    float elementDropTime = 10.0f;

    //
    public Text playerScore;
    public GameObject endGameAssets;
    bool gameOn;
    public bool gameOver = false;

    Renderer skyRend;
    Renderer mainMenuRend;
    public GameObject sky;
    public GameObject mainMenu;
    public GameObject playButton;
    public float scrollSpeed = 0.3f;
    public float offset;

    //
    public GameObject[] obstacleType;
    public Light gameLight;
    public bool changingLightColor = false;
    public string natureEffect = "Both";
    // Use this for initialization

    void Start()
    {
        skyRend = sky.GetComponent<Renderer>();
        mainMenuRend = mainMenu.GetComponent<Renderer>();
        //player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver == false)
        {
            timer += Time.deltaTime;
            SkyOffset();
            //ChangeSkyColor();
            if (changingLightColor)
            {
                ChangeSkyColor();
                gameLight.color = Color.Lerp(gameLight.color, Color.white, Time.deltaTime);
                if (gameLight.color == Color.white)
                {
                    Debug.Log("COLOR CHANGED");
                    changingLightColor = false;
                    natureEffect = "Both";
                }


            }
        }
        
    }

    IEnumerator ReleaseObstacles(float obstacleCooldown)
    {
        if (gameOver == false) {

            for (int i = 0; i < 1; i += 0)
            {
                InstantiateObstacle();
                yield return new WaitForSecondsRealtime(obstacleCooldown / everySpeedChanger); //changed 08/17 but I have my doubts on wether if it's to be divided or minus

            }
        }

        
    }

    void InstantiateObstacle()
    {
        int randomRow1 = Random.Range(0, 3);
        int randomRow2 = Random.Range(0, 3);

        while (randomRow1 == randomRow2)
        {
            randomRow2 = Random.Range(0, 3);
        }

        GameObject obstPrefabClone = (GameObject)Instantiate(obstPrefab, new Vector3(obstSpawnPos[randomRow1].transform.position.x, obstSpawnPos[randomRow1].transform.position.y, obstSpawnPos[randomRow1].transform.position.z), Quaternion.identity);
        GameObject obstPrefabClone2 = (GameObject)Instantiate(obstPrefab, new Vector3(obstSpawnPos[randomRow2].transform.position.x, obstSpawnPos[randomRow2].transform.position.y, obstSpawnPos[randomRow2].transform.position.z), Quaternion.identity);
        obstPrefabClone.gameObject.GetComponent<ObstacleBehaviour>().bottom = bottom;
        obstPrefabClone2.gameObject.GetComponent<ObstacleBehaviour>().bottom = bottom;

    }

    public IEnumerator ElementObstacleDrop()
    {
        if (gameOver == false)
        {
            yield return new WaitForSecondsRealtime(elementDropTime);
            InstantiateElementObstacle();
            elementDropTime = Random.Range(0, 16);
            yield return new WaitForSecondsRealtime(3.0f);
            StartCoroutine("ElementObstacleDrop");
        }
        
    }

    void InstantiateElementObstacle()
    {
        if (gameOver == false)
        {
            int elementType = Random.Range(0, 3);
            switch (elementType)
            {
                case 0:
                    GameObject obstacleTypeDevilClone = (GameObject)Instantiate(obstacleType[0].gameObject, new Vector3(obstSpawnPos[1].transform.position.x, obstSpawnPos[1].transform.position.y, obstSpawnPos[1].transform.position.z), Quaternion.identity);
                    obstacleTypeDevilClone.gameObject.GetComponent<ObstacleBehaviour>().bottom = bottom;
                    obstacleTypeDevilClone.gameObject.GetComponent<ObstacleBehaviour>().nature = "Devil";
                    if (changingLightColor == true)
                    {
                        changingLightColor = false;
                    }
                    gameLight.color = Color.red;
                    natureEffect = "Devil";
                    skyRend.material.SetColor("_Color", Color.red);
                    break;
                case 1:
                    GameObject obstacleTypeBothClone = (GameObject)Instantiate(obstacleType[1].gameObject, new Vector3(obstSpawnPos[1].transform.position.x, obstSpawnPos[1].transform.position.y, obstSpawnPos[1].transform.position.z), Quaternion.identity);
                    obstacleTypeBothClone.gameObject.GetComponent<ObstacleBehaviour>().bottom = bottom;
                    obstacleTypeBothClone.gameObject.GetComponent<ObstacleBehaviour>().nature = "Both";
                    if (changingLightColor == true)
                    {
                        changingLightColor = false;
                    }
                    gameLight.color = Color.white;
                    natureEffect = "Both";
                    break;
                case 2:
                    GameObject obstacleTypeGodClone = (GameObject)Instantiate(obstacleType[2].gameObject, new Vector3(obstSpawnPos[1].transform.position.x, obstSpawnPos[1].transform.position.y, obstSpawnPos[1].transform.position.z), Quaternion.identity);
                    obstacleTypeGodClone.gameObject.GetComponent<ObstacleBehaviour>().bottom = bottom;
                    obstacleTypeGodClone.gameObject.GetComponent<ObstacleBehaviour>().nature = "God";
                    if (changingLightColor == true)
                    {
                        changingLightColor = false;
                    }
                    gameLight.color = Color.black;
                    natureEffect = "God";
                    skyRend.material.SetFloat("_OcclusionStrength", 1.0f);
                    break;
            }
        }
        

    }

    public void StartGameTime()
    {
        timer = 0;
    }

    public IEnumerator CheckStageAndMoveOn()
    {
        if (gameOver == false)
        {
            yield return new WaitForSecondsRealtime(nextStageTimer);

            switch (stage)
            {
                case 0:
                    stage = 1;
                    everySpeedChanger = 2.3f;
                    LessCooldown();

                    nextStageTimer = 8.0f;
                    break;
                case 1:
                    stage = 2;
                    everySpeedChanger = 2.6f;
                    LessCooldown();
                    break;
                case 2:
                    stage = 3;
                    everySpeedChanger = 2.9f;
                    LessCooldown();
                    break;
                case 3:
                    stage = 4;
                    everySpeedChanger = 3.2f;
                    LessCooldown();
                    break;
                case 4:
                    stage = 5;
                    everySpeedChanger = 3.5f;
                    LessCooldown();
                    break;
                case 5:
                    stage = 6;
                    everySpeedChanger = 3.8f;
                    LessCooldown();
                    break;
                case 6:
                    stage = 7;
                    everySpeedChanger = 4.1f;
                    LessCooldown();

                    nextStageTimer = 20.0f;
                    break;
                case 7:
                    stage = 8;
                    everySpeedChanger = 4.4f;
                    LessCooldown();

                    nextStageTimer = 15.0f;
                    break;
            }
            StartCoroutine("CheckStageAndMoveOn");
        }
        

    }

    void LessCooldown()
    {
        obstacleCooldown = obstacleCooldown - 0.2f;
    }

    public void GameOff() {

        //playerScore.text = "Your time: " + timer;
        playerScore.text = "Your time: " + Mathf.Round(timer) +" "+ "sec";
        endGameAssets.gameObject.SetActive(true);
        gameOn = false;
        gameOver = true;
        everySpeedChanger = 0;
    }

    public void CloseApp()
    {
        Application.Quit();
    }
    public void PlayApp()
    {
        SceneManager.LoadScene("ProjectElementD");
    }

    public void SkyOffset()
    {
        offset = Time.time * scrollSpeed; //* everySpeedChanger;
        skyRend.material.SetTextureOffset("_MainTex", new Vector2(0, offset));
    }

    void ChangeSkyColor() {
        
        if (natureEffect == "Devil")
        {

            skyRend.material.SetColor("_Color", Color.Lerp(Color.red,Color.white,Time.time * 100));
        }
        
        //skyRend.material.SetFloat("_OcclusionStrength", Mathf.Lerp(1.0f, 0.0f, Time.deltaTime * 2));
    }

    public void ChangeSkyOcclusion()
    {

        if (natureEffect == "God")
        {

            //skyRend.material.SetFloat("_OcclusionStrength", Mathf.Lerp(1.0f, 0.0f, Time.deltaTime));
            skyRend.material.SetFloat("_OcclusionStrength", 0.0f);
        }

        //skyRend.material.SetFloat("_OcclusionStrength", Mathf.Lerp(1.0f, 0.0f, Time.deltaTime * 2));
    }

    public void StartGame()
    {
        playPushed = true;
        Destroy(playButton);
        mainMenuRend.material.color = new Color(255, 255, 255, 0);
        Time.timeScale = 1;
        //mainMenuRend.material.color = new Color(1f, 1f, 1f,0.0f);
        //mainMenuRend.material.color = Color.Lerp((1f, 1f, 1f, 1f), (1f, 1f, 1f, 0.0f),Time.deltaTime);
        //mainMenuRend.material.color = new Color(1f, 1f, 1f, Mathf.Lerp(1f,0f,Time.deltaTime));
        //Color.Lerp(mainMenuRend.material.color, new Color(1f, 1f, 1f,0f),Time.deltaTime);
        //mainMenuAlphaColor = Mathf.Lerp(240.0f, 0.0f, Time.deltaTime * 100);
        //Color.Lerp(mainMenuRend.material.color, new Color(1f, 1f, 1f, 0f), Time.deltaTime);
        //menuAlpha -= 0.01f;
        //mainMenuRend.material.color = new Color(255,255,255, Mathf.Lerp(1f, 0f, Time.deltaTime * 10));
    }

}
