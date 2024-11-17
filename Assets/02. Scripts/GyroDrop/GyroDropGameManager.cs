using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GyroDropGameManager : MonoBehaviour
{
    [SerializeField] GyroDropSceneManager _gyrodropSceneManager;
    [SerializeField] GyroDropUIManager _gyrodropUIManager;
    [SerializeField] GyroDropSoundManager _gyrodropSoundManager;

    // [ ���� ������Ʈ ���� ]
    public GameObject disk;                
    private GameObject cameraObject;        
    public GameObject XRoom;
    public GameObject[] platformPieces;    
    public GameObject Player;
    public GameObject Firework;
    public GameObject[] Lights;

    // [ UI ]
    public Text TimerText;                 
    public Slider HeightSlider;             
    public GameObject ArrowObject1; public GameObject ArrowObject2;
    public Sprite Arrow;            // ȭ��ǥ : �⺻ �ð���� 
    public Sprite Arrow_reverse;    // �ð�ݴ���� 

    private float remainingTime;

    public Material material_gray;                 
    public Material material_green;                 
    public Material material_blue;                 
    public Material material_orange;
    public Material material_red;

    // [ ���� ]
    private const float TargetYPosition = 525f;  // ī�޶� ��ǥ Y ��ġ
    private const float DiskCameraOffset = 41f;  // ���ǰ� ī�޶� �� ������
    private const float TotalRiseDuration = 60f; // ��ü ��¿� �ɸ��� �ð�

    private const float LowerPercentage = 0.05f; // �ϰ� ���� (5%)
    private const float ClearTimeLimit = 90f;   // ���� ���� �ð� (1��30��)

    // [ ���� �÷��� ]
    private bool isRising = false;
    public bool gameEnded = false;           // ������ ����Ǿ����� ����
    public bool pausedOnce = false;          // 50���� �� ���� ���߱� ���� �÷���
    public bool isCollisionDetected = false;  // �浹 �߻� ����
    public bool isCollisionOngoing = false;
    private bool isCountdown;
    public bool isTrigger_center = false;

    // [ ȸ�� �� �ӵ� ]
    private float RotationSpeed = 20f;     // ���� ȸ�� �ӵ�
    private int RotationDirection = 1;     // 1: �ð� ����, -1: �ݽð� ����
    private float riseSpeed;               // ī�޶� ��� �ӵ�


    // 4���� ������ ���� ���� �÷��� �迭
    private bool[] levelUpExecuted = new bool[4];
    private readonly float[] thresholds = { 0.2f, 0.4f, 0.6f, 0.8f };



    void Start()
    {
        // �� �ÿ� ��, �ʿ� 
        cameraObject = GameObject.Find("SpoutCamera"); 
        //cameraObject = GameObject.Find("SpoutCamera_2new"); // �׽�Ʈ�� 

        riseSpeed = (TargetYPosition - 10f) / TotalRiseDuration; // ��� �ӵ� ��� (��ǥ ��ġ���� ���� �ð��� �°�)
        print("��� �ӵ� = " + riseSpeed);

        remainingTime = ClearTimeLimit; 

        _gyrodropSoundManager.PlayBGM(); 
        _gyrodropUIManager.FadeInImage(); 

        StartCoroutine(StartGameGuide());

        StartCoroutine(PlatformHoleRoutine());

        StartCoroutine(ChangeRotationDirectionRoutine()); // Y ��ǥ�� 50 �̻��� ������ ȸ�� ���� ���� ��ƾ ����

    }


    // �� ���� ����
    IEnumerator StartGameGuide()
    {
        yield return new WaitForSeconds(5f);

        _gyrodropUIManager.StartCountDown( );
        yield return new WaitForSeconds(7f);

        _gyrodropUIManager.ShowStartArrows( );
        yield return new WaitForSeconds(4f);

        isRising = true;
        _gyrodropUIManager.HideStartArrows();

        StartCoroutine(RiseCoroutine()); // ��� : 1�� ���� ���� 
    }


    void Update()
    {
        if (isRising && !gameEnded)
        {
            disk.transform.Rotate(Vector3.up, RotationSpeed * RotationDirection * Time.deltaTime);

            UpdateDisk(); 
            UpdateLevel();

            HeightSlider.value = (cameraObject.transform.position.y / TargetYPosition); 
            UpdateTimerText();                                                          
        }

        if (cameraObject.transform.position.y >= TargetYPosition) 
        {
            RestoreAllPlatformPieces();  
        }

    }

    // Ÿ�̸� ���� 
    private void UpdateTimerText()
    {
        if (pausedOnce)
        {
            remainingTime -= Time.deltaTime;

            if (remainingTime <= 0)
            {
                remainingTime = 0;  
                StartCoroutine(GameOver());
            }
            else if (remainingTime <= 10 && !isCountdown)
            {
                isCountdown = true;
                _gyrodropSoundManager.Play_CountDown();
            }
            else
            {
                int minutes = Mathf.FloorToInt(remainingTime / 60);
                int seconds = Mathf.FloorToInt(remainingTime % 60);
                TimerText.text = $"{minutes:00}:{seconds:00}";
            }
        }
        
    }


    // ��� ���� 
    private IEnumerator RiseCoroutine()
    {
        while (!gameEnded && cameraObject.transform.position.y < TargetYPosition)
        {
            float currentY = cameraObject.transform.position.y;


            if (currentY>=30f && !pausedOnce)
            {
                _gyrodropUIManager.StartCountDown2();

                yield return new WaitForSeconds(3f);

                Player.SetActive(true); Debug.Log("�÷��̾ Ȱ��ȭ �˴ϴ�.");
                pausedOnce = true; 
            }

            MoveCameraAndDisk(currentY + riseSpeed * Time.deltaTime);
            yield return null;
        }

        // ��ǥ ���̿� �����ϸ� ���� Ŭ���� ó��
        if (!gameEnded) StartCoroutine(GameClear()); 
    }



    // �� [ ī�޶� - ���� ��ġ ]  ------------------------------------------------------------------------------------------------
    // 
    private void MoveCameraAndDisk(float newY)
    {
        newY = Mathf.Min(newY, TargetYPosition); // ��ǥ ���� �ʰ� ����
        cameraObject.transform.position = new Vector3(cameraObject.transform.position.x, newY, cameraObject.transform.position.z);
        disk.transform.position = new Vector3(disk.transform.position.x, newY - DiskCameraOffset, disk.transform.position.z);
        XRoom.transform.position = new Vector3(XRoom.transform.position.x, newY, XRoom.transform.position.z); 
    }



    // ----------------------------------------------------------------------------------------------------------------
    // �� [ ���� ���� ] -------------------------------------------------------------------------------------

    private IEnumerator PlatformHoleRoutine()
    {
        isCollisionDetected = false; // ��ƾ ���� �� �浹 �÷��� �ʱ�ȭ

        while (!gameEnded)
        {
            if (cameraObject.transform.position.y >= 40f)  // Y ��ǥ�� 40 �̻��� ���� ���� ����
            {
                int numHoles = GetNumberOfHolesBasedOnHeight(); // ���̿� ���� ���� ���� ����

                List<GameObject> selectedPieces = new List<GameObject>();
                for (int i = 0; i < numHoles; i++)
                {
                    GameObject piece;
                    do
                    {
                        piece = platformPieces[Random.Range(0, platformPieces.Length)];
                    } while (selectedPieces.Contains(piece)); // �ߺ� ����

                    selectedPieces.Add(piece);
                }

                foreach (GameObject piece in selectedPieces)
                {
                    StartCoroutine(BlinkPlatform(piece));
                }
            }
            yield return new WaitForSeconds(10f);

            isCollisionDetected = false; // �浹 �÷��� �ʱ�ȭ
        }
    }


    // ���̿� ���� ���� ���� ����
    private int GetNumberOfHolesBasedOnHeight()
    {
        float height = cameraObject.transform.position.y;

        if      (height >= 300f) { return Random.Range(3, 5);  } // 3�� �Ǵ� 4��
        else if (height >= 100f) { return Random.Range(2, 4); } // 2�� �Ǵ� 3��
        else                     { return 2; } // �⺻ 2��
    }


    // ���� ������ 
    private IEnumerator BlinkPlatform(GameObject piece)
    {
        PlatformPiece platformPiece = piece.GetComponent<PlatformPiece>();
        if (platformPiece != null)
        {
            _gyrodropSoundManager.HoleWarining_SFX();
            platformPiece.StartBlinking(0.5f, 5); 
        }

        yield return new WaitForSeconds(8f); 

        piece.GetComponent<Renderer>().enabled = true;   // ���� ���� 
        piece.GetComponent<Collider>().enabled = false;

        if (platformPiece != null)
        {
            platformPiece.DeactivateWarningUI(); // ���� ���� �� WarningUI ��Ȱ��ȭ
        }

    }


    // ���� ����/�ӵ� ���� 
    private void UpdateDisk()
    {
        float heightPercentage = cameraObject.transform.position.y / TargetYPosition;

        Material newMaterial = null;
        if (heightPercentage >= 0.8f) { newMaterial = material_red; RotationSpeed = 10f * 2.5f;  }        // �ӵ� 2.5��
        else if (heightPercentage >= 0.6f) { newMaterial = material_orange; RotationSpeed = 10f * 2f; }   // �ӵ� 2��
        else if (heightPercentage >= 0.4f) { newMaterial = material_blue; RotationSpeed = 10f * 1.7f; }   // �ӵ� 1.7��
        else if (heightPercentage >= 0.2f) { newMaterial = material_green; RotationSpeed = 10f * 1.3f; }  // �ӵ� 1.3��
        else { newMaterial = material_gray; }

        if (newMaterial != null)
        {
            foreach (GameObject piece in platformPieces)
            {
                piece.GetComponent<Renderer>().material = newMaterial;
            }
        }

    }


    // ȸ�� ���� ���� (8~12�� ���� ����)
    private IEnumerator ChangeRotationDirectionRoutine()
    {
        while (!gameEnded)
        {
            if (cameraObject.transform.position.y >= 50f)
            {
                _gyrodropUIManager.UpdateArrowSprites(RotationDirection); 

                _gyrodropUIManager.ShowArrows( );

                yield return new WaitForSeconds(3f); 

                RotationDirection *= -1; // ���� ���� : �ð� ���� �� �ð� �ݴ� ���� ��ȯ

                _gyrodropUIManager.HideArrows();
            }

            yield return new WaitForSeconds(Random.Range(8f, 12f));

        }
    }
    

    // ----------------------------------------------------------------------------------------------------------
    // �� [ �浹 �߻� �� ȣ��Ǵ� �Լ� ] �� ---------------------------------------------------------------------

    public void HandleCollision()
    {
        if (!isCollisionDetected)
        {
            isCollisionDetected = true;    // ù �浹�� �νĵǵ��� 
            Debug.Log("�浹 �߻�: �ϰ� ����!");

            _gyrodropSoundManager.Hole_SFX();

            StartCoroutine(LowerHeight()); // �ϰ� ����
        }
    }


    // �� �浹 ��, �ϰ� 
    // 
    private IEnumerator LowerHeight()
    {
        float currentY = cameraObject.transform.position.y;
        float targetY = currentY - (TargetYPosition * LowerPercentage);
        targetY = Mathf.Max(targetY, 0); // �ּ� ���� ����

        float duration = 1f; // �ϰ��� �ɸ� �ð� (1��)
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float newY = Mathf.Lerp(currentY, targetY, elapsedTime / duration);
            MoveCameraAndDisk(newY);

            elapsedTime += Time.deltaTime;
            yield return null; // ���� �����ӱ��� ���
        }

        MoveCameraAndDisk(targetY); // ������ ��ġ�� ��Ȯ�� �̵�

        _gyrodropUIManager.FinishWarning(); // PlatformPiece���� Start ���� 
    }


    // [ ���� �߰��� �� ���� �� "center" �浹 ] --------------------------------------------------------------------------

    #region �ӽ� Center �浹 ���� : ��Ȱ��ȭ ���� X 
    /*
    public void OnPlayerCollidedWithCenter()
    {
        if (!warningDisplayed && !gameEnded)
        {
            warningDisplayed = true;
            isCollisionOngoing = true;
            _gyrodropSoundManager.Hole_SFX();
            _gyrodropUIManager.StartWarning();

            StartCoroutine(WaitAndCheckCollision(5f));
        }
    }

    // ���� �浹 ���� �÷��̾ �����ִ��� Ȯ��
    public void CheckCollisionStatus(int remainingCollisions)
    {
        if (remainingCollisions == 0)
        {
            isCollisionOngoing = false;
            warningDisplayed = false; // ��� �浹�� ����Ǹ� �ʱ�ȭ
            _gyrodropUIManager.FinishWarning();
        }
    }

    // 5�� ��� �� �浹 ���� Ȯ��
    private IEnumerator WaitAndCheckCollision(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        if (isCollisionOngoing)
        {
            StartCoroutine(ContinuousLowering());
        }

    }

    // �浹 ���� �� ī�޶�� ���� �ϰ�
    public IEnumerator ContinuousLowering()
    {
        while (isCollisionOngoing && !gameEnded)
        {
            float newY = cameraObject.transform.position.y - (TargetYPosition * LowerPercentage * Time.deltaTime);
            MoveCameraAndDisk(Mathf.Max(newY, 0));

            if (cameraObject.transform.position.y <= 30)
            {
                StartCoroutine(GameOver());
                break;
            }

            yield return null;
        }
    }
    */
    #endregion


    public void CenterDrop()
    {
        if (isCollisionOngoing && !gameEnded)
        {
            StartCoroutine(DropLowering());
        }

    }

    public IEnumerator DropLowering()
    {
        while (isCollisionOngoing && !gameEnded)
        {
            float newY = cameraObject.transform.position.y - (TargetYPosition * LowerPercentage * Time.deltaTime);
            MoveCameraAndDisk(Mathf.Max(newY, 0));

            if (cameraObject.transform.position.y <= 30)
            {
                StartCoroutine(GameOver());
                break;
            }
            yield return null;
        }
    }



    // [ UI ] -------------------------------------------------------------------------------------------------------

    void UpdateLevel()
    {
        float heightPercentage = cameraObject.transform.position.y / TargetYPosition;

        for (int i = 0; i < thresholds.Length; i++)
        {
            if (heightPercentage >= thresholds[i] && !levelUpExecuted[i])
            {
                levelUpExecuted[i] = true; // �ش� ���� �÷��� ����
                // _gyrodropUIManager.LevelUpUI();
                // _gyrodropSoundManager.Play_LevelUp();
            }
        }
    }


    // --------------------------------------------------------------------------------------------------------------
    // �� [ ���� Ŭ����/���� ] �� -----------------------------------------------------------------------------------

    IEnumerator GameClear() 
    {
        gameEnded = true;
        Firework.SetActive(true);

        _gyrodropSoundManager.Play_GameClear();
        _gyrodropUIManager.FinishWarning();
        _gyrodropUIManager.HideArrows();
        _gyrodropUIManager.GameClearUI();

        yield return new WaitForSeconds(5f);
        _gyrodropSoundManager.Play_DropWarning();

        yield return new WaitForSeconds(8f);
        StartCoroutine(Drop(60));

    }

    IEnumerator GameOver()
    {
        gameEnded = true;
        _gyrodropSoundManager.Play_GameOver();
        _gyrodropUIManager.FinishWarning();
        _gyrodropUIManager.HideArrows();
        _gyrodropUIManager.GameOverUI();

        yield return new WaitForSeconds(5f);
        _gyrodropSoundManager.Play_DropWarning();
        StartCoroutine(TurnOffLight());

        yield return new WaitForSeconds(5f);
        StartCoroutine(Drop(6));

    }

    private IEnumerator Drop(float speedMultiplier)
    {
        while (cameraObject.transform.position.y > 0)
        {
            MoveCameraAndDisk(cameraObject.transform.position.y - riseSpeed * speedMultiplier * Time.deltaTime);
            yield return null;
        }
        Debug.Log("�ϰ� �Ϸ�.");

        StartCoroutine(ReturnMainMap());
    }

    // ��� ���� ���󺹱�(���̵���)
    private void RestoreAllPlatformPieces()
    {
        foreach (GameObject piece in platformPieces)
        {
            piece.GetComponent<Renderer>().enabled = true; // ������ ���̰� �� 
        }
    }


    // ���� �� ����
    IEnumerator ReturnMainMap()
    {
        yield return new WaitForSeconds(3f);
        _gyrodropUIManager.FadeOutImage();

        yield return new WaitForSeconds(5f);
        _gyrodropSceneManager.LoadMainMenuMap();
    }

    IEnumerator TurnOffLight()
    {
        foreach (GameObject light in Lights)
        {
            if (light != null) 
            {
                light.SetActive(false);
            }
            yield return new WaitForSeconds(1f); // 1�� ����
        }
    }
}
