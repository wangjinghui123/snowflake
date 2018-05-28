using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cells : MonoBehaviour {
    public List<GameObject> cells;
    private void Start()
    {
        cells.Add(transform .GetChild (0).gameObject );
    }

}
