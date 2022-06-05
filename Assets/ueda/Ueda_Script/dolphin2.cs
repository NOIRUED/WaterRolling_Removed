using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dolphin2 : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Boat1;
    public GameObject Boat2;
    public float Boatdistance1;
    public float Boatdistance2;
    bool dolphinbool = false;
    public float Speed = 1.0f;

    void Start() {

    }

    // Update is called once per frame
    void Update() {

        Boatdistance1 = Vector3.Distance(Boat1.transform.position, transform.position);
        Boatdistance2 = Vector3.Distance(Boat2.transform.position, transform.position);
        if ((Boatdistance1 <= Boatdistance2) && Boatdistance1 <= 50f && dolphinbool == false && Boatdistance1 >= 5f) {
            StartCoroutine(dolphinattack(Boat1));
            var aim = Boat1.transform.position - this.transform.position;
            var look = Quaternion.LookRotation(aim);
            this.transform.localRotation = look;
            var step = Speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, Boat1.transform.position, step);
        } else if ((Boatdistance2 <= Boatdistance1) && Boatdistance2 <= 50f && dolphinbool == false && Boatdistance2 >= 5f) {
            StartCoroutine(dolphinattack(Boat2));
            var aim = Boat2.transform.position - this.transform.position;
            var look = Quaternion.LookRotation(aim);
            this.transform.localRotation = look;
            var step = Speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, Boat2.transform.position, step);
        }

    }

    private IEnumerator dolphinattack(GameObject boat) {
        dolphinbool = true;
        yield return new WaitForSeconds(0.05f);
        dolphinbool = false;
    }

}
