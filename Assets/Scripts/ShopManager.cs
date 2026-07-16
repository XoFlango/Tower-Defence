using UnityEngine;
using TMPro;

[System.Serializable]
public class BotaoUpgradeUI
{
    public TextMeshProUGUI textoDescricao;
    public TextMeshProUGUI textoValor;
    public TextMeshProUGUI textoCusto;
}

public class ShopManager : MonoBehaviour
{
    [Header("Referências da Torre")]
    public TowerController torreController;
    public TowerHealth torreHealth;

    [Header("Custos Iniciais")]
    public int custoExplosao;
    public int custoDano;
    public int custoVelocidade;
    public int custoVidaMaxima;
    public int custoRegeneracao;
    public int custoMultiplicadorMoeda;

    public float multiplicadorDeCusto = 1.5f;

    [Header("Interface dos Botões")]
    public BotaoUpgradeUI uiExplosao;
    public BotaoUpgradeUI uiDano;
    public BotaoUpgradeUI uiVelocidade;
    public BotaoUpgradeUI uiVidaMaxima;
    public BotaoUpgradeUI uiRegeneracao;
    public BotaoUpgradeUI uiMultiplicadorMoeda;

    void Start()
    {
        AtualizarTextosLoja();
    }

    // --- FUNÇÕES DE COMPRA ---

    public void ComprarUpgradeExplosao()
    {
        if (GameManager.instance.GastarMoedas(custoExplosao))
        {
            torreController.raioDeExplosaoAtual += 0.5f;
            custoExplosao = Mathf.RoundToInt(custoExplosao * multiplicadorDeCusto);
            AtualizarTextosLoja();
        }
    }

    public void ComprarUpgradeDano()
    {
        if (GameManager.instance.GastarMoedas(custoDano))
        {
            torreController.danoAtual += 1;
            custoDano = Mathf.RoundToInt(custoDano * multiplicadorDeCusto);
            AtualizarTextosLoja();
        }
    }

    public void ComprarUpgradeVelocidade()
    {
        if (torreController.fireRate > 0.15f && GameManager.instance.GastarMoedas(custoVelocidade))
        {
            torreController.fireRate -= 0.05f;
            custoVelocidade = Mathf.RoundToInt(custoVelocidade * multiplicadorDeCusto);
            AtualizarTextosLoja();
        }
    }

    public void ComprarUpgradeVidaMaxima()
    {
        if (GameManager.instance.GastarMoedas(custoVidaMaxima))
        {
            torreHealth.AumentarVidaMaxima(5f);
            custoVidaMaxima = Mathf.RoundToInt(custoVidaMaxima * multiplicadorDeCusto);
            AtualizarTextosLoja();
        }
    }

    public void ComprarRegeneracao()
    {
        if (GameManager.instance.GastarMoedas(custoRegeneracao))
        {
            torreHealth.AumentarRegeneracao(0.5f);
            custoRegeneracao = Mathf.RoundToInt(custoRegeneracao * multiplicadorDeCusto);
            AtualizarTextosLoja();
        }
    }

    public void ComprarMultiplicadorMoeda()
    {
        if (GameManager.instance.GastarMoedas(custoMultiplicadorMoeda))
        {
            GameManager.instance.multiplicadorDeMoedas += 0.2f;
            custoMultiplicadorMoeda = Mathf.RoundToInt(custoMultiplicadorMoeda * multiplicadorDeCusto);
            AtualizarTextosLoja();
        }
    }

    // --- ATUALIZAÇÃO DOS TEXTOS ---
    void AtualizarTextosLoja()
    {
        if (uiExplosao.textoDescricao != null)
        {
            uiExplosao.textoDescricao.text = "Raio Explosão";
            uiExplosao.textoValor.text = torreController.raioDeExplosaoAtual.ToString("0.0");
            uiExplosao.textoCusto.text = "$" + GameManager.instance.FormatarMoedas(custoExplosao);
        }

        if (uiDano.textoDescricao != null)
        {
            uiDano.textoDescricao.text = "Dano";
            uiDano.textoValor.text = torreController.danoAtual.ToString();
            uiDano.textoCusto.text = "$" + GameManager.instance.FormatarMoedas(custoDano);
        }

        if (uiVelocidade.textoDescricao != null)
        {
            uiVelocidade.textoDescricao.text = "Velocidade de Tiro";
            float tirosPorSegundo = 1f / torreController.fireRate;
            uiVelocidade.textoValor.text = tirosPorSegundo.ToString("0.0") + "/s";

            if (torreController.fireRate <= 0.15f) uiVelocidade.textoCusto.text = "MÁX";
            else uiVelocidade.textoCusto.text = "$" + GameManager.instance.FormatarMoedas(custoVelocidade);
        }

        if (uiVidaMaxima.textoDescricao != null)
        {
            uiVidaMaxima.textoDescricao.text = "Vida";
            uiVidaMaxima.textoValor.text = torreHealth.vidaMaxima.ToString("0");
            uiVidaMaxima.textoCusto.text = "$" + GameManager.instance.FormatarMoedas(custoVidaMaxima);
        }

        if (uiRegeneracao.textoDescricao != null)
        {
            uiRegeneracao.textoDescricao.text = "Regeneração de vida";
            uiRegeneracao.textoValor.text = torreHealth.regeneracaoPorSegundo.ToString("0.0") + "/s";
            uiRegeneracao.textoCusto.text = "$" + GameManager.instance.FormatarMoedas(custoRegeneracao);
        }

        if (uiMultiplicadorMoeda.textoDescricao != null)
        {
            uiMultiplicadorMoeda.textoDescricao.text = "Bônus de Moedas";
            uiMultiplicadorMoeda.textoValor.text = GameManager.instance.multiplicadorDeMoedas.ToString("0.0") + "x";
            uiMultiplicadorMoeda.textoCusto.text = "$" + GameManager.instance.FormatarMoedas(custoMultiplicadorMoeda);
        }
    }
}