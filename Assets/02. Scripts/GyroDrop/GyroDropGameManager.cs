using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GyroDropGameManager : MonoBehaviour
{
    // [ ���� ������Ʈ ���� ]
    public GameObject disk;                // ����
    public GameObject cameraObject;        // ī�޶� 
    public GameObject XRoom;
    public GameObject[] platformPieces;    // ���� ������

    // [ UI ��� ]
    public Text TimerText;                 // Ÿ�̸� �ؽ�Ʈ
    public Slider HeightSlider;             // ���� �����̴�
    public Text WarningText;       // ��� �޽��� UI �ؽ�Ʈ

    // Ÿ�̸� ����
    private float remainingTime;

    // [ ���͸��� ���� ]
    public Material material_gray;                 
    public Material material_green;                 
    public Material material_blue;                 
    public Material material_orange;
    public Material material_red;

    // [ ���� ��� ]
    private const float TargetYPosition = 525f;  // ī�޶� ��ǥ Y ��ġ
    private const float DiskCameraOffset = 53f;  // ���ǰ� ī�޶� �� ������
    private const float PauseDuration = 5f;      // ��� ���� �ð�
    private const float TotalRiseDuration = 90f; // ��ü ��¿� �ɸ��� �ð�

    private const float LowerPercentage = 0.05f; // �ϰ� ���� (5%)
    private const float ClearTimeLimit = 120f;   // ���� ���� �ð� (2��)

    // [ ���� �÷��� ]
    private bool isRising = false;
    private bool gameEnded = false;           // ������ ����Ǿ����� ����
    private bool pausedOnce = false;          // 50���� �� ���� ���߱� ���� �÷���
    public bool isCollisionDetected = false;  // �浹 �߻� ����
    private bool warningDisplayed = false;  // ��� �޽��� 1ȸ ��� �÷���
    private bool isCollisionOngoing = false;


    // [ ȸ�� �� �ӵ� ]
    private float RotationSpeed = 10f;     // ���� ȸ�� �ӵ�
    private int RotationDirection = 1;     // 1: �ð� ����, -1: �ݽð� ����
    private float riseSpeed;               // ī�޶� ��� �ӵ�

    // [ ���� ]
    public AudioSource warningSound; // ����� ����


    [SerializeField] GyroDropSceneManager _gyrodropSceneManager;


    void Start()
    {
        Debug.Log("���� ����! ���� ���� �ö������.");

        riseSpeed = (TargetYPosition - 10f) / TotalRiseDuration; // ��� �ӵ� ��� (��ǥ ��ġ���� ���� �ð��� �°�)
        print("��� �ӵ� = " + riseSpeed);

        remainingTime = ClearTimeLimit; // ���� �ð� �ʱ�ȭ

        Invoke("StartRising", 5f);    // 5�� �� ī�޶� 1�� ��� ����
        
        StartCoroutine(PlatformHoleRoutine());

        StartCoroutine(ChangeRotationDirectionRoutine()); // Y ��ǥ�� 50 �̻��� ������ ȸ�� ���� ���� ��ƾ ����

    }

    void Update()
    {
        // ���� ���� �� ȸ�� ó��
        if (isRising && !gameEnded)
        {
            disk.transform.Rotate(Vector3.up, RotationSpeed * RotationDirection * Time.deltaTime);

            UpdateDiskMaterial(); // ���̿� ���� ���͸���� �ӵ� ����

            HeightSlider.value = (cameraObject.transform.position.y / TargetYPosition); // �����̴� ������Ʈ (0���� 1�� ���� ���)
            UpdateTimerText();                                                          // Ÿ�̸� �ؽ�Ʈ ������Ʈ
        }

        if (cameraObject.transform.position.y >= TargetYPosition) 
        {
            RestoreAllPlatformPieces();  // �ְ� ���̿� �������� �� : ���� �޿�� 
        }

    }


    private void UpdateTimerText()
    {
        if (pausedOnce)
        {
            remainingTime -= Time.deltaTime;

            if (remainingTime <= 0)
            {
                remainingTime = 0;  // Ÿ�̸Ӱ� ������ �������� �ʵ��� 0���� ����
                StartCoroutine(GameOver());
            }
            else
            {
                int minutes = Mathf.FloorToInt(remainingTime / 60);
                int seconds = Mathf.FloorToInt(remainingTime % 60);
                TimerText.text = $"{minutes:00}:{seconds:00}"; // "MM:SS" �������� �ؽ�Ʈ ������Ʈ
            }
        }
        
    }


    private void StartRising()
    {
        isRising = true; // ��� ����
        StartCoroutine(RiseCoroutine()); // ī�޶� ��� ��ƾ ����
    }


    private IEnumerator RiseCoroutine()
    {
        while (!gameEnded && cameraObject.transform.position.y < TargetYPosition)
        {
            float currentY = cameraObject.transform.position.y;

            // Y ��ǥ�� 30�� �� ���߰� 5�� ���
            if (currentY>=30f && !pausedOnce)
            {
                Debug.Log("ī�޶� ����! 5�� ��� �� ����.");
                yield return new WaitForSeconds(PauseDuration);

                pausedOnce = true; 
            }

            // ī�޶�� ������ ��� �̵�
            MoveCameraAndDisk(currentY + riseSpeed * Time.deltaTime);
            yield return null;
        }

        // ��ǥ ���̿� �����ϸ� ���� Ŭ���� ó��
        if (!gameEnded) StartCoroutine(GameClear()); Debug.Log("�ְ� ���̿� ���� =" + cameraObject.transform.position.y);
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


    // �� ���̿� ���� ���� ���� ����
    private int GetNumberOfHolesBasedOnHeight()
    {
        float height = cameraObject.transform.position.y;

        if      (height >= 300f) { return Random.Range(2, 4);  } // 2�� �Ǵ� 3��
        else if (height >= 100f) { return Random.Range(1, 3); } // 1�� �Ǵ� 2��
        else                     { return 1; } // �⺻ 1��
    }


    // �� ���� ������ �Լ� ȣ�� (5�� ����)
    private IEnumerator BlinkPlatform(GameObject piece)
    {
        PlatformPiece platformPiece = piece.GetComponent<PlatformPiece>();
        if (platformPiece != null)
        {
            platformPiece.StartBlinking(0.5f, 5); 
        }

        yield return new WaitForSeconds(5f); 

        piece.GetComponent<Renderer>().enabled = true;   // �ٽ� ������ ���̰� �ϰ� �浹 �����ϰ� ��
        piece.GetComponent<Collider>().enabled = false; 

    }


    // �� ���� ���� ���� 
    private void UpdateDiskMaterial()
    {
        float heightPercentage = cameraObject.transform.position.y / TargetYPosition;

        Material newMaterial = null;
        if (heightPercentage >= 0.8f) { newMaterial = material_red; RotationSpeed = 10f * 2.5f; } // �ӵ� 2.5��
        else if (heightPercentage >= 0.6f) { newMaterial = material_orange; RotationSpeed = 10f * 2f; } // �ӵ� 2��
        else if (heightPercentage >= 0.4f) { newMaterial = material_blue; RotationSpeed = 10f * 1.7f; } // �ӵ� 1.7��
        else if (heightPercentage >= 0.2f) { newMaterial = material_green; RotationSpeed = 10f * 1.3f; } // �ӵ� 1.3��
        else { newMaterial = material_gray; }

        if (newMaterial != null)
        {
            foreach (GameObject piece in platformPieces)
            {
                piece.GetComponent<Renderer>().material = newMaterial;
            }
        }
    }


    // �� ȸ�� ������ 8~12�� ���� ���� �������� �����ϴ� ��ƾ
    private IEnumerator ChangeRotationDirectionRoutine()
    {
        while (!gameEnded)
        {
            if (cameraObject.transform.position.y >= 50f)
            {
                RotationDirection *= -1;  
                Debug.Log("ȸ�� ���� ����! ���� ����: " + (RotationDirection == 1 ? "�ð�" : "�ݽð�"));
            }

            float randomWaitTime = Random.Range(8f, 12f);  
            yield return new WaitForSeconds(randomWaitTime);
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
    }


    // [ ���� �߰��� �� ���� �� "center" �浹 ] --------------------------------------------------------------------------

    // �÷��̾�� ù �浹 �� ȣ��
    public void OnPlayerCollidedWithCenter()
    {
        if (!warningDisplayed)
        {
            warningDisplayed = true;
            isCollisionOngoing = true;
            // warningSound.Play();
            // WarningText.text = "5�� ���� �������� �ö���ּ���!";
            print("5�� ���� �������� �ö���ּ���!");
            // WarningText.gameObject.SetActive(true);

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

        // WarningText.gameObject.SetActive(false);
    }

    // �浹 ���� �� ī�޶�� ���� �ϰ�
    private IEnumerator ContinuousLowering()
    {
        while (isCollisionOngoing && !gameEnded)
        {
            print("���Ϳ� �������Ƿ� �ϰ��մϴ�.");
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



    // --------------------------------------------------------------------------------------------------------------
    // �� [ ���� Ŭ����/���� ] �� -----------------------------------------------------------------------------------

    IEnumerator GameClear() 
    {
        gameEnded = true;
        Debug.Log("���� Ŭ����! 5�� �� ������ �ϰ��մϴ�.");

        yield return new WaitForSeconds(5f);
        StartCoroutine(Drop(25));

        _gyrodropSceneManager.LoadMainMenuMap();
    }

    IEnumerator GameOver()
    {
        gameEnded = true; 
        Debug.Log("���� ����! 5�� �� õõ�� �ϰ��մϴ�.");

        yield return new WaitForSeconds(5f);
        StartCoroutine(Drop(5));

        _gyrodropSceneManager.LoadMainMenuMap();
    }

    private IEnumerator Drop(float speedMultiplier)
    {
        while (cameraObject.transform.position.y > 0)
        {
            MoveCameraAndDisk(cameraObject.transform.position.y - riseSpeed * speedMultiplier * Time.deltaTime);
            yield return null;
        }
        Debug.Log("�ϰ� �Ϸ�.");

    }

    // �� ��� ���� ���󺹱�(���̵���)
    private void RestoreAllPlatformPieces()
    {
        foreach (GameObject piece in platformPieces)
        {
            piece.GetComponent<Renderer>().enabled = true; // ������ ���̰� �� 
        }
    }
}
