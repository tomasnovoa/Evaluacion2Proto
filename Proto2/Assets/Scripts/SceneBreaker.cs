using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class SceneBreaker : MonoBehaviour
{
     [Header("UI Elements")]
    public TextMeshProUGUI instructionText;
    public GameObject winPanel;
    public GameObject losePanel;
    public Button restartButton;

    void Start()
    {
        ResetGame();
        if (restartButton != null)
            restartButton.onClick.AddListener(ResetGame);
    }

    public void PlayerWon()
    {
        if (instructionText != null)
            instructionText.text = "¡GANASTE! Pizza > Anillo - Rompiste el cliché";
        if (winPanel != null)
            winPanel.SetActive(true);
    }

    public void PlayerLost()
    {
        if (instructionText != null)
            instructionText.text = "¡PERDISTE! Caíste en el cliché del anillo";
        if (losePanel != null)
            losePanel.SetActive(true);
    }

    public void ResetGame()
    {
        if (instructionText != null)
            instructionText.text = "Proponle matrimonio a la novia";
        if (winPanel != null)
            winPanel.SetActive(false);
        if (losePanel != null)
            losePanel.SetActive(false);
        
        // Reactivar y reposicionar objetos
        DragDrop[] draggers = FindObjectsOfType<DragDrop>();
        foreach (var dragger in draggers)
        {
            dragger.ResetPosition();
        }
    }
}