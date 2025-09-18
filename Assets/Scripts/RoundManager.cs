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
    private int m_roundCount;

    [Header("Inimigo")]
    [SerializeField]
    private List<Enemy_SO> m_enemies;
    [SerializeField]
    private Enemy m_currentEnemy;

    public Enemy CurrentEnemy => m_currentEnemy;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        StartCoroutine(InitializeEnemy());
    }

    private IEnumerator InitializeEnemy()
    {
        // ESPERA O INIMIGO SE REGISTRAR PRIMEIRO
        yield return new WaitForEndOfFrame();

        if (m_currentEnemy != null && m_enemies.Count > 0)
        {
            Enemy_SO nextEnemy = m_enemies[Random.Range(0, m_enemies.Count)];
            m_currentEnemy.SetEnemy(nextEnemy);
            Debug.Log($"Inimigo inicializado: {nextEnemy.name}");
        }
        else
        {
            Debug.LogError("Referências de inimigo não configuradas!");
        }
    }

    // MÉTODO PARA O INIMIGO SE REGISTRAR
    public void RegisterEnemy(Enemy enemy)
    {
        m_currentEnemy = enemy;
        Debug.Log($"Inimigo registrado: {enemy.name}");

        // Configura o inimigo se a lista estiver pronta
        if (m_enemies.Count > 0)
        {
            Enemy_SO nextEnemy = m_enemies[Random.Range(0, m_enemies.Count)];
            m_currentEnemy.SetEnemy(nextEnemy);
        }
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

        // VERIFICA SE O INIMIGO EXISTE
        if (m_currentEnemy == null)
        {
            Debug.LogError("Não há inimigo atual!");
            return;
        }

        // CORREÇÃO: verifica se a vida chegou a ZERO
        if (m_currentEnemy.EnemyLife <= 0)
        {
            Debug.Log("Vitória! Inimigo derrotado!");
            //IMPLEMENTAR VITÓRIA
        }
        else
        {
            Debug.Log("Round finalizado, mas inimigo ainda vive");
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
