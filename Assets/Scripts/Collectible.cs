using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] AudioClip pickUpSound;
    [SerializeField] float pickUpVolume;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        AudioSource.PlayClipAtPoint(pickUpSound, Camera.main.transform.position, pickUpVolume);
        FindObjectOfType<GameSession>().AddToScore(1);
        Destroy(gameObject);
    }

}
