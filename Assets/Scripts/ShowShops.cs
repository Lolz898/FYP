using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowShops : MonoBehaviour
{
    public GameObject shop1;
    public GameObject shop2;
    public GameObject shop3;

    private RectTransform shop1RT;
    private RectTransform shop2RT;
    private RectTransform shop3RT;

    private bool shop1Show = false;
    private bool shop2Show = false;
    private bool shop3Show = false;

    private void Start()
    {
        shop1RT = shop1.GetComponent<RectTransform>();
        shop2RT = shop2.GetComponent<RectTransform>();
        shop3RT = shop3.GetComponent<RectTransform>();

        shop1RT.localPosition = new Vector3(450,shop1RT.localPosition.y,0);
        shop2RT.localPosition = new Vector3(450, shop2RT.localPosition.y, 0);
        shop3RT.localPosition = new Vector3(450, shop3RT.localPosition.y, 0);
    }

    private void Update()
    {
        if (!shop1Show)
        {
            shop1RT.localPosition = Vector3.Lerp(shop1RT.localPosition, new Vector3(450, shop1RT.localPosition.y, 0), 0.04f);
        }
        else
        {
            shop1RT.localPosition = Vector3.Lerp(shop1RT.localPosition, new Vector3(0, shop1RT.localPosition.y, 0), 0.03f);
        }

        if (!shop2Show)
        {
            shop2RT.localPosition = Vector3.Lerp(shop2RT.localPosition, new Vector3(450, shop2RT.localPosition.y, 0), 0.04f);
        }
        else
        {
            shop2RT.localPosition = Vector3.Lerp(shop2RT.localPosition, new Vector3(0, shop2RT.localPosition.y, 0), 0.03f);
        }

        if (!shop3Show)
        {
            shop3RT.localPosition = Vector3.Lerp(shop3RT.localPosition, new Vector3(450, shop3RT.localPosition.y, 0), 0.04f);
        }
        else
        {
            shop3RT.localPosition = Vector3.Lerp(shop3RT.localPosition, new Vector3(0, shop3RT.localPosition.y, 0), 0.03f);
        }
    }

    public void OpenShop1()
    {
        if (!shop1Show)
        {
            shop1Show = true;
            shop2Show = false;
            shop3Show = false;
        }
        else
        {
            shop1Show = false;
        }
    }

    public void OpenShop2()
    {
        if (!shop2Show)
        {
            shop2Show = true;
            shop1Show = false;
            shop3Show = false;
        }
        else
        {
            shop2Show = false;
        }
    }
    public void OpenShop3()
    {
        if (!shop3Show)
        {
            shop3Show = true;
            shop1Show = false;
            shop2Show = false;
        }
        else
        {
            shop3Show = false;
        }
    }

}
