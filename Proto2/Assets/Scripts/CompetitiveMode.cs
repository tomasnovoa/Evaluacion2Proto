using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
public class CompetitiveMode : MonoBehaviour
{
    [Header("UI Competitivo")]
    public InputField[] playerInputs;
    public Button[] voteButtons;
    public TextMeshProUGUI votingText;
    public GameObject votingPanel;

    private List<string> playerAnswers = new List<string>();
    
    private int currentVoter = 0;
    private int[] votes;

    public void StartVoting(List<string> answers)
    {
        playerAnswers = answers;
        votes = new int[answers.Count];
        currentVoter = 0;

        votingPanel.SetActive(true);
        votingText.text = $"Jugador {currentVoter + 1}, vota por la mejor alternativa:";

        // Mostrar respuestas para votar
        for (int i = 0; i < voteButtons.Length; i++)
        {
            if (i < answers.Count)
            {
                voteButtons[i].gameObject.SetActive(true);
                voteButtons[i].GetComponentInChildren<Text>().text = answers[i];
                int index = i;
                voteButtons[i].onClick.RemoveAllListeners();
                voteButtons[i].onClick.AddListener(() => CastVote(index));
            }
            else
            {
                voteButtons[i].gameObject.SetActive(false);
            }
        }
    }

    void CastVote(int answerIndex)
    {
        votes[answerIndex]++;
        currentVoter++;

        if (currentVoter < playerAnswers.Count)
        {
            votingText.text = $"Jugador {currentVoter + 1}, vota por la mejor alternativa:";
        }
        else
        {
            EndVoting();
        }
    }

    void EndVoting()
    {
        // Encontrar respuesta ganadora
        int maxVotes = 0;
        int winningIndex = 0;

        for (int i = 0; i < votes.Length; i++)
        {
            if (votes[i] > maxVotes)
            {
                maxVotes = votes[i];
                winningIndex = i;
            }
        }

        votingText.text = $"¡Respuesta ganadora!\n\n\"{playerAnswers[winningIndex]}\"\n\nVotos: {maxVotes}";
        
        // Dar puntos extra al ganador
        // (implementar según tu sistema de puntuación)
    }
}

