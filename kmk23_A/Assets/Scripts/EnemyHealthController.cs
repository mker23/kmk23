using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthController : MonoBehaviour
{
    public int totalHealth;             //ü�� ����

    public int moneyOnDeath = 50;

    public void TakeDamage(int damageAmount)        //�������� �޴� �Լ�
    {
        totalHealth -= damageAmount;

        if(totalHealth <= 0)
        {
            totalHealth = 0;

            Destroy(gameObject);
            //�̱������� ���� �÷��ִ� ó�� �Լ�
            //���� ���� ó�� ���� ���⼭ ���ش�.
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