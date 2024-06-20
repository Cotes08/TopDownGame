using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private GameManagerSO gameManager;
    [SerializeField] private Vector3 nextScenePosition;
    [SerializeField] private Vector2 nextSceneRotation;
    [SerializeField] private string nextSceneName;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Al colisionar con el jugador emitimos un sonido y pasamos a la siguiente escena
        if (collision.TryGetComponent(out Player _))
        {
            AudioManager.Instance.PlaySFX("Enter");
            gameManager.LoadNewScene(nextScenePosition, nextSceneRotation, nextSceneName);
        } 
    }
}
