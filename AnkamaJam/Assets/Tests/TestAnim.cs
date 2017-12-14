using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAnim : MonoBehaviour
{
    private Animator _animator;

    private void OnEnable()
    {
        _animator = GetComponent<Animator>();
        Invoke("MarcheOn", 2);
        Invoke("MarcheOff", 4);
        Invoke("MarcheOn", 6);
        Invoke("CriPeurOn", 8);
        Invoke("CriPeurOff", 16);
    }

    private void MarcheOn()
    {
        _animator.SetBool("marche", true);
    }

    private void MarcheOff()
    {
        _animator.SetBool("marche", false);
    }

    private void CriPeurOn()
    {
        _animator.SetBool("criPeur", true);
    }

    private void CriPeurOff()
    {
        _animator.SetBool("criPeur", false);
    }
}