using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircusGameManager : MonoBehaviour
{
    [Header("Levels")]
    [SerializeField] float gamePlayTime;           // 전체 게임 진행 시간 120초
    [SerializeField] float currentAttackCycle;     // 현재의 동물 공격 주기
    [SerializeField] float currentBallSpeed;       // 현재의 공 속도

    [Header("Managers")]
    [SerializeField] CircusUIManager _circusUIManager;
    [SerializeField] CircusSoundManager _circusSoundManager;
    [SerializeField] CircusSceneManager _circusSceneManager;
}
