using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class SceneBreaker : MonoBehaviour
{
    [Header("Configuración del Juego")]
    public TextMeshProUGUI instructionText;
    public GameObject winPanel;
    public GameObject losePanel;
    public Button restartButton;

    void Start()
    {
        ResetGame();
        restartButton.onClick.AddListener(ResetGame);
    }

    public void CheckWinCondition(bool isPizza)
    {
        if (isPizza)
        {
            WinGame();
        }
        else
        {
            LoseGame();
        }
    }

    public void WinGame()
    {
        instructionText.text = "¡GANASTE! Rompiste el cliché con pizza 🍕";
        winPanel.SetActive(true);
    }

    public void LoseGame()
    {
        instructionText.text = "¡PERDISTE! Caíste en el cliché del anillo 💍";
        losePanel.SetActive(true);
    }

    void ResetGame()
    {
        instructionText.text = "Arrastra la PIZZA a la novia para romper el cliché";
        winPanel.SetActive(false);
        losePanel.SetActive(false);

        // Reactivar todos los objetos arrastrables
        DragDrop[] draggables = FindObjectsOfType<DragDrop>();
        foreach (var draggable in draggables)
        {
            draggable.ResetToStart();
        }
    }
}