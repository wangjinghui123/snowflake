using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class RunkingUser : MonoBehaviour
{
    public Image headPortriat
    {
        get
        {
            return _headPortriat;
        }
        set
        {
            _headPortriat = value;
        }
    }
    public Text rankName
    {
        get
        {
            return _name;
        }
        set
        {
            _name = value;
        }
    }
    public Text step
    {
        get
        {
            return _step;
        }
        set
        {
            _step = value;
        }
    }
    public int runkNumber
    {
        get
        {
            return _rankNum;
        }
        set
        {
            _rankNum = value;
        }
    }

    [SerializeField]
    private Image _headPortriat;
    [SerializeField]
    private Text _name;
    [SerializeField]
    private Text _step;
    private int _rankNum;

}
