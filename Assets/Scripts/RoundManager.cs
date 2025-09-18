using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    [Header("Configura��o de Vit�ria")]
    [SerializeField] private string victorySceneName = "TelaDeVitoria"; // Nome exato da cena


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
            Debug.LogError("Refer�ncias de inimigo n�o configuradas!");
        }
    }

    // M�TODO PARA O INIMIGO SE REGISTRAR
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
            Debug.LogError("N�o h� inimigo atual!");
            return;
        }

        // VERIFICA SE O INIMIGO MORREU
        if (m_currentEnemy.EnemyLife <= 0)
        {
            Debug.Log("Vit�ria! Inimigo derrotado!");
            LoadVictoryScene(); // ? CARREGA A CENA DE VIT�RIA
        }
        else
        {
            Debug.Log("Round finalizado, mas inimigo ainda vive");
        }
    }

    private void LoadVictoryScene()
    {
        // Verifica se o nome da cena est� configurado
        if (string.IsNullOrEmpty(victorySceneName))
        {
            Debug.LogError("Nome da cena de vit�ria n�o configurado! Verifique o Inspector.");
            return;
        }

        // Verifica se a cena existe
        if (SceneUtility.GetBuildIndexByScenePath(victorySceneName) < 0)
        {
            Debug.LogError($"Cena '{victorySceneName}' n�o encontrada no Build Settings!");
            return;
        }

        Debug.Log($"Carregando cena de vit�ria: {victorySceneName}");

        // Carrega a cena ap�s um pequeno delay para anima��es
        StartCoroutine(LoadVictorySceneWithDelay(1.5f));
    }

    private IEnumerator LoadVictorySceneWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(victorySceneName);
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
