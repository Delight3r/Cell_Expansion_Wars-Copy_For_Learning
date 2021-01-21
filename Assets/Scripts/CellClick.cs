using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Manager;
using Spinner;
using Models.Cell;
using System.Linq;

public class CellClick : MonoBehaviour
{
    #region Properties

    public CellManager cellManager;
    List<GameObject> Cells = new List<GameObject>();
    List<GameObject> ConnectionsIn = new List<GameObject>();
    List<GameObject> ConnectionsOut = new List<GameObject>();

    #endregion

    #region Update

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            OnClicked();
        }
    }

    #endregion

    #region OnClicked

    void OnClicked()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            bool hitConfirmed = false;

            try
            {
                // if this statement is true, hit is confirmed
                if (hit.transform.gameObject.GetComponent<CellModel>())
                {
                    // Hit
                    hitConfirmed = true;
                }
                else
                {
                    // Missed
                    return;
                }
            }
            catch
            {
                // Missed
                return;
            }

            if (!hitConfirmed)
                return;

            GameObject go = hit.transform.gameObject;

            //if (go.GetComponent<CellModel>().color != CellModel.CellColor.Player && Cells.Count == 0)
              //  return;

            if (go.GetComponent<CellModel>().isSelected == true)
            {
                go.GetComponent<CellModel>().isSelected = false;
                ClearCells(ref Cells);
            }
            else
            {
                go.GetComponent<CellModel>().isSelected = true;

                Cells.Add(go);

                if (Cells.Count == 2)
                {
                    int cellCurrentOutLines = Cells.First().GetComponent<CellModel>().currentOutLines.Count;
                    int cellMaxOutLines = Cells.First().GetComponent<CellModel>().maxOutLines;

                    if (CheckIfIsConnected(Cells) || cellCurrentOutLines >= cellMaxOutLines)
                    {
                        if (CheckIfIsConnected(Cells))
                            ConnectConnection(Cells);

                        ClearCells(ref Cells);

                        return;
                    }

                    cellManager.CreateConnection(Cells);
                    ClearCells(ref Cells);
                }
            }
    }

    #endregion

    #region ClearCells

    void ClearCells(ref List<GameObject> Cells)
    {
        if(Cells != null && Cells.Count > 0)
        {
            foreach(GameObject cell in Cells)
                cell.GetComponent<CellModel>().isSelected = false;

            Cells.Clear();
        }
    }

    #endregion

    #region CheckIfIsConnected

    bool CheckIfIsConnected(List<GameObject> Cells)
    {
        bool isConnected = false;

        foreach(GameObject Cell in Cells.First().GetComponent<CellModel>().currentInLines)
        {
            if (Cell == Cells.Last())
                isConnected = true;
        }

        return isConnected;
    }

    #endregion

    #region ConnectConnection

    void ConnectConnection(List<GameObject> Cells)
    {
        List<LineRenderer> line = new List<LineRenderer>();

        if (Cells.Last().GetComponent<CellModel>().LeavingConnections.Count > 0)
        {
            foreach (LineRenderer lr in Cells.Last().GetComponent<CellModel>().LeavingConnections)
            {
                if (lr.GetPosition(1) == Cells.First().gameObject.transform.position)
                {
                    line.Add(lr);
                }
            }
        }

        if(line.Count > 0)
        {
            LineRenderer lineRenderer = line.First();

            lineRenderer.GetComponent<CircleCollider2D>().enabled = true;

            lineRenderer.endColor = Cells.First().GetComponent<CellModel>().sprite.color;
            lineRenderer.startColor = Cells.Last().GetComponent<CellModel>().sprite.color;

            cellManager.CreateConnectionWithoutRenderer(Cells);
        }
    }

    #endregion
}
