using UnityEngine;

public class BossEscalonamento : MonoBehaviour
{
    [Header("Matemática do Boss")]
    public float vidaBase = 100f;
    public int moedasBase = 100;

    // --- NOVO: Quantos Euros ele dá na onda 10 ---
    public int metaMoedasBase = 5;

    private Enemy scriptInimigo;
    private bool chefeAtivo = false;

    void Start()
    {
        scriptInimigo = GetComponent<Enemy>();
      

        if (scriptInimigo != null && GameManager.instance != null)
        {
            int nivelDoBoss = GameManager.instance.ondaAtual / 10;
            if (nivelDoBoss < 1) nivelDoBoss = 1;

            scriptInimigo.vida = vidaBase * nivelDoBoss;
            scriptInimigo.valorEmMoedas = moedasBase * nivelDoBoss;

            scriptInimigo.valorEmMetaMoedas = metaMoedasBase * nivelDoBoss;
            scriptInimigo.danoPorSegundo *= nivelDoBoss;

            float aumentoDeTamanho = 1f + (nivelDoBoss * 0.1f);
            transform.localScale = new Vector3(aumentoDeTamanho, aumentoDeTamanho, 1f);

            // --- TESTE DA BARRA ---
          

            if (BossHealthBar.instance != null)
            {
               
                BossHealthBar.instance.MostrarBoss(scriptInimigo.vida, nivelDoBoss);
                chefeAtivo = true;
            }
            else
            {
                Debug.LogError("ERRO FATAL: O Boss não achou a interface! A BossHealthBar.instance está NULL.");
            }
        }
    }

    void Update()
    {
        // Se o chefe está na tela, avisa a barra de vida quanto de HP ele tem
        if (chefeAtivo && scriptInimigo != null)
        {
            if (BossHealthBar.instance != null)
            {
                BossHealthBar.instance.AtualizarVida(scriptInimigo.vida);
            }

            // Quando a vida zerar, esconde a barra
            if (scriptInimigo.vida <= 0)
            {
                if (BossHealthBar.instance != null) BossHealthBar.instance.Esconder();
                chefeAtivo = false;
            }
        }
    }

    // Trava de segurança: Se o Boss sumir/for destruído de outra forma, esconde a barra
    void OnDestroy()
    {
        if (chefeAtivo && BossHealthBar.instance != null)
        {
            BossHealthBar.instance.Esconder();
        }
    }
}