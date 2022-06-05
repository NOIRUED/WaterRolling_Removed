using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class maruta2zyouge : MonoBehaviour
{
    // Start is called before the first frame update
    public float marutaposition;
    void Start()
    {
        marutaposition = this.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {

        transform.position = new Vector3(transform.position.x, marutaposition - Mathf.PingPong(10 * Time.time, 30f), transform.position.z);
    }
}
