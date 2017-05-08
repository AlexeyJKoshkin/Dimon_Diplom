using System;
using System.Collections;
using System.Collections.Generic;
using GameKit.UI;
using UnityEngine;

public class ProfileWindow : BaseWindow {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override Enum TypeWindow
    {
        get { throw new NotImplementedException(); }
    }

    public override WindowState State { get; set; }
    public override void RefreshView()
    {
        throw new NotImplementedException();
    }
}
