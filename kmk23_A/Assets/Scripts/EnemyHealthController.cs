using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthController : MonoBehaviour
{
    public int totalHealth;             //체력 설정

    public int moneyOnDeath = 50;

    public void TakeDamage(int damageAmount)        //데미지를 받는 함수
    {
        totalHealth -= damageAmount;

        if(totalHealth <= 0)
        {
            totalHealth = 0;

            Destroy(gameObject);
            //싱글톤으로 돈을 올려주는 처리 함수
            //죽은 이후 처리 들을 여기서 해준다.
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
