using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(Player, new Vector3(0, 0, 0), Quaternion.identity);
    }

}
