using UnityEngine;
using System.Collections;

public class LookAtPlayers : MonoBehaviour
{
    public GameObject players;

    // Use this for initialization
    void Awake()
    {
        players = GameObject.Find("StepTWO/Charator/Players");
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.LookAt(players.transform);
    }
}
