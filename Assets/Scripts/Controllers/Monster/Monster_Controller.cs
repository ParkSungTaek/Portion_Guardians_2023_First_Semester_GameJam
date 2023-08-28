using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Controller : MonoBehaviour
{

    /// <summary> �갡 ����ֳ�? </summary>
    bool _live = true;

    /// <summary> ������ HP ����ֳ�? </summary>
    float _HP = 10;
    
    /// <summary> ������ �⺻ �ӵ� </summary>
    public float DEFAULTSPEED { get { return 3.5f; } }
    
    /// <summary> ������ ���� �ӵ� </summary>
    float _speed { get; set; } = 3.5f;

    /// <summary> x�� </summary>
    public float xpos { get; set; }
    /// <summary> y�� </summary>
    public float ypos { get; set; }
    /// <summary> ������ ���� �� ��ġ </summary>
    static Vector3[] direction;

    public float horizon { get; set; } = 1.5f;
    public float yzero { get; set; } = 0.1f;
    public float yEnd { get; set; } = 3.0f;
    /// <summary> �갡 ����ֳ�?  </summary>
    public bool Live { get { return _live; } set { _live = value; } }
    /// <summary> �ӵ� (���ο� & �ӵ� ����ȭ�� �̿�) </summary>
    public float Speed { get { return _speed; } set { _speed = value; } }
    /// <summary> ���ο� ���� ��ġ�� Ƚ�� </summary>
    public int stickyCount { get; set; }
    /// <summary> �Ӽ� ��ȿ ���� ��ġ�� Ƚ�� </summary>
    public int nullCount { get; set; }
    int _checkBox = 0;

    public int checkBox { get { return _checkBox; } set { _checkBox = value; } }
    public float dist { get; set; }

    /// <summary>
    /// pooling ��ɿ� ������ �־� ������¿�
    /// </summary>
    int pooling { get; set; }

    /// <summary> ���� �¾ �� �Ӽ�(��,��,Ǯ) </summary>
    Define.Properties _bornproperty;
    [SerializeField]
    /// <summary> ���� ���� �Ӽ� (��,��,Ǯ <-> ���Ӽ� ) </summary>
    Define.Properties _property = Define.Properties.Fire;
    /// <summary> ���� ���� �Ӽ� (��,��,Ǯ <-> ���Ӽ� ) </summary>
    public Define.Properties Property { get { return _property; } set { _property = value; } }
    /// <summary> ���� �¾ �� �Ӽ�(��,��,Ǯ) </summary>
    public Define.Properties BornProperty { get { return _bornproperty; }}

    /// <summary> ���� ���� HP </summary>
    public float HP { get { return _HP;} set { _HP = value; } }
    public LinkedList<GameObject> inRangeTower = new LinkedList<GameObject>();
    public void RemoveNode(GameObject value)
    {
        LinkedListNode<GameObject> node = inRangeTower.First; // ����Ʈ�� ó������ ����

        while (node != null)
        {
            if (node.Value.Equals(value)) // ���ϴ� ���� ã����
            {
                inRangeTower.Remove(node); // �ش� ��� ����
                break;
            }
            node = node.Next; // ���� ���� �̵�
        }
    }

    void RemoveAll()
    {
        LinkedListNode<GameObject> node = inRangeTower.First; // ����Ʈ�� ó������ ����

        while (node != null)
        {
            node.Value.GetComponent<T_Controller>().RemoveNode(gameObject);

            //if (node.Value.Equals(value)) // ���ϴ� ���� ã����
            //{
            //    inRangeTower.Remove(node); // �ش� ��� ����
            //    break;
            //}
            node = node.Next; // ���� ���� �̵�
        }
    }

    /// <summary>
    /// ���� Ȥ�� pooling ������ �ʱ�ȭ
    /// </summary>
    void OnEnable()
    {
        inRangeTower.Clear();
        _live = true;
        _HP = 10;
        _speed = DEFAULTSPEED;
        horizon = 1.5f;
        yzero = 0.1f;
        yEnd = 3.0f;
        stickyCount = 0;
        nullCount = 0;
        _checkBox = 0;
        dist = 0;
        transform.position = GameManager.Instance.StartPoint.transform.position;
        _bornproperty = _property;
        checkBox = 0;
        HP = GameManager.Instance.StartHP + (GameManager.Instance.Wave - 1) * (GameManager.Instance.Wave - 1) * (GameManager.Instance.WaveHPPlus);


    }


    public void beAttacked( Define.Properties Property) //������, ����ü �Ӽ�
    {
        float DMG = GameManager.DMGTABLE[GameManager.Instance.LV[(int)Property]];
        

        //�Ӽ� �˻�
        if ((this._property == Define.Properties.Fire && Property == Define.Properties.Water) ||
            ((this._property == Define.Properties.Water && Property == Define.Properties.Grass)) ||
            (this._property == Define.Properties.Grass && Property == Define.Properties.Fire))//����
        {
            
            DMG *= 1.5f;
        }
        if ((this._property == Define.Properties.Fire && Property == Define.Properties.Grass) ||
            ((this._property == Define.Properties.Water && Property == Define.Properties.Fire)) ||
            (this._property == Define.Properties.Grass && Property == Define.Properties.Water))//�ݰ�
        {
            DMG *= 0.5f;
        }

        this.HP -= DMG;
        if (this.HP <= 0)//���
        {
            _live = false;
            Dead();
        }
    }
    public void beAttacked(float DMG) //������, ����ü �Ӽ�
    {
        this.HP -= DMG;
        if (this.HP <= 0)//���
        {
            _live = false; 
            Dead();

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //����ü�� ����
        if (collision.gameObject.tag == "Arrow")
        {
            beAttacked(collision.gameObject.GetComponent<Projectile_Controller>().ProjProp());

        }
        //����ǥ�� �浹
        if (collision.gameObject.tag == "Dir")
        {
            checkBox++;
        }
    }
    
    

    public void Dead()  //����
    {
        if (!_live)
        {
            switch (_bornproperty) 
            {
                case Define.Properties.Fire:
                    GameManager.Sound.Play("Effect/firedie");
                    break;
                case Define.Properties.Water:
                    GameManager.Sound.Play("Effect/waterdie");
                    break;
                case Define.Properties.Grass:
                    GameManager.Sound.Play("Effect/plantdie");
                    break;

            }
            GameManager.Instance.Money += GameManager.Instance.Wave * 2;
            GameManager.Instance.NowPoint += 1;
        }
        RemoveAll();
        transform.position = StartPoint.StartPos;
        _bornproperty = _property;
        checkBox = 0;
        _speed = 0;
        stickyCount = 0;
        nullCount = 0;
        GameManager.Resource.DestroyMonster(_property, this.gameObject);
    }

    // Start is called before the first frame update
    void Awake()
    {
        pooling = 0;
        if(direction == null)
        {
            direction = GameManager.Instance.Direction;

        }

        Transform MonsterPool;
        string name = $"Monster{Enum.GetName(typeof(Define.Properties), _property)}";
        GameManager.Pooling.PoolingRoot.TryGetValue(name, out MonsterPool);
        transform.parent = MonsterPool;
    }

    
    private void Update()
    {
        xpos = transform.position.x;
        ypos = transform.position.y;
        transform.position = Vector3.MoveTowards(transform.position, direction[checkBox], _speed * Time.deltaTime);

        this.dist += this._speed;
    }



}