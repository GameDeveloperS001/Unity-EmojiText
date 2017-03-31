﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


//Generalized version of Outline8.It renders text meshes at (2X+1)*(2Y+1)-1 positions on a grid whose center is same as the main text mesh.
public class BoxOutline : ModifiedShadow
{
    const int maxHalfSampleCount = 20;

    [SerializeField] [Range(1, maxHalfSampleCount)]
    int m_halfSampleCountX = 1;
    [SerializeField] [Range(1, maxHalfSampleCount)]
    int m_halfSampleCountY = 1;

    public int halfSampleCountX
    {
        get
        {
            return m_halfSampleCountX;
        }

        set
        {
            m_halfSampleCountX = Mathf.Clamp(value, 1, maxHalfSampleCount);
            if (graphic != null)
                graphic.SetVerticesDirty();
        }
    }

    public int halfSampleCountY
    {
        get
        {
            return m_halfSampleCountY;
        }

        set
        {
            m_halfSampleCountY = Mathf.Clamp(value, 1, maxHalfSampleCount);
            if (graphic != null)
                graphic.SetVerticesDirty();
        }
    }

    public override void ModifyVertices(List<UIVertex> verts)
    {
        if (!IsActive())
            return;

        var neededCapacity = verts.Count * (m_halfSampleCountX * 2 + 1) * (m_halfSampleCountY * 2 + 1);
        if (verts.Capacity < neededCapacity)
            verts.Capacity = neededCapacity;

        var original = verts.Count;
        var count = 0;
        var dx = effectDistance.x / m_halfSampleCountX;
        var dy = effectDistance.y / m_halfSampleCountY;
        for (int x = -m_halfSampleCountX; x <= m_halfSampleCountX; x++)
        {
            for (int y = -m_halfSampleCountY; y <= m_halfSampleCountY; y++)
            {
                if (!(x == 0 && y == 0))
                {
                    var next = count + original;
                    ApplyShadow(verts, effectColor, count, next, dx * x, dy * y);
                    count = next;
                }
            }
        }
    }
}
