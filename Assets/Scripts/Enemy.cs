using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Status")]
    public float vida = 1f; // Quantos tiros ele aguenta
    public int valorEmMoedas = 1; // Quantas moedas ele dá ao morrer
    public int valorEmMetaMoedas = 0;
    private bool jaMorreu = false;

    [Header("Efeitos Visuais")]
    public GameObject efeitoMortePrefab;

    [Header("Ataque")]
    public float danoPorSegundo = 10f; // Quanto de vida ele tira da torre por segundo encostado

    public void ReceberDano(float danoRecebido)
    {
        if (jaMorreu) return;

        vida -= danoRecebido;

        if (vida <= 0)
        {
            jaMorreu = true;

            // Avisa o gerente para avançar a onda e dar o dinheiro
            GameManager.instance.RegistrarMorteInimigo();
            GameManager.instance.AdicionarMoedas(valorEmMoedas);
            if (valorEmMetaMoedas > 0)
            {
                GameManager.instance.AdicionarMetaMoedas(valorEmMetaMoedas);
            }

            // --- NOVA LÓGICA DE EXPLOSÃO ---
            if (efeitoMortePrefab != null)
            {
                // Garante que o efeito nasça no Z=0 para não ficar invisível atrás da câmera
                Vector3 posicaoCorrecaoZ = new Vector3(transform.position.x, transform.position.y, 0f);
                Instantiate(efeitoMortePrefab, posicaoCorrecaoZ, Quaternion.identity);
            }

            // --- NOVO: Só entrega se tiver algum valor configurado ---
            
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