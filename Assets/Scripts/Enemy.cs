using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Status")]
    public int vida = 1; // Quantos tiros ele aguenta
    public int valorEmMoedas = 1; // Quantas moedas ele dá ao morrer
    [Header("Ataque")]
    public float danoPorSegundo = 10f; // Quanto de vida ele tira da torre por segundo encostado

    public void ReceberDano(int dano)
    {
        vida -= dano;

        if (vida <= 0)
        {
            Morrer();
        }
    }

    void OnCollisionStay2D(Collision2D colisao)
    {
        // Verifica se o objeto em que o inimigo está encostado tem a Tag "Torre"
        if (colisao.gameObject.CompareTag("Torre"))
        {
            // Tenta pegar o script de vida da torre
            TowerHealth saudeTorre = colisao.gameObject.GetComponent<TowerHealth>();

            if (saudeTorre != null)
            {
                // Causa um dano contínuo e suave baseado no tempo (Time.deltaTime)
                saudeTorre.ReceberDano(danoPorSegundo * Time.deltaTime);
            }
        }
    }

    void Morrer()
    {
        // Avisa o GameManager para adicionar os pontos
        if (GameManager.instance != null)
        {
            GameManager.instance.AdicionarMoedas(valorEmMoedas);
        }

        // Destrói o quadrado
        Destroy(gameObject);
    }
}