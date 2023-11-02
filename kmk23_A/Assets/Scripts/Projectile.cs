using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody rb;           //내부에서 사용할 rb

    public float moveSpeed;             //이동속도
    public float damageAmount;          //데미지
    private bool hasDamaged;            //flag 선언

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * moveSpeed;            //rb의 앞쪽 방향으로 총알 이동 속도를 입력
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy" && !hasDamaged)
        {
            other.GetComponent<EnemyHealthController>().TakeDamage((int)damageAmount);  //데미지 계산 추가
            hasDamaged = true;
        }

        Destroy(gameObject);                        //Trigger 충돌이 일어나면 파괴
    }

}
