using UnityEngine;

// Isso garante que a Unity adicione um AudioSource automaticamente ao objeto
[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    [Header("Playlist (Músicas de Fundo)")]
    [Tooltip("Arraste suas músicas para cá.")]
    public AudioClip[] musicas;

    private AudioSource audioSource;
    private int indiceUltimaMusica = -1;

    void Awake()
    {
        // Padrão Singleton para manter a música tocando mesmo se mudar de cena
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        TocarProximaMusica();
    }

    void Update()
    {
        // Verifica continuamente se a música atual terminou de tocar
        if (!audioSource.isPlaying && musicas.Length > 0)
        {
            TocarProximaMusica();
        }
    }

    void TocarProximaMusica()
    {
        if (musicas.Length == 0) return;

        int indiceSorteado = Random.Range(0, musicas.Length);

        // Se houver mais de uma música, garante que não vai repetir a anterior
        if (musicas.Length > 1)
        {
            while (indiceSorteado == indiceUltimaMusica)
            {
                indiceSorteado = Random.Range(0, musicas.Length);
            }
        }

        // Atualiza a memória, coloca o clipe no rádio e dá o play
        indiceUltimaMusica = indiceSorteado;
        audioSource.clip = musicas[indiceSorteado];
        audioSource.Play();
    }
}