using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Models.Cell;
using System.Linq;
using JetBrains.Annotations;

namespace Manager
{
    public class CellManager : MonoBehaviour
    {
        #region Properties

        public GameObject lineObject;

        public static int amountOfSeeds;

        public static int maxAmountOfSeeds = 200;

        #endregion

        #region CreateConnection

        public void CreateConnection(List<GameObject> Cells)
        {
            if(Cells.Count == 2)
            {
                Vector3[] positions = new Vector3[2];

                Cells.First().GetComponent<CellModel>().isSelected = false;
                Cells.Last().GetComponent<CellModel>().isSelected = false;

                Cells.First().GetComponent<CellModel>().currentOutLines.Add(Cells.Last());
                Cells.Last().GetComponent<CellModel>().currentInLines.Add(Cells.First());

                positions[0] = Cells.First().transform.position;
                positions[1] = Cells.Last().transform.position;

                GameObject line = Instantiate(lineObject);

                line.AddComponent<CircleCollider2D>();

                line.GetComponent<CircleCollider2D>().radius = 1f;
                line.GetComponent<CircleCollider2D>().transform.position =
                   line.GetComponent<LineRenderer>().GetPosition(1) + ( line.GetComponent<LineRenderer>().GetPosition(0) - line.GetComponent<LineRenderer>().GetPosition(1) ) / 2;

                line.GetComponent<CircleCollider2D>().isTrigger = true;

                Color32 cellColor = new Color(
                    Cells.First().GetComponent<CellModel>().sprite.color.r, 
                    Cells.First().GetComponent<CellModel>().sprite.color.g, 
                    Cells.First().GetComponent<CellModel>().sprite.color.b, 150 / 255f);

                line.GetComponent<LineRenderer>().startColor = cellColor;
                line.GetComponent<LineRenderer>().endColor = cellColor;

                Cells.First().GetComponent<CellModel>().LeavingConnections.Add( line.GetComponent<LineRenderer>() );

                line.GetComponent<LineRenderer>().positionCount = 2;
                line.GetComponent<LineRenderer>().SetPositions(positions);
            }
        }

        public void CreateConnectionWithoutRenderer(List<GameObject> Cells)
        {
            Cells.First().GetComponent<CellModel>().isSelected = false;
            Cells.Last().GetComponent<CellModel>().isSelected = false;

            Cells.First().GetComponent<CellModel>().currentOutLines.Add(Cells.Last());
            Cells.Last().GetComponent<CellModel>().currentInLines.Add(Cells.First());
        }

        #endregion
    }
}
