using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPC : MonoBehaviour, Interactive
{
    private bool talking = false;
    private int actualIndex = -1;
    private bool itemSold = false;

    [SerializeField] private GameManagerSO gameManager;
    [SerializeField, TextArea(1, 5)] private string[] dialogs;
    [SerializeField, TextArea(1, 5)] private string[] alternativeDialogs;
    [SerializeField] private float timeBetweenLetters;
    [SerializeField] private GameObject dialogFrame;
    [SerializeField] private TextMeshProUGUI dialogText;
    [SerializeField] private ItemSO hasItemToSell;

    public void Interact()
    {
        //Al interactuar con el NPC se activa el cuadro de dialogo y bloquea el movimiento del jugador
        gameManager.ChangePlayerState(false);
        dialogFrame.SetActive(true);
        if (!talking)
        {
            //Si el NPC no nos esta hablando cargara la siguiente frase
            NextFrase();
        }
        else
        {
            //Si el NPC esta hablanddo se completara la frase
            FinishFrase();
        }
    }

    private IEnumerator WriteText(string[] dialogsToWrite)
    {
        talking = true;
        dialogText.text = "";//Vaciamos el dialog para que no se solape con el anterior

        //Dividimos la frase actual en caracteres
        char[] dialogCharacters = dialogsToWrite[actualIndex].ToCharArray();

        //Ponemos sonido de dialogo
        AudioManager.Instance.PlaySound("Text");

        foreach (char character in dialogCharacters)
        {
            dialogText.text += character;
            yield return new WaitForSeconds(timeBetweenLetters);
        }
        talking = false;
        //Si ha acabado la frase paramos el sonido
        AudioManager.Instance.StopAllAudio();
    }

    private void NextFrase()
    {
        //Sobreescribimos el dialogo en el caso de que tengamos el objeto
        string[] dialogsToWrite = dialogs;
        if (hasItemToSell != null)
        {
            if (gameManager.HasItem(hasItemToSell.name) && !itemSold)
            {
                dialogsToWrite = alternativeDialogs;
            }
        }

        actualIndex++;
        if (actualIndex >= dialogsToWrite.Length)
        {
            Endialog();
        }
        else
        {
            StartCoroutine(WriteText(dialogsToWrite));
        }
    }

    private void FinishFrase()
    {
        //Si le cortamos la frase para el sonido, la corrutina y nos muestra el texto completo
        AudioManager.Instance.StopAllAudio();
        StopAllCoroutines();
        string[] dialogsToWrite = dialogs;
        if (hasItemToSell != null)
        {
            if (gameManager.HasItem(hasItemToSell.name) && !itemSold)
            {
                dialogsToWrite = alternativeDialogs;
            }
        }
        dialogText.text = dialogsToWrite[actualIndex];
        talking = false;
    }

    private void Endialog()
    {
        //Al acabar el dialogo se desactiva la interfaz y damos el control al jugador
        talking = false;
        dialogText.text = "";
        actualIndex = -1;
        dialogFrame.SetActive(false);
        gameManager.ChangePlayerState(true);

        //Solo hara el if si tenemos el item o si no lo hemos vendido ya
        if (hasItemToSell != null)
        {
            if (gameManager.HasItem(hasItemToSell.name) && !itemSold)
            {
                //Al vender el objeto lo eliminamos del inventario y nos sumamos su valor
                gameManager.RemoveItem(hasItemToSell);
                gameManager.AddCoins(hasItemToSell.value);
                itemSold = true;
                AudioManager.Instance.PlaySFX("Pickup");
            }
        }
    }
}