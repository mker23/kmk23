using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Slot[] slots;

    private Vector3 _target;                //�̵� ��Ű�� �ִ� ������ ����
    private ItemInfo carryingItem;

    private Dictionary<int, Slot> slotDictionary;           //���� ������ �����ϴ� �ڷᱸ��           

    void Start()
    {
        slotDictionary = new Dictionary<int, Slot>();           //���� ��ųʸ� �ʱ�ȭ

        for(int i = 0; i < slots.Length; i++)           //�� ������ ID�� �����ϰ� ��ųʸ��� �߰�
        {
            slots[i].id = i;
            slotDictionary.Add(i, slots[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))                 //���콺 ��ư�� ������ ��
        {
            SendRayCast();
        }

        if(Input.GetMouseButton(0) && carryingItem)                 //���콺 ��ư ���� ���¿��� ������ ���� �� �̵� ����
        {
            OnItemSelected();
        }
        
        if(Input.GetMouseButtonUp(0))                           //���콺 ��ư ���� �̺�Ʈ ó��
        {
            SendRayCast();
        }
        
        if (Input.GetKeyDown(KeyCode.Space))                    //�����̽� Ű�� ������ �� ���� ������ ��ġ
        {
            PlaceRandomItem();
        }
    }

    void SendRayCast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);            //ȭ���� ���콺 ��ǥ�� ���ؼ� ���� ���� ����ĳ����
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
        else //hit�� �������
        {
            if(!carryingItem)
            {
                return;
            }
            OnItemCarryFail();
        }
    }

    //�������� �����ϰ� ���콺 ��ġ�� �̵�
    void OnItemSelected()               //�������� �����ϰ� ���콺 ��ġ�� �̵�
    {
        _target = Camera.main.ScreenToWorldPoint(Input.mousePosition);      //���� ��ǥ���� ���콺 ������ ���� �����ͼ� _target�Է�
        _target.z = 0;

        var delta = 10 * Time.deltaTime;                            //�̵� �ӵ� ����

        delta *= Vector3.Distance(transform.position, _target);
        carryingItem.transform.position = Vector3.MoveTowards(carryingItem.transform.position, _target, delta);     //�Լ��� �̵�
    }

    void OnItemMergedWithTarget(int targetSlotId)       //�������� ���԰� ���� �� �� �Լ�
    {
        var slot = GetSlotByld(targetSlotId);           //���� ���Կ� �ִ� ������Ʈ�� �����ͼ� �ı�
        Destroy(slot.itemObject.gameObject);
        slot.CreateItem(carryingItem.itemId + 1);       //���� �Ǿ����Ƿ� ���� ������Ʈ�� ����
        Destroy(carryingItem.gameObject);               //����ִ� ���� ������Ʈ�� �ı�
    }

    void OnItemCarryFail()                      //������ ��ġ ���� �� ����
    {
        var slot = GetSlotByld(carryingItem.slotId);        //��� �ִ� �������� ���� ���� ��ġ
        slot.CreateItem(carryingItem.itemId);               //�ش� ���Կ� �ٽ� ����
        Destroy(carryingItem.gameObject);                   //��� �ִ� ���� �������� ����
    }

    void PlaceRandomItem()                  //������ ���Կ� ������ ��ġ
    {
        if(AllSlotsOccupied())
        {
            Debug.Log("������ �� ������ => ���� �Ұ�");
            return;
        }

        var rand = UnityEngine.Random.Range(0, slots.Length);       //���� ������ ���� ���� ��ȣ�� rand�� �Է�
        var slot = GetSlotByld(rand);                               //Rand���� index���� ���ؼ� slot ��ü�� �����´�.
        
        while (slot.state == Slot.SLOTSTATE.FULL)                   //���� ���°� FULL�� �ƴҶ����� ���� ��ȣň ã�Ƽ� ����                 
        {
            rand = UnityEngine.Random.Range(0, slots.Length);   //���� ������ ���� ���� ��ȣ�� RAND�� �Է�
            slot = GetSlotByld(rand);                           //Rand ���� index���� ���ؼ� slot ��ü�� �����´�.
        }
        slot.CreateItem(0);                                     //�� ������ �߰��ϸ� 0��° �������� ����
    }


    bool AllSlotsOccupied()
    {
        foreach(var slot in slots)                  //������ ä���� �ִ��� Ȯ���ϴ� �Լ�
        {
            if(slot.state == Slot. SLOTSTATE.EMPTY)     //���� �迭�� �� �ڸ��� ������
            {
                return false;                   //�߰��� false�� ����
            }
        }
        return true;                //�� �������Ƿ� true�� ����
    }

    Slot GetSlotByld(int id)        //���� ID�� ������ �˻�
    {
        return slotDictionary[id];      //��ųʸ��� ����ִ� Slot Class ��ȯ (��ȣ�� ���ؼ�)
    }


}
