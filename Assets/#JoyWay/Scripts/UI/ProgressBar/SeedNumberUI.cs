using TMPro;
using UnityEngine;

public class SeedNumberUI : MonoBehaviour {
    public static SeedNumberUI instance;
    public TextMeshProUGUI textNumber;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        textNumber.text = "";
    }
}