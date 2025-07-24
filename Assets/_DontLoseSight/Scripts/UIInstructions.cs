using UnityEngine;
using TMPro;
using System.Collections;

public class UIInstructions : MonoBehaviour
{
    [Header("Références UI")]
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text subtitleText;
    [SerializeField] private TMP_Text instructionsText;

    [Header("Durée d'affichage en secondes")]
    [SerializeField] private float displayDuration = 3f;

    [Header("Contenu")]
    [SerializeField] private string titleMessage = "🎯 Attrapez la balle…";
    [SerializeField] private string subtitleMessage = "👀 …et ne la perdez jamais de vue !";
    [TextArea(3,10)]
    [SerializeField] private string instructionsMessage =
        "🕹️ Commandes :\n" +
        "Espace = Monter\n" +
        "Shift = Descendre\n" +
        "Z/Q/S/D = Se déplacer\n" +
        "Souris = Orienter la caméra";

    private void OnEnable()
    {
        // Vérifie les références
        if (titleText == null || subtitleText == null || instructionsText == null)
        {
            Debug.LogWarning("⚠️ Assigne bien les trois TMP_Text dans l'inspecteur !");
            return;
        }

        // Met à jour les textes
        titleText.text = titleMessage;
        subtitleText.text = subtitleMessage;
        instructionsText.text = instructionsMessage;

        // Lance la coroutine pour masquer après un délai
        StartCoroutine(HideAfterDelay());
    }

    private IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(displayDuration);
        gameObject.SetActive(false); // désactive le panel complet
    }
}