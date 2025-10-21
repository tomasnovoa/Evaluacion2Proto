using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
public class ClichéDetectorManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI sceneDescriptionText;
    public TMP_InputField playerInputField;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;
    public Button submitButton;
    public GameObject resultPanel;
    public TextMeshProUGUI resultText;
    public Button nextButton;

    [Header("Configuración del Juego")]
    public int totalRounds = 5;
    public float timePerRound = 60f;

    private int currentRound = 0;
    private int score = 0;
    private float currentTime;
    private bool gameActive = false;

    // Base de datos de clichés y alternativas
    private List<ClichéScene> clichéScenes = new List<ClichéScene>();
    

    void Start()
    {
        InitializeScenes();
        SetupUI();
        StartGame();
    }

    void InitializeScenes()
    {
        // Escenas cliché con sus posibles alternativas
        clichéScenes.Add(new ClichéScene(
            "El héroe salva a la chica en el último segundo del peligro inminente",
            new List<string> { "héroe salva", "último segundo", "chica en peligro" },
            new List<string> { "la chica se salva sola", "el héroe llega tarde", "era una trampa" }
        ));

        clichéScenes.Add(new ClichéScene(
            "El villano explica todo su plan antes de intentar matar al héroe",
            new List<string> { "villano explica plan", "antes de matar", "monólogo" },
            new List<string> { "el villano actúa directamente", "el héroe ya sabía el plan", "es una distracción" }
        ));

        clichéScenes.Add(new ClichéScene(
            "Los padres no entienden al adolescente rebelde con el pelo teñido",
            new List<string> { "padres no entienden", "adolescente rebelde", "pelo teñido" },
            new List<string> { "los padres fueron rebeldes también", "el adolescente es conservador", "todos se llevan bien" }
        ));

        clichéScenes.Add(new ClichéScene(
            "La comedia del amigo torpe que tropieza con todo",
            new List<string> { "amigo torpe", "tropieza con todo", "comedia física" },
            new List<string> { "el torpe es el más inteligente", "todos son torpes", "nadie es torpe" }
        ));

        clichéScenes.Add(new ClichéScene(
            "La chica elige entre el chico bueno y el chico malo",
            new List<string> { "elige entre", "chico bueno", "chico malo" },
            new List<string> { "elige a una chica", "prefiere estar sola", "los dos son malos" }
        ));
    }

    void SetupUI()
    {
        submitButton.onClick.AddListener(SubmitAnswer);
        nextButton.onClick.AddListener(NextRound);
        resultPanel.SetActive(false);
    }

    void StartGame()
    {
        currentRound = 0;
        score = 0;
        gameActive = true;
        LoadNextScene();
    }

    void Update()
    {
        if (gameActive)
        {
            UpdateTimer();
        }
    }

    void UpdateTimer()
    {
        currentTime -= Time.deltaTime;
        timerText.text = $"Tiempo: {Mathf.CeilToInt(currentTime)}s";

        if (currentTime <= 0)
        {
            TimeUp();
        }
    }

    void LoadNextScene()
    {
        if (currentRound >= totalRounds)
        {
            EndGame();
            return;
        }

        currentRound++;
        currentTime = timePerRound;
        playerInputField.text = "";
        resultPanel.SetActive(false);

        // Mostrar escena cliché actual
        ClichéScene currentScene = clichéScenes[currentRound - 1];
        sceneDescriptionText.text = $"Escena {currentRound}/{totalRounds}:\n\"{currentScene.description}\"";
        
        scoreText.text = $"Puntuación: {score}";
    }

    void SubmitAnswer()
    {
        if (!gameActive) return;

        string playerAnswer = playerInputField.text.Trim();
        if (string.IsNullOrEmpty(playerAnswer))
        {
            return;
        }

        ClichéScene currentScene = clichéScenes[currentRound - 1];
        int roundScore = CalculateScore(playerAnswer, currentScene);

        score += roundScore;

        // Mostrar resultado
        resultText.text = $"¡Respuesta enviada!\nPuntos: +{roundScore}\n\nAlternativas sugeridas:\n";
        foreach (string alternative in currentScene.suggestedAlternatives)
        {
            resultText.text += $"• {alternative}\n";
        }

        resultPanel.SetActive(true);
        gameActive = false;
    }

    int CalculateScore(string playerAnswer, ClichéScene scene)
    {
        int points = 0;

        // Puntos por longitud (creatividad)
        points += Mathf.Min(playerAnswer.Length / 5, 10);

        // Puntos por evitar palabras cliché
        foreach (string clicheWord in scene.clichéKeywords)
        {
            if (!playerAnswer.ToLower().Contains(clicheWord.ToLower()))
            {
                points += 5;
            }
        }

        // Puntos extra por ironía/sátira
        if (playerAnswer.ToLower().Contains("ironía") || playerAnswer.ToLower().Contains("sátira") ||
            playerAnswer.ToLower().Contains("parodia") || playerAnswer.ToLower().Contains("absurdo"))
        {
            points += 10;
        }

        return Mathf.Min(points, 50); // Máximo 50 puntos por ronda
    }

    void TimeUp()
    {
        gameActive = false;
        resultText.text = "¡Tiempo agotado!\nPuntos: +0";
        resultPanel.SetActive(true);
    }

    void NextRound()
    {
        gameActive = true;
        LoadNextScene();
    }

    void EndGame()
    {
        sceneDescriptionText.text = "¡Juego Completado!";
        playerInputField.gameObject.SetActive(false);
        submitButton.gameObject.SetActive(false);
        timerText.text = "";

        string finalMessage = $"Puntuación Final: {score}/{(totalRounds * 50)}\n\n";
        if (score >= 200) finalMessage += "¡Eres un genio anticliché! 🎉";
        else if (score >= 150) finalMessage += "¡Muy buen trabajo! 👏";
        else if (score >= 100) finalMessage += "¡Bien hecho! 👍";
        else finalMessage += "Sigue practicando 💪";

        resultText.text = finalMessage;
        resultPanel.SetActive(true);
        nextButton.gameObject.SetActive(false);
    }
}

[System.Serializable]
public class ClichéScene
{
    public string description;
    public List<string> clichéKeywords;
    public List<string> suggestedAlternatives;

    public ClichéScene(string desc, List<string> keywords, List<string> alternatives)
    {
        description = desc;
        clichéKeywords = keywords;
        suggestedAlternatives = alternatives;
    }
}
