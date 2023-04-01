using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTile : MonoBehaviour
{
    GameObject Skill;
    [SerializeField]
    Define.Skill Myskill;

    //float[] _exitTime = new float[3] { 0.5f,5f,5f};

    public void InstanceSkill(Define.Skill skill)// �� �Լ��� Tile_Controller�� ���°� �½��ϴ�
    {

        Skill = Instantiate(GameManager.Instance.Skills[(int)skill]);
        Skill.transform.parent = transform;

        Skill.transform.localPosition = new Vector3(0, 0, 0);
    }

    private void Start()
    {
        switch (Myskill)
        {
            case Define.Skill.Explosion:
                GameManager.Sound.Play("Effect/meteor");
                break;
            case Define.Skill.Sticky:
                GameManager.Sound.Play("Effect/slow");
                break;
            case Define.Skill.Nullity:
                GameManager.Sound.Play("Effect/splash2");
                break;

        }
        this.transform.localScale = new Vector3(0.6482642f, 0.6180149f, 1f);

    }

    //��ų ������ ���� �߰�
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Mob")//�����̰�
        {
            /*
            float range = Vector3.Magnitude(this.gameObject.transform.position - other.gameObject.transform.position);
            if (range <= GameManager.SkillRange)//3*3���� ���ΰ�?
            {
                gameObject.GetComponent<ResourceManager>().InAreaMonster_List.Add(other.gameObject);
            }
            */

            // �׳� SkillŸ�� Collider�� ũ�⸦ 3*3 ũ��� ��

            switch (Myskill) {

                case Define.Skill.Explosion:
                    other.GetComponent<Monster_Controller>().beAttacked(300);
                    break;
                case Define.Skill.Sticky:
                    if(other.GetComponent<Monster_Controller>().stickyCount == 0)//���ǿ� ó�� ����
                    {
                        other.GetComponent<Monster_Controller>().Speed = other.GetComponent<Monster_Controller>().DEFAULTSPEED / 2;//�ӵ� ����
                    }
                    other.GetComponent<Monster_Controller>().stickyCount++;
                    break;
                case Define.Skill.Nullity:
                    if (other.GetComponent<Monster_Controller>().nullCount == 0)//���ǿ� ó�� ����
                    {
                        other.GetComponent<Monster_Controller>().Property = Define.Property.None;
                    }
                    other.GetComponent<Monster_Controller>().nullCount++;
                    break;
            }


        }
    }
    // ���ο찡 ������ ������ ���� �ӵ��� ���Ѵٸ� ������ ��ġ�� �̾������� ù ��° ������ �����ڸ��� ���� �ӵ��� ���ƿ� ���̴� �� ��° ���� ȿ�� x
    // �� ������ ������Ʈ �� ���� ó���� ����� �� ã��
   
    //��ų ������ ���� ����
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Mob")//�����̸�
        {
            //gameObject.GetComponent<ResourceManager>().InAreaMonster_List.Remove(other.gameObject);

            switch (Myskill)
            {

                case Define.Skill.Explosion:
                    //other.GetComponent<Monster_Controller>().beAttacked(300);
                    break;
                case Define.Skill.Sticky:
                    if (other.GetComponent<Monster_Controller>().stickyCount == 1)//������ ������ ����
                    {
                        other.GetComponent<Monster_Controller>().Speed = other.GetComponent<Monster_Controller>().DEFAULTSPEED;//�������
                    }
                    other.GetComponent<Monster_Controller>().stickyCount--;
                    break;
                case Define.Skill.Nullity:
                    if (other.GetComponent<Monster_Controller>().nullCount == 1)//������ ������ ����
                    {
                        other.GetComponent<Monster_Controller>().Property = other.GetComponent<Monster_Controller>().BornProperty;//�������
                    }
                    other.GetComponent<Monster_Controller>().nullCount--;
                    break;

                default:
                    break;
            }
        }
    }

    public void SkillExistTime()
    {
        StartCoroutine(ExistTime());
    }
    IEnumerator ExistTime()
    {
        Debug.Log($"LV[{(int)Myskill + 5}] : {GameManager.Instance.LV[(int)Myskill + 5]} time: {GameManager.SKILLEXISTTIME[GameManager.Instance.LV[(int)Myskill + 5]]}");
        yield return new WaitForSeconds(Myskill == Define.Skill.Explosion ? 0.4f : GameManager.SKILLEXISTTIME[GameManager.Instance.LV[(int)Myskill + 5]]);

        Destroy(this.gameObject);
    }
}