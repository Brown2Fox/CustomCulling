using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public GameObject productDisplay;
    public Slider rotator;
    public Dropdown options;
    public GameObject[] rocks;
    public GameObject lastProduct = null;
	// Use this for initialization
	void Start () {
        PlaceProduct(0);
	}
	
	// Update is called once per frame
	void Update () {
        productDisplay.transform.eulerAngles = new Vector3(productDisplay.transform.eulerAngles.x, rotator.value, productDisplay.transform.eulerAngles.z);
	}

    public void UpdateProduct() {
        PlaceProduct(options.value);
    }


    public void PlaceProduct(int product) {
        if (lastProduct) {
            Destroy(lastProduct);
        }
        GameObject p = Instantiate(rocks[product], productDisplay.transform.position, productDisplay.transform.rotation);
        p.transform.parent = productDisplay.transform;
        lastProduct = p;
    }




}
