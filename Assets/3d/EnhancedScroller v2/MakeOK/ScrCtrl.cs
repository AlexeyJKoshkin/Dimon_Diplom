using UnityEngine;
using System.Collections;
using EnhancedUI.EnhancedScroller;
using LuaInterface;
using System;

public class ScrCtrl : MonoBehaviour, IEnhancedScrollerDelegate
{
    private EnhancedScroller scroller;
    private CellView cellViewPrefab;
    private int totalCellCount;
    private LuaFunction setPrefab;

    public int GetNumberOfCells(EnhancedScroller scroller)
    {
        //throw new NotImplementedException();
        return totalCellCount;
    }

    public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
    {
        //throw new NotImplementedException();
        float height = cellViewPrefab.GetComponent<RectTransform>().rect.height;
        return height;
    }

    public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {
        //throw new NotImplementedException();
        CellView cellView = scroller.GetCellView(cellViewPrefab) as CellView;
        //setPrefab(cellView.gameObject, dataIndex);
        if (setPrefab != null)
        {
            setPrefab.Call(cellView.gameObject, dataIndex);
        }
        return cellView;
    }

    //对外使用的唯一接口
    //参数1表示cell预制体的引用
    //参数2表示设置cell的函数
    //参数3表示cell总数量
    public void StartScrollView(GameObject cellViewPrefabGo, LuaFunction setPrefab, int totalCellCount)
    {
        scroller = transform.parent.GetComponent<EnhancedScroller>();
        scroller.Delegate = this;
        scroller.JumpToDataIndex(0, 0, 0, true);
        RectTransform rect = cellViewPrefabGo.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        cellViewPrefab = cellViewPrefabGo.GetComponent<CellView>();
        this.totalCellCount = totalCellCount;
        this.setPrefab = setPrefab;
        scroller.ReloadData();

    }
}
