using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Status")]
    public float vida = 1f; // Quantos tiros ele aguenta
    public int valorEmMoedas = 1; // Quantas moedas ele dá ao morrer
    private bool jaMorreu = false;
    [Header("Ataque")]
    public float danoPorSegundo = 10f; // Quanto de vida ele tira da torre por segundo encostado

    public void ReceberDano(float danoRecebido)
    {
        // Se ele já estiver morto, ignora qualquer bala nova que bater nele
        if (jaMorreu) return;

        vida -= danoRecebido;

        if (vida <= 0)
        {
            jaMorreu = true; // Tranca o inimigo. Ele não pode mais receber dano.

            GameManager.instance.RegistrarMorteInimigo();

            // Recomendo colocar aqui a sua lógica de dar moedas ao jogador também
            GameManager.instance.AdicionarMoedas(valorEmMoedas);
            Destroy(gameObject);
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

}