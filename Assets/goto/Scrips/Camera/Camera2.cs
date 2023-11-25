using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera2 : MonoBehaviour
{
    GameObject player;

    [SerializeField]
    CameraChenz chenz;
    [SerializeField]
    CameraChenz chenz2;

    // Use this for initialization
    void Start()
    {
        // Playerの部分はカメラが追いかけたいオブジェクトの名前をいれる
        this.player = GameObject.Find("Player");
       

    }

    // Update is called once per frame
    void Update()
    {
        if (chenz.Check == "StartMove")
        {

            Vector3 playerPos = this.player.transform.position;
            transform.position = new Vector3(
                transform.position.x, transform.position.y, transform.position.z);
        }
        if (chenz.Check == "")
        {

            Vector3 playerPos = this.player.transform.position;
            transform.position = new Vector3(
                playerPos.x+5, transform.position.y, transform.position.z);
        }
      
        else if (chenz.Check == "Move")
        {
            //Debug.Log("");
            Vector3 playerPos = this.player.transform.position;
            transform.position = new Vector3(
                playerPos.x, transform.position.y, transform.position.z);
        }
        else if (chenz.Check == "CameraMove1")
        {
            //Debug.Log("");
            Vector3 playerPos = this.player.transform.position;
            transform.position = new Vector3(
                playerPos.x, transform.position.y, transform.position.z);
        }
    }
}