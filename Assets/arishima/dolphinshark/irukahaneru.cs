using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class irukahaneru : MonoBehaviour
{
    public Vector3 defpos;
    public Vector3 beforepos;
    public Vector3 nowpos;
    float shake;
    // Start is called before the first frame update
    void Start()
    {
        defpos = transform.position; 
    }

    // Update is called once per frame
    void Update()
    {
        //上下に揺れる
        nowpos = transform.position;//現在位置
        shake = Mathf.Abs(2 * Mathf.Sin(Time.time));//揺れる
        transform.position = new Vector3(defpos.x, shake, defpos.z);//反映
        //進行方向を向く
        var diff = transform.position - beforepos;//方向決定
        transform.rotation = Quaternion.LookRotation(diff);//方向反映
        beforepos = transform.position;
    }
}
