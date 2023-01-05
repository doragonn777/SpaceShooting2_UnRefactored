using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Test : MonoBehaviour
{
    TextMeshProUGUI tmp;

    // Start is called before the first frame update
    void Start()
    {
        tmp = this.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        tmp.text = "fps " + (1 / Time.deltaTime).ToString("F0");
    }
}
