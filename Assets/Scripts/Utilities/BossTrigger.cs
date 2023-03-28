using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    public delegate void BossFightTriggered();
    public static event BossFightTriggered onBossFightTriggered;

    [SerializeField] private GameObject _boss;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<PlayerMarker>() != null)
        {

            //_boss.SetActive(true);
            onBossFightTriggered();
            gameObject.GetComponent<BoxCollider2D>().gameObject.SetActive(false);
        }
    }
}
