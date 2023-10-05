using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public EnemyController[] enemiesToSpawn;            //몬스터종류 배열 값

    public Transform spawnPoint;

    public float timeBetweenSpawns = 0.5f;                     //스폰 사이 시간
    private float spawnCounter;                           //숫자를 카운팅하는 변수 시간

    public int amountToSpawn = 15;               //스폰될 때 몬스터 숫자

    // Start is called before the first frame update
    void Start()
    {
        spawnCounter = timeBetweenSpawns;
    }

    // Update is called once per frame
    void Update()
    {
        if(amountToSpawn > 0)                   //남아있는 스폰될 숫자가 잇을때
        {
            spawnCounter -= Time.deltaTime;             //프레임마다 시간을 감소 시킴
            if(spawnCounter <= 0)                       //spawnCOunt 0 이하일 때
            {
                spawnCounter = timeBetweenSpawns;               //정해진 스폰 사이 간격 시간을 다시 리셋
                //Random.Range(0, enemiesToSpawn.Length) 배열안의 랜덤값 정해서 프리팹을 생성, 위치랑 로테이션 값
                Instantiate(enemiesToSpawn[Random.Range(0, enemiesToSpawn.Length)], spawnPoint.position, spawnPoint.rotation);

                amountToSpawn--;                            //스폰될 숫자를 빼준다
            }
        }
    }
}
