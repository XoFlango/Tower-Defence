using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Status")]
    public int vida = 1; // Quantos tiros ele aguenta
    public int valorEmMoedas = 1; // Quantas moedas ele dá ao morrer

    public void ReceberDano(int dano)
    {
        vida -= dano;

        if (vida <= 0)
        {
            Morrer();
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