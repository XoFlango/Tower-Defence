using UnityEngine;
using TMPro;

public class ShopManager : MonoBehaviour
{
    [Header("Referências")]
    public TowerController torre;
    public TowerHealth saudeTorre;

    [Header("Custos Iniciais")]
    public int custoDano = 5;
    public int custoVelocidade = 5;
    public int custoCura = 10;
    public int custoVidaMaxima = 15;
    public int custoMultiplicador = 25; // NOVO CUSTO (um pouco mais caro por ser muito forte)
    public int custoExplosao = 30;

    [Header("Matemática da Loja")]
    public float multiplicadorDeCusto = 1.25f;

    [Header("Textos da UI")]
    public TextMeshProUGUI textoBotaoDano;
    public TextMeshProUGUI textoBotaoVelocidade;
    public TextMeshProUGUI textoBotaoCura;
    public TextMeshProUGUI textoBotaoVidaMaxima;
    public TextMeshProUGUI textoBotaoMultiplicador; // NOVO TEXTO
    public TextMeshProUGUI textoBotaoExplosao;

    void Start()
    {
        AtualizarTextosLoja();
    }

    public void ComprarUpgradeDano()
    {
        if (GameManager.instance.GastarMoedas(custoDano))
        {
            // O dano pode subir de 1 em 1 (ou fracionado se você mudar na torre depois)
            torre.danoAtual += 1;

            // Multiplica o custo atual por 1.25 e arredonda para o inteiro mais próximo
            custoDano = Mathf.RoundToInt(custoDano * multiplicadorDeCusto);

            AtualizarTextosLoja();
        }
    }

    public void ComprarRegeneracao()
    {
        if (GameManager.instance.GastarMoedas(custoCura))
        {
            // Adiciona 0.5 de vida regenerada por segundo a cada compra
            saudeTorre.AumentarRegeneracao(0.5f);

            custoCura = Mathf.RoundToInt(custoCura * multiplicadorDeCusto);
            AtualizarTextosLoja();
        }
    }

    public void ComprarUpgradeVidaMaxima()
    {
        if (GameManager.instance.GastarMoedas(custoVidaMaxima))
        {
            // Aumenta o limite máximo em 5.0 pontos
            saudeTorre.AumentarVidaMaxima(5f);

            custoVidaMaxima = Mathf.RoundToInt(custoVidaMaxima * multiplicadorDeCusto);
            AtualizarTextosLoja();
        }
    }

    public void ComprarUpgradeExplosao()
    {
        if (GameManager.instance.GastarMoedas(custoExplosao))
        {
            // Aumenta o raio da explosão em 0.5 unidades da Unity por compra
            torre.raioDeExplosaoAtual += 0.5f;

            custoExplosao = Mathf.RoundToInt(custoExplosao * multiplicadorDeCusto);
            AtualizarTextosLoja();
        }
    }

    public void ComprarMultiplicadorDeMoedas()
    {
        if (GameManager.instance.GastarMoedas(custoMultiplicador))
        {
            // Aumenta o rendimento de moedas em 20% (0.2) a cada compra
            GameManager.instance.multiplicadorDeMoedas += 0.2f;

            custoMultiplicador = Mathf.RoundToInt(custoMultiplicador * multiplicadorDeCusto);
            AtualizarTextosLoja();
        }
    }

    void AtualizarTextosLoja()
    {
        if (textoBotaoDano != null)
            textoBotaoDano.text = "Mais Dano\n$" + custoDano;

        if (textoBotaoVelocidade != null)
            textoBotaoVelocidade.text = "Tiro Rápido\n$" + custoVelocidade;

        if (textoBotaoCura != null)
            textoBotaoCura.text = "+ Regen Vida\n$" + custoCura;

        if (textoBotaoVidaMaxima != null)
            textoBotaoVidaMaxima.text = "+ Vida Max\n$" + custoVidaMaxima;

        // Atualiza o novo texto do multiplicador
        if (textoBotaoMultiplicador != null)
            textoBotaoMultiplicador.text = "+ Ouro\n$" + custoMultiplicador;

        if(textoBotaoExplosao != null)
            textoBotaoExplosao.text = "Bala Explosiva\n$" + custoExplosao;
    }
public void ComprarUpgradeVelocidade()
    {
        if (GameManager.instance.GastarMoedas(custoVelocidade))
        {
            torre.fireRate -= 0.05f;
            if (torre.fireRate < 0.1f) torre.fireRate = 0.1f;

            custoVelocidade = Mathf.RoundToInt(custoVelocidade * multiplicadorDeCusto);
            AtualizarTextosLoja();
        }
    }
}