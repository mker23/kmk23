using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public float range = 3f;                    //공격 범위
    public float fireRate = 1.0f;               //공격 속도

    public LayerMask isEnemy;                   //레이를 통해서 적 캐릭터 검출

    public Collider[] colliderInRange;          //공격 범위 안에 들어오는 Collider 배열

    public List<EnemyController> enemiesInRange = new List<EnemyController>();              //Collider 배열을 통해서 받은 EnemyController List

    public float checkCounter;  
    public float checkTime = 0.2f;                  //공격 범위 다시 검출하는 시간

    public bool enemiesUpdate;

    // Start is called before the first frame update
    void Start()
    {
        checkCounter = checkTime;                   //0.2초를 Counter에 입력
    }

    // Update is called once per frame
    void Update()
    {
        enemiesUpdate = false;

        checkCounter -= Time.deltaTime;             //Counter에 정해진 시간을 감소 시켜서 주기적으로 체크하게 만듬

        if(checkCounter <= 0)
        {
            checkCounter = checkTime;

            colliderInRange = Physics.OverlapSphere(transform.position, range, isEnemy);        //자신의 위치, 범위, 레이어

            enemiesInRange.Clear();         //List에 있는것 초기화

            foreach (Collider col in colliderInRange)                           //colliderInRange 배열에서
            {
                enemiesInRange.Add(col.GetComponent<EnemyController>());        //EnemyController를 받아와서 List에 넣는다.
            }

            enemiesUpdate = true;
        }
    }
}
