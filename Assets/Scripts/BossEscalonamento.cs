using UnityEngine;

public class BossEscalonamento : MonoBehaviour
{
    [Header("Matemática do Boss")]
    public float vidaBase = 500f; // Vida na Onda 10
    public int moedasBase = 100;  // Moedas dadas na Onda 10

    private Enemy scriptInimigo;

    void Start()
    {
        scriptInimigo = GetComponent<Enemy>();

        if (scriptInimigo != null && GameManager.instance != null)
        {
            // Descobre o nível do boss atual. Onda 10 = Nível 1. Onda 20 = Nível 2.
            int nivelDoBoss = GameManager.instance.ondaAtual / 10;
            if (nivelDoBoss < 1) nivelDoBoss = 1; // Segurança matemática

            // 1. Escala a Vida
            scriptInimigo.vida = vidaBase * nivelDoBoss;

            // 2. Escala a Recompensa
            scriptInimigo.valorEmMoedas = moedasBase * nivelDoBoss;

            // 3. Escala o Dano (Opcional, mas deixa ele mais perigoso)
            scriptInimigo.danoPorSegundo *= nivelDoBoss;

            // 4. Efeito Visual: Faz o Boss ficar 10% maior a cada nível!
            float aumentoDeTamanho = 1f + (nivelDoBoss * 0.1f);
            transform.localScale = new Vector3(aumentoDeTamanho, aumentoDeTamanho, 1f);
        }
    }
}