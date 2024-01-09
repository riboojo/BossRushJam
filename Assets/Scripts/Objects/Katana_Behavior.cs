using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Katana_Behavior : MonoBehaviour
{
    [SerializeField] private Material matNormal, matBlock;
    [SerializeField] Player_Animations playerAnim;

    private ParticleSystem trail;
    private MeshRenderer mesh;

    private void Start()
    {
        trail = GetComponentInChildren<ParticleSystem>();
        mesh = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        if (playerAnim.IsAttacking())
        {
            TrailOn();
        }
        else
        {
            TrailOff();
        }

        if (playerAnim.IsBlocking())
        {
            GlowOn();
        }
        else
        {
            GlowOff();
        }
    }

    private void GlowOn()
    {
        mesh.material = matBlock;
    }

    private void GlowOff()
    {
        mesh.material = matNormal;
    }

    private void TrailOn()
    {
        trail.gameObject.SetActive(true);
        trail.Play();
    }

    private void TrailOff()
    {
        trail.gameObject.SetActive(false);
        trail.Stop();
    }
}
