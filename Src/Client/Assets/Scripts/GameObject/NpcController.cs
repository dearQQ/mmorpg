using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Text;

public class NpcController : MonoBehaviour
{
    public int ID;
    Animator _animator;
    SkinnedMeshRenderer _meshRender;
    Color _color;
    Vector3 _forward;
    bool _isInteractive = false;
    bool _isInteractived = false;
    Coroutine _co;
    private void Start()
    {
        _animator = this.transform.GetComponentInChildren<Animator>();
        _meshRender = this.transform.GetComponentInChildren<SkinnedMeshRenderer>();
        _color = _meshRender.sharedMaterial.color;
        _forward = this.transform.forward;
        StartCoroutine(Action());
    }
    IEnumerator Action()
    {
        while (true)
        {
            if (_isInteractive)
                yield return new WaitForSeconds(2f);

            else
                yield return new WaitForSeconds(UnityEngine.Random.Range(5f, 10f));
            _animator.SetTrigger("Relax");
        }
    }

    IEnumerator Interactive()
    {
        _isInteractive = true;
        _co = StartCoroutine(FacePlayer((Models.User.Instance.CurrentCharacterObject.transform.position - this.transform.position).normalized));
        _animator.SetTrigger("Talk");
        yield return new WaitForSeconds(3f);
        _isInteractive = false;
    }

    IEnumerator FacePlayer(Vector3 faceto)
    {
        //faceto = (Models.User.Instance.CurrentCharacterObject.transform.position - this.transform.position).normalized;

        while (Mathf.Abs(Vector3.Angle(this.transform.forward, faceto)) > 5)
        {
            this.transform.forward = Vector3.Lerp(this.transform.forward, faceto, Time.deltaTime * 5f);
            yield return null;
        }
    }
    
    private void OnMouseDown()
    {
        if (CheckDistance())
            return;
        SetColor(Color.white);
        _isInteractived = true;
        StartCoroutine(Interactive());

    }
    private void OnMouseEnter()
    {
        if (CheckDistance())
            return;
        SetColor(Color.white);
    }

    private void OnMouseExit()
    {
        SetColor(_color);
    }

    void SetColor(Color color)
    {
        _meshRender.sharedMaterial.color = color;
    }

    bool CheckDistance()
    {
        return (Models.User.Instance.CurrentCharacterObject.transform.position - this.transform.position).sqrMagnitude > 8;
    }
    void Update()
    {
        if (_isInteractived)
        {
            if (_co != null)
                StopCoroutine(_co);

            if (CheckDistance())
            {
                _isInteractived = false;
                
                _co = StartCoroutine(FacePlayer(_forward));
            }
            else
                _co = StartCoroutine(FacePlayer((Models.User.Instance.CurrentCharacterObject.transform.position - this.transform.position).normalized));
        }
    }


}
