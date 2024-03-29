using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile_Controller : MonoBehaviour
{
    GameObject _tower;
    GameObject _skill;

    int _y, _x;
    int _towerNum = 0;

    public int Y { get { return _y; } set { _y = value; } }

    public int X { get { return _x; } set { _x = value; } }

    public int TowerNum { get { return _towerNum; } set { _towerNum = value; } }

    public GameObject Tower { get { return _tower; } }
    public GameObject Skill { get { return _skill; } }
    public void InstanceTower(Define.Properties property)
    {

        _tower = Instantiate(GameManager.Data.Tower[(int)property]);
        _tower.transform.parent = transform;

        _tower.transform.localPosition = new Vector3(0,0,0);
    }
    public void InstanceSkill(Define.Skills property)
    {
        _skill = Instantiate(GameManager.Data.Skills[(int)property]);
        
        _skill.transform.parent = transform;
        _skill.GetComponent<SkillTile>().SkillExistTime();
        _skill.transform.localPosition = new Vector3(0, 0, 0);
    }
}
