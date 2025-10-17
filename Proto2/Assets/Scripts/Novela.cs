using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Novela : MonoBehaviour
{
     public TextMeshProUGUI sceneText;
    public Button[] optionButtons;
    public Slider originalityBar;

    private float originality = 0.5f;

    void Start()
    {
        LoadScene(0);
    }

    public void LoadScene(int sceneIndex)
    {
        // Ejemplo de escena cliché
        sceneText.text = "El personaje se acerca y dice: 'Siempre te he amado...'";

        // Opciones anticliché
        optionButtons[0].GetComponentInChildren<TextMeshProUGUI>().text = "Yo también te amo";
        optionButtons[1].GetComponentInChildren<TextMeshProUGUI>().text = "Te confundí con otra persona";

        // Asignar eventos
        optionButtons[0].onClick.RemoveAllListeners();
        optionButtons[1].onClick.RemoveAllListeners();

        optionButtons[0].onClick.AddListener(() => ChooseOption(0.1f)); // Cliché
        optionButtons[1].onClick.AddListener(() => ChooseOption(0.3f)); // Anticliché
    }

    public void ChooseOption(float originalityChange)
    {
        originality += originalityChange;
        originality = Mathf.Clamp(originality, 0, 1);
        originalityBar.value = originality;

        // Cargar siguiente escena o final
        if (originality >= 1f)
        {
            sceneText.text = "¡Final original alcanzado!";
        }
    }
}
