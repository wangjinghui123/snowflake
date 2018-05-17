using UnityEngine;

public class RemoveCollider : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    [ContextMenu("ReNameChild")]
    public void ReNameChild()
    {
        int len = transform.childCount;
        for(int i = 0; i < len; i++)
        {
            transform.GetChild(i).name = "waypoint" + i;
        }
    }

    [ContextMenu("StartEditRemove")]
    public void RemoveColliderFunc(Transform t)
    {
        Collider[] all = transform.GetComponentsInChildren<Collider>();
        for (int i = 0; i < all.Length; i++)
        {
            DestroyImmediate(all[i]);
        }
    }
}
