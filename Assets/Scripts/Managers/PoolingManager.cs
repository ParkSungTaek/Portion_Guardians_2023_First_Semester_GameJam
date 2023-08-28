using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PoolingManager
{
    Dictionary<string, Transform> _poolingRoot = new Dictionary<string, Transform>();
    public Dictionary<string, Transform> PoolingRoot { get { return _poolingRoot; } } 

    Dictionary<string, Stack<GameObject>> _pool = new Dictionary<string, Stack<GameObject>>();
    Dictionary<Define.Properties, string> _projectileNameForPool = new Dictionary<Define.Properties, string>();
    Dictionary<Define.Properties, string> _monsterNameForPool = new Dictionary<Define.Properties, string>();

    GameObject root = null;

    public void Init()
    {
        
        if (root == null )
        {

            _projectileNameForPool.Add(Define.Properties.Fire, "MonsterFire");
            _projectileNameForPool.Add(Define.Properties.Water, "MonsterWater");
            _projectileNameForPool.Add(Define.Properties.Grass, "MonsterGrass");

            _monsterNameForPool.Add(Define.Properties.Fire, "ProjectileFire");
            _monsterNameForPool.Add(Define.Properties.Water, "ProjectileWater");
            _monsterNameForPool.Add(Define.Properties.Grass, "ProjectileGrass");


            root =  new GameObject ("root");




            GameObject Monster = new GameObject ("MonsterFire" );
            _poolingRoot.Add("MonsterFire", Monster.transform);
            _pool.Add("MonsterFire", new Stack<GameObject>());
            Monster.transform.parent = root.transform;

            Monster = new GameObject ("MonsterWater" );
            _poolingRoot.Add("MonsterWater", Monster.transform);
            _pool.Add("MonsterWater", new Stack<GameObject>());

            Monster.transform.parent = root.transform;

            Monster = new GameObject ("MonsterGrass" );
            _poolingRoot.Add("MonsterGrass", Monster.transform);
            _pool.Add("MonsterGrass", new Stack<GameObject>());

            Monster.transform.parent = root.transform;


            GameObject Projectile = new GameObject ("ProjectileFire" );
            _poolingRoot.Add("ProjectileFire", Projectile.transform);
            _pool.Add("ProjectileFire", new Stack<GameObject>());

            Projectile.transform.parent = root.transform;

            Projectile =new GameObject ("ProjectileWater");
            _poolingRoot.Add("ProjectileWater", Projectile.transform);
            _pool.Add("ProjectileWater", new Stack<GameObject>());

            Projectile.transform.parent = root.transform;

            Projectile = new GameObject ("ProjectileGrass" );
            _poolingRoot.Add("ProjectileGrass", Projectile.transform);
            _pool.Add("ProjectileGrass", new Stack<GameObject>());

            Projectile.transform.parent = root.transform;

            GameObject Play = new GameObject("PlayProjectile");
            _poolingRoot.Add("PlayProjectile", Play.transform);
            _pool.Add("PlayProjectile", new Stack<GameObject>());

            Play.transform.parent = root.transform;

            Play = new GameObject("PlayMonster");
            _poolingRoot.Add("PlayMonster", Play.transform);
            _pool.Add("PlayMonster", new Stack<GameObject>());

            Play.transform.parent = root.transform;



        }

    }

    public GameObject GetPoolMonster(Define.Properties property)
    {
        

        string name = $"Monster{Enum.GetName(typeof(Define.Properties), property)}";

        if (_pool[name].Count == 0)
        {
            return null;
        }
        else
        {
            GameObject pool = _pool[name].Pop();
            return pool;
        }

    }
    public GameObject GetPoolProjectile(Define.Properties property)
    {


        string name = $"Projectile{Enum.GetName(typeof(Define.Properties), property)}";

        if (_pool[name].Count == 0)
        {
            return null;
        }
        else
        {

            return _pool[name].Pop();
        }
    }
    public void SetPoolMonster(Define.Properties property, GameObject destroyObj)
    {

        //실험용 시스템
        //UnityEngine.Object.Destroy(destroyObj);
        //return;

        Transform MonsterPool;

        string name = $"Monster{Enum.GetName(typeof(Define.Properties), property) }";
        _poolingRoot.TryGetValue(name, out MonsterPool);
        //destroyObj.transform.parent = MonsterPool;


        _pool[name].Push(destroyObj);


    }
    public void SetPoolProjectile(Define.Properties property, GameObject destroyObj)
    {
        //실험용 시스템
        //UnityEngine.Object.Destroy(destroyObj);
        //return;

        Transform Projectile;

        string name = $"Projectile{Enum.GetName(typeof(Define.Properties), property) }";
        _poolingRoot.TryGetValue(name, out Projectile);
        //destroyObj.transform.parent = Projectile;

        _pool[name].Push(destroyObj);

    }
    public void Clear()
    {
        _poolingRoot.Clear();
        _pool.Clear();

        UnityEngine.Object.Destroy(root);
    }
}
