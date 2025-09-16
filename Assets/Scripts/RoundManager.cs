using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoundManager : MonoBehaviour
{
    public static RoundManager instance;

    private GameManager m_gameManager;

    [SerializeField]
    private int m_roundCount; //CONTAGEM DE ROUNDS, SE NECESS�RIO

    [Header("Inimigo")]
    [SerializeField]
    private List<Enemy_SO> m_enemies; //LISTA DE POSS�VEIS INIMIGOS
    [SerializeField]
    private Enemy m_currentEnemy; //VARI�VEL PRIVADA DO INIMIGO ATUAL

    public Enemy CurrentEnemy => m_currentEnemy; //VARI�VEL P�BLICA DO INIMIGO ATUAL

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Enemy_SO nextEnemy = m_enemies[Random.Range(0, m_enemies.Count)];
        m_currentEnemy.SetEnemy(nextEnemy);
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.F))
        {
            FinishRound();
        }
#endif
    }

    public void FinishRound()
    {
        AddRound();
        int currentGoal = (int)Mathf.Floor(m_currentEnemy.EnemyInfo.Vida);
        if (m_currentEnemy.EnemyLife >= currentGoal)
        {
            //IMPLEMENTAR VIT�RIA
        }
    }

    private void AddRound()
    {
        m_roundCount++;
    }

    public void ResetRounds()
    {
        m_roundCount = 0;
    }
}
