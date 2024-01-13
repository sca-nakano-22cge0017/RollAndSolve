using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCatch : MonoBehaviour
{
    HPController hpController;

    void Start()
    {
        hpController = GameObject.FindObjectOfType<HPController>();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            if(this.gameObject.tag == "HealHeart" && hpController.Hp < hpController.HpLimit)
            {
                hpController.Hp++;
            }

            if(this.gameObject.tag == "EmptyHeart")
            {
                hpController.HpLimit++;
            }

            if (this.gameObject.tag == "SecretCoin")
            {
                
            }

            Destroy(this.gameObject);
        }
    }
}
