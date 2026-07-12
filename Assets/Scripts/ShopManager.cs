using UnityEngine;
using TMPro;

public class ShopManager : MonoBehaviour
{
    [Header("Referências")]
    public TowerController torre;

    [Header("Custos Iniciais")]
    public int custoDano = 5;
    public int custoVelocidade = 5;

    [Header("Textos da UI")]
    public TextMeshProUGUI textoBotaoDano;
    public TextMeshProUGUI textoBotaoVelocidade;

    void Start()
    {
        AtualizarTextosLoja();
    }

    // Função que será chamada ao clicar no Botão de Dano
    public void ComprarUpgradeDano()
    {
        if (GameManager.instance.GastarMoedas(custoDano))
        {
            torre.danoAtual += 1; // Aumenta o dano da torre
            custoDano += 5;       // Aumenta o preço para a próxima compra
            AtualizarTextosLoja();
        }
    }

    // Função que será chamada ao clicar no Botão de Velocidade
    public void ComprarUpgradeVelocidade()
    {
        if (GameManager.instance.GastarMoedas(custoVelocidade))
        {
            torre.fireRate += 0.5f; // Atira meia vez a mais por segundo
            custoVelocidade += 5;          // Aumenta o preço
            AtualizarTextosLoja();
        }
    }

    void AtualizarTextosLoja()
    {
        if (textoBotaoDano != null)
            textoBotaoDano.text = "Mais Dano\n$" + custoDano;

        if (textoBotaoVelocidade != null)
            textoBotaoVelocidade.text = "Tiro Rápido\n$" + custoVelocidade;
    }
}