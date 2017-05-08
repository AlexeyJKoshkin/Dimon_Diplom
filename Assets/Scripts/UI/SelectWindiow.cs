using System;
using UnityEngine;
using System.Collections;
using GameKit.UI;

public class SelectWindiow : BaseWindow
{
    public override Enum TypeWindow
    {
        get { return  WindowType.SelectWindiow; }
    }

    public override WindowState State { get; set; }



    public override void RefreshView()
    {
    }
}
