using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Slot[] slots;

    private Vector3 _target;                //이동 시키고 있는 아이템 정보
    private ItemInfo carryingItem;

    private Dictionary<int, Slot> slotDictionary;           //슬롯 정보값 관리하는 자료구조           

    void Start()
    {
        slotDictionary = new Dictionary<int, Slot>();           //슬롯 딕셔너리 초기화

        for(int i = 0; i < slots.Length; i++)           //각 슬롯의 ID를 설정하고 딕셔너리에 추가
        {
            slots[i].id = i;
            slotDictionary.Add(i, slots[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))                 //마우스 버튼을 눌렀을 때
        {
            SendRayCast();
        }

        if(Input.GetMouseButton(0) && carryingItem)                 //마우스 버튼 누름 상태에서 아이템 선택 및 이동 정리
        {
            OnItemSelected();
        }
        
        if(Input.GetMouseButtonUp(0))                           //마우스 버튼 떼기 이벤트 처리
        {
            SendRayCast();
        }
        
        if (Input.GetKeyDown(KeyCode.Space))                    //스페이스 키를 눌렀을 때 랜덤 아이템 배치
        {
            PlaceRandomItem();
        }
    }

    void SendRayCast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);            //화면의 마우스 좌표를 통해서 월드 상의 레이캐스팅
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            var slot = hit.transform.GetComponent<Slot>();
            if(slot.state == Slot.SLOTSTATE.FULL && carryingItem == null)
            {
                string itemPath = "Prefabs/Item_Grabbed" + slot. itemObject.id.ToString("000");
                var itemGo = (GameObject)Instantiate(Resources.Load(itemPath));
                itemGo.transform.position = slot.transform.position;
                itemGo.transform.localScale = Vector3.one * 2;

                carryingItem = itemGo.GetComponent<ItemInfo>();
                carryingItem.InitDummy(slot.id ,slot.itemObject.id);

                slot.ItemGrabbed();
            }
            else if(slot.state == Slot.SLOTSTATE.EMPTY && carryingItem != null)
            {
                slot.CreateItem(carryingItem.itemId);
                Destroy(carryingItem.gameObject);
            }
            else if (slot.state == Slot.SLOTSTATE.EMPTY && carryingItem != null)
            {
                if(slot.itemObject.id == carryingItem.itemId)
                {
                    OnItemMergedWithTarget(slot.id);
                }
                else
                {
                    OnItemCarryFail();
                }
            }
        }
        else //hit가 없을경우
        {
            if(!carryingItem)
            {
                return;
            }
            OnItemCarryFail();
        }
    }

    //아이템을 선택하고 마우스 위치로 이동
    void OnItemSelected()               //아이템을 선택하고 마우스 위치로 이동
    {
        _target = Camera.main.ScreenToWorldPoint(Input.mousePosition);      //월드 좌표에서 마우스 포지션 값을 가져와서 _target입력
        _target.z = 0;

        var delta = 10 * Time.deltaTime;                            //이동 속도 조절

        delta *= Vector3.Distance(transform.position, _target);
        carryingItem.transform.position = Vector3.MoveTowards(carryingItem.transform.position, _target, delta);     //함수로 이동
    }

    void OnItemMergedWithTarget(int targetSlotId)       //아이템이 슬롯과 병합 될 때 함수
    {
        var slot = GetSlotByld(targetSlotId);           //기존 슬롯에 있는 오브젝트를 가져와서 파괴
        Destroy(slot.itemObject.gameObject);
        slot.CreateItem(carryingItem.itemId + 1);       //병합 되었으므로 다음 오브젝트를 생성
        Destroy(carryingItem.gameObject);               //들고있던 더미 오브젝트를 파괴
    }

    void OnItemCarryFail()                      //아이템 배치 실패 시 실행
    {
        var slot = GetSlotByld(carryingItem.slotId);        //잡고 있던 아이템의 원래 슬롯 위치
        slot.CreateItem(carryingItem.itemId);               //해당 슬롯에 다시 생성
        Destroy(carryingItem.gameObject);                   //잡고 있는 더미 아이템을 삭제
    }

    void PlaceRandomItem()                  //랜덤한 슬롯에 아이템 배치
    {
        if(AllSlotsOccupied())
        {
            Debug.Log("슬롯이 다 차있음 => 생성 불가");
            return;
        }

        var rand = UnityEngine.Random.Range(0, slots.Length);       //슬롯 갯수에 따라서 랜덤 번호를 rand에 입력
        var slot = GetSlotByld(rand);                               //Rand받은 index갑을 통해서 slot 객체를 가져온다.
        
        while (slot.state == Slot.SLOTSTATE.FULL)                   //슬롯 상태가 FULL이 아닐때까지 랜덤 번호흫 찾아서 진행                 
        {
            rand = UnityEngine.Random.Range(0, slots.Length);   //슬롯 갯수에 따라서 랜덤 번호를 RAND에 입력
            slot = GetSlotByld(rand);                           //Rand 받은 index값을 통해서 slot 객체를 가져온다.
        }
        slot.CreateItem(0);                                     //빈 슬롯을 발견하면 0번째 아이템을 생성
    }


    bool AllSlotsOccupied()
    {
        foreach(var slot in slots)                  //슬롯이 채워져 있는지 확인하는 함숨
        {
            if(slot.state == Slot. SLOTSTATE.EMPTY)     //슬롯 배열에 빈 자리가 있으면
            {
                return false;                   //중간에 false를 리턴
            }
        }
        return true;                //다 차있으므로 true를 리턴
    }

    Slot GetSlotByld(int id)        //슬롯 ID로 슬롯을 검색
    {
        return slotDictionary[id];      //딕셔너리에 담겨있는 Slot Class 반환 (번호를 통해서)
    }


}
