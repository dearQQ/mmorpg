using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Entities;
using Managers;
using SkillBridge.Message;
using Models;

public class GameObjectManager : MonoBehaviour
{

    Dictionary<int, GameObject> Characters = new Dictionary<int, GameObject>();
    private Transform _parent;
    // Use this for initialization
    void Start()
    {
        _parent = GameObject.FindGameObjectWithTag("Role").transform;
        DontDestroyOnLoad(_parent);
        StartCoroutine(InitGameObjects());
        CharacterManager.Instance.OnCharacterEnter += OnCharacterEnter;
        CharacterManager.Instance.OnCharacterLeave += OnCharacterLeave;
    }

    private void OnDestroy()
    {
        CharacterManager.Instance.OnCharacterEnter -= OnCharacterEnter;
        CharacterManager.Instance.OnCharacterLeave -= OnCharacterLeave;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCharacterEnter(Character cha)
    {
        CreateCharacterObject(cha);
    }

    void OnCharacterLeave(Character cha)
    {
        if (!this.Characters.ContainsKey(cha.entityId))
            return;
        if (this.Characters[cha.entityId])
        {
            Destroy(this.Characters[cha.entityId]);
            GameManager.NameMgr.RemoveHearBar(cha.Name);
            this.Characters.Remove(cha.entityId);
        }
    }

    IEnumerator InitGameObjects()
    {
        foreach (var cha in CharacterManager.Instance.Characters.Values)
        {
            CreateCharacterObject(cha);
            yield return null;
        }
    }

    private void CreateCharacterObject(Character character)
    {
        if (!Characters.ContainsKey(character.entityId) || Characters[character.entityId] == null)
        {
            Object obj = Resloader.Load<Object>(character.Define.Resource);
            if(obj == null)
            {
                Debug.LogErrorFormat("Character[{0}] Resource[{1}] not existed.",character.Define.TID, character.Define.Resource);
                return;
            }
            GameObject go = (GameObject)Instantiate(obj);
            go.name = "Character_" + character.entityId + "_" + character.Info.Name;

            go.transform.position = GameObjectTool.LogicToWorld(character.position);
            go.transform.forward = GameObjectTool.LogicToWorld(character.direction);
            go.transform.SetParent(_parent);
            Characters[character.entityId] = go;
            EntityController ec = go.GetComponent<EntityController>();
            if (ec != null)
            {
                ec.entity = character;
                ec.isPlayer = character.IsPlayer;
                EntityManager.Instance.RegisterEntityChangeNotify(character.entityId, ec);
            }
            
            PlayerInputController pc = go.GetComponent<PlayerInputController>();
            if (pc != null)
            {
                if (character.Info.Id == Models.User.Instance.CurrentCharacter.Id)
                {
                    User.Instance.CurrentCharacterObject = go;
                    GameManager.CameraMgr.player = go;
                    pc.enabled = true;
                    pc.character = character;
                    pc.entityController = ec;
                }
                else
                {
                    pc.enabled = false;
                }
            }
            GameManager.NameMgr.AddHeadBar(go.transform,character);
        }
    }
}

