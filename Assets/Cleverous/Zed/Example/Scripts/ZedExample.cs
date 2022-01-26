using UnityEngine;
using UnityEngine.UI;

public class ZedExample : MonoBehaviour
{
    public Animator Zed1;
    public Animator Zed2;
    public Text LabelText;

    private int _index;
    private readonly string[] _names =
    {
        "00 Idle",
        "01 Forward",
        "02 Forward 45 L",
        "03 Forward 45 R",
        "04 Back",
        "05 Back 45 L",
        "06 Back 45 R",
        "07 Strafe L",
        "08 Strafe R",
        "09 Ability 1",
        "10 Ability 2",
        "11 Death",
        "12 Death Big",
        "13 Hit Reaction 1",
        "14 Hit Reaction 2",
        "15 Jump Start",
        "16 Jump Loop",
        "17 Jump Land",
        "18 Melee 1",
        "19 Melee 2",
    };

    public void Awake()
    {
        LabelText.text = _names[_index];
    }

    public void Next()
    {
        _index++;
        if (_index < 0) _index = _names.Length - 1;
        if (_index > _names.Length - 1) _index = 0;

        Zed1.SetInteger("Index", _index);
        Zed1.SetTrigger("Change");

        Zed2.SetInteger("Index", _index);
        Zed2.SetTrigger("Change");

        LabelText.text = _names[_index];
    }

    public void Previous()
    {
        _index--;
        if (_index < 0) _index = _names.Length - 1;
        if (_index > _names.Length - 1) _index = 0;

        Zed1.SetInteger("Index", _index);
        Zed1.SetTrigger("Change");

        Zed2.SetInteger("Index", _index);
        Zed2.SetTrigger("Change");

        LabelText.text = _names[_index];
    }

    public void Again()
    {
        Zed1.SetTrigger("Change");
        Zed2.SetTrigger("Change");
    }
}