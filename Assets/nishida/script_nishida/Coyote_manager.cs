using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coyote_manager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public class Coyo_Card : MonoBehaviour {
    //カード順番未定
    //配列No:コヨーテカードの数値
    //0-3:0(0が月0)
    //4-7:1
    //8-11:2
    //12-15:3
    //16-19:4
    //20-23:5(ここまでは割り算で楽に計算できそう
    //24-26:10
    //27,28:15
    //29:20
    //30,31:-5
    //32:-10
    //33-35:x2,Max0,?
    public static int[] coyo_card = new int[36];
    public static List<int> coyo_deck = new List<int>();
    public static List<int> coyo_field = new List<int>();
    public static List<int> coyo_grave = new List<int>();



    public static void Init() {
        for(int i = 0; i < coyo_card.Length; ++i) {
            coyo_card[i] = i;
        }
        coyo_deck.AddRange(coyo_card);

    }

    public static void coyo_deckshuffle() {
        int temp = 0;
        for(int i = 0; i < 1000; ++i) {
            int start = Random.Range(0, coyo_deck.Count);
            int goal = Random.Range(0, coyo_deck.Count);
            temp = coyo_deck[start];
            coyo_deck[start] = coyo_deck[goal];
            coyo_deck[goal] = temp;
        }
    }

    public static int coyo_cardpickup() {
        int num = coyo_deck[0];
        coyo_deck.RemoveAt(0);
        coyo_field.Add(num);
        return num;
    }

    public static int coyo_search(int num) {
        int ber = 0;
        if (num <= 23) {
            ber = num / 4;
        } else if (num <= 26) {
            ber = 10;
        } else if (num <= 28) {
            ber = 15;
        } else if (num == 29) {
            ber = 20;
        } else if (num <= 31) {
            ber = -5;
        } else if (num == 32) {
            ber = -10;
        } 
        return ber;
    }

    public static int coyo_calc() {
        int sum=0,max=-20;
        bool multi = false, mto0 = false, ques = false, shu=false;
        while (coyo_field.Count != 0) {
            int num = coyo_field[0];
            int ber=0;

            ber = coyo_search(num);

            if (num == 0) {
                shu = true;
            }else if (num == 33) {
                multi = true;
            }else if (num == 34) {
                mto0 = true;
            }else if (num == 35) {
                ques = true;
            }

            num += ber;
            if (max <= ber) {
                max = ber;
            }

            coyo_field.RemoveAt(0);
            coyo_grave.Add(num);
         }

        if (ques) {
            int num = coyo_cardpickup();

            int ber =coyo_search(num);

            sum += ber;
            if (max <= ber) {
                max = ber;
            }

            if (num == 0) {
                shu = true;
            } else if (num == 33) {
                multi = true;
            } else if (num == 34) {
                mto0 = true;
            }

            coyo_field.RemoveAt(0);
            coyo_grave.Add(num);


        }
        if (mto0) {
            sum -= max;
        }
        if (multi) {
            sum *= 2;
        }

        if (shu) {
            coyo_deck.Clear();
            coyo_field.Clear();
            coyo_grave.Clear();
            coyo_deck.AddRange(coyo_card);
        }

        return sum;
    }
}