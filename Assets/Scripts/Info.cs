using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Info : MonoBehaviour, Interactive
{
    //Info tiene dos funcionalidades, usarse cuando el juagor interactua con objetos
    //O para dar infromacion cuando pase por una zona

    private bool talking = false;

    [SerializeField] private GameManagerSO gameManager;
    [SerializeField, TextArea(1, 5)] private string dialog;
    [SerializeField] private GameObject dialogFrame;
    [SerializeField] private TextMeshProUGUI dialogText;
    [SerializeField] private GameObject objectExists;

    public void Interact()
    {
        gameManager.ChangePlayerState(false);
        dialogFrame.SetActive(true);

        if (!talking)
        {
            AudioManager.Instance.PlaySFX("MenuIn");
            talking = true;
            dialogText.text = dialog;
        }
        else
        {
            talking = false;
            dialogText.text = "";
            dialogFrame.SetActive(false);
            gameManager.ChangePlayerState(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Con esto nos aseguramos de si la informacion esta relacionado con un objeto de la escena
        //Si este se destruye o se desactiva no mostrara la info
        if (collision.TryGetComponent(out Player _) && objectExists!=null && objectExists.activeSelf)
        {
            dialogFrame.SetActive(true);
            dialogText.text = dialog;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player _))
        {
            dialogFrame.SetActive(false);
            dialogText.text = "";
        }
    }
}
