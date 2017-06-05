using System.Collections;
using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using GameKit;
using ShutEye.UI.Core;
using UnityEngine;
using UnityEngine.EventSystems;

public class scrolportfolioContrpller : SEContainerWithViews<PortfolioContainerUI>, IEnhancedScrollerDelegate, IDataBinding<string[]>
{
    public void UpdateDataView(string[] newdata)
    {
        CurrentData = newdata;
    }

    public int GetNumberOfCells(EnhancedScroller scroller)
    {
        return CurrentData == null ? 0 : CurrentData.Length;
    }

    public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
    {
        return 200;
    }

    public SEUIContainerItem GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {
        // first, we get a cell from the scroller by passing a prefab.
        // if the scroller finds one it can recycle it will do so, otherwise
        // it will create a new cell.
        var cellView = scroller.GetCellView(PrefabSlot, this.InstantiatePrefabCell);

        // set the name of the game object to the cell's data index.
        // this is optional, but it helps up debug the objects in 
        // the scene hierarchy.
        cellView.NameCell = "Cell " + dataIndex.ToString();

        // In this example, we do not set the data here since the cell is not visibile yet. Use a coroutine
        // before the cell is visibile will result in errors, so we defer loading until the cell has
        // become visible. We can trap this in the cellViewVisibilityChanged delegate handled below

        // return the cell to the scroller
        return cellView;
    }
    public string[] CurrentData { get; private set; }
}
