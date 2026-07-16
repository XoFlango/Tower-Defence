using UnityEngine;

public class BossEscalonamento : MonoBehaviour
{
    [Header("Matemática do Boss")]
    public float vidaBase = 500f;
    public int moedasBase = 100;

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
            scriptInimigo.danoPorSegundo *= nivelDoBoss;

            float aumentoDeTamanho = 1f + (nivelDoBoss * 0.1f);
            transform.localScale = new Vector3(aumentoDeTamanho, aumentoDeTamanho, 1f);

            // --- NOVO: Manda a UI ligar a barra do Boss ---
            if (BossHealthBar.instance != null)
            {
                BossHealthBar.instance.MostrarBoss(scriptInimigo.vida, nivelDoBoss);
                chefeAtivo = true;
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