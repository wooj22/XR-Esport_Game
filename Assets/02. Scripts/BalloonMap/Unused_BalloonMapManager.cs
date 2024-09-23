using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unused_BalloonMapManager : MonoBehaviour
{

    // ★ [ 풍선 생성 및 랜덤배치 ]
    // 
    public GameObject[] spawnPrefabs;   // 생성할 6가지 풍선 프리팹
    public GameObject[] centerObjects;  // 중심점 오브젝트 배열 (right, left, back, front, down)

    private float spawnRadiusY = 70f;   // Y축 랜덤 거리 범위
    private float spawnRadiusZ = 350f;  // Z축 랜덤 거리 범위 (right, left, down)
    private float spawnRadiusX = 250f;  // X축 랜덤 거리 범위 (back, front, down)

    #region down 중심점의 거리 범위 설정 (Spout 카메라 확인 필요)
    // private float downSpawnRadiusZ = 350f;  // down 중심점의 Z축 랜덤 거리 범위
    // private float downSpawnRadiusX = 250f;  // down 중심점의 X축 랜덤 거리 범위
    #endregion

    private int objectsPerCenter = 40;  // 각 중심점에서 생성할 오브젝트 개수
    private float minDistance = 20f;    // 오브젝트 간 최소 거리


    // ------------------------------------------------------------------------------------------------


    void Start()
    {
        SpawnObjects();
    }


    // ★ [ 각 중심점에 맞게 오브젝트 생성 ] ★
    // 
    // - spawnedPositions : 생성된 오브젝트들의 위치 리스트
    // - 각 중심점에서 objectsPerCenter 수 만큼 오브젝트 생성
    // 
    void SpawnObjects()
    {
        foreach (GameObject centerObject in centerObjects)
        {
            List<Vector3> spawnedPositions = new List<Vector3>(); 

            for (int i = 0; i < objectsPerCenter; i++)
            {
                SpawnAtCenter(centerObject, spawnedPositions);
            }
        }
    }



    // ★ [ 주어진 중심점에서 하나의 오브젝트 생성 ]
    // 
    // 최대 100번 시도 => ( 유효한 위치에 오브젝트 생성 , 부모로 중심점 설정 )
    // - selectedPrefab : 랜덤하게 생성할 프리팹 선택
    // - validPosition : 생성 위치가 유효한지 여부
    // - attempts : 위치 생성 시도 횟수
    // 
    void SpawnAtCenter(GameObject centerObject, List<Vector3> spawnedPositions)
    {
        GameObject selectedPrefab = spawnPrefabs[Random.Range(0, spawnPrefabs.Length)];
        Vector3 spawnPosition;
        bool validPosition = false;
        int attempts = 0;

        while (!validPosition && attempts < 100)
        {
            attempts++;

            spawnPosition = GetRandomPosition(centerObject);
            validPosition = IsPositionValid(spawnPosition, spawnedPositions);

            if (validPosition)
            {
                GameObject newObject = Instantiate(selectedPrefab, spawnPosition, Quaternion.identity);
                newObject.transform.parent = centerObject.transform;

                spawnedPositions.Add(spawnPosition);
            }
        }
    }


    // ★ [ 중심점에 따라 랜덤 좌표 반환 ]
    // 
    // - randomY : 모든 중심점에서 사용
    // 
    // - 기본적으로 중심점의 위치를 반환 (오류 방지용)
    // 
    Vector3 GetRandomPosition(GameObject centerObject)
    {
        float randomY = Random.Range(-spawnRadiusY, spawnRadiusY);

        float randomX = 0f, randomZ = 0f;

        if (centerObject.name == "right" || centerObject.name == "left")      // X축 고정, Y와 Z축 랜덤
        {
            randomZ = Random.Range(-spawnRadiusZ, spawnRadiusZ);
            return new Vector3(centerObject.transform.position.x,
                               centerObject.transform.position.y + randomY,
                               centerObject.transform.position.z + randomZ);
        }
        else if (centerObject.name == "back" || centerObject.name == "front")  // Z축 고정, X와 Y축 랜덤
        {
            randomX = Random.Range(-spawnRadiusX, spawnRadiusX);
            return new Vector3(centerObject.transform.position.x + randomX,
                               centerObject.transform.position.y + randomY,
                               centerObject.transform.position.z);
        }
        else if (centerObject.name == "down")                                  // Y축 고정, Z와 X축 랜덤
        {
            randomZ = Random.Range(-spawnRadiusZ, spawnRadiusZ);
            randomX = Random.Range(-spawnRadiusX, spawnRadiusX);
            return new Vector3(centerObject.transform.position.x + randomX,
                               centerObject.transform.position.y,
                               centerObject.transform.position.z + randomZ);
        }

        return centerObject.transform.position; 
    }


    // ★ [ 최소 거리 만족하는지 확인 ] 
    // 
    bool IsPositionValid(Vector3 position, List<Vector3> spawnedPositions)
    {
        foreach (Vector3 pos in spawnedPositions)
        {
            if (Vector3.Distance(position, pos) < minDistance)
            {
                return false;
            }
        }
        return true; 
    }

}
