using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    [Header("Configurações de Tiro")]
    public GameObject projetilPrefab;
    public float tempoEntreTiros = 2f;

    private Transform tower;
    private TowerController scriptDaTorre;
    private float timer;

    void Start()
    {
        GameObject towerObject = GameObject.Find("Torre");
        if (towerObject != null)
        {
            tower = towerObject.transform;
            scriptDaTorre = towerObject.GetComponent<TowerController>();
        }
    }

    void Update()
    {
        if (tower == null || scriptDaTorre == null) return;

        float distancia = Vector2.Distance(transform.position, tower.position);

        // Novamente, troque 'alcanceDeVisao' para a variável correta da sua torre.
        // Colocamos + 0.1f como uma margem de segurança para garantir que ele não fique bugando no limite.
        float alcanceDeTiro = scriptDaTorre.attackRange + 0.1f;

        // Se ele chegou no limite...
        if (distancia <= alcanceDeTiro)
        {
            timer += Time.deltaTime;

            // ... começa a atirar!
            if (timer >= tempoEntreTiros)
            {
                Atirar();
                timer = 0f;
            }
        }
    }

    void Atirar()
    {
        if (projetilPrefab != null)
        {
            Vector2 direcao = (tower.position - transform.position).normalized;
            float angulo = Mathf.Atan2(direcao.y, direcao.x) * Mathf.Rad2Deg;
            Quaternion rotacaoDoTiro = Quaternion.Euler(0, 0, angulo);

            Instantiate(projetilPrefab, transform.position, rotacaoDoTiro);
        }
    }
}