using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    [SerializeField] private GameManagerSO gameManager;
    [SerializeField] private ItemSO itemSO;
    [SerializeField] private GameObject fence;
    [SerializeField] private GameObject endGameFrame;
    [SerializeField] private GameObject blockinObject;

    
    void Start()
    {
        //Comprobamos si tiene el objeto para acabar el juego
        if (gameManager.HasItem(itemSO.name))
        {
            //En el caso de que sea asi desactivamos el sprite de la valla y desctivamos el objeto que bloqueaba el camino
            fence.GetComponent<SpriteRenderer>().enabled = false;
            blockinObject.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D _)
    {
        StartCoroutine(EndGameCorrutine());
    }

    //Al salir del GameObject mostraremos un mensaje de fin de juego
    private IEnumerator EndGameCorrutine() {
        endGameFrame.SetActive(true);
        gameManager.ChangePlayerState(false);
        yield return new WaitForSeconds(5);
        Application.Quit();
    }


}
