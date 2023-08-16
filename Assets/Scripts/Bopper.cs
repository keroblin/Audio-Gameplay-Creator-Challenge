using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Bopper : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int bopper;
    public delegate void Bop(int dir, int bopper);
    public Bop bop;

    public Animator anim;
    public void OnPointerEnter(PointerEventData eventData)
    {
        bop(1,bopper);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        bop(0, bopper);
    }

    public void Coming(int dir)
    {
        if (dir == 1)
        {
            anim.Play("Incoming");
        }
        else
        {
            anim.Play("Outgoing");
        }
    }
}
