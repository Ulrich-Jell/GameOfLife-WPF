using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConwaysGameOfLife.Classes
{
    public class CellOfLife
    {        
        public int Row;
        public int Column;
        public int Status;
        public int GridWidth;
        public int GridHeight;
        public List<(int row, int column)> Neighbours;

        public CellOfLife( int row, int column, int gridWidth, int gridHeight)
        {
            Row = row;
            Column = column;
            Status = 0;
            GridWidth = gridWidth - 1;
            GridHeight = gridHeight - 1;

            Neighbours = new List<(int row, int column)>()
                {
                    (row-1, column-1),
                    (row-1, column),                    
                    (row-1, column+1),
                    (row,   column-1),
                    (row,   column+1),
                    (row+1, column-1),
                    (row+1, column),
                    (row+1, column+1)
                };

        }

        public int GetNeighboursValue(List<List<CellOfLife>> cells)
        {
            int sum = 0;
            foreach (var coordinates in Neighbours)
            {
                if (coordinates.row < 0 || coordinates.column < 0 || coordinates.column > GridWidth || coordinates.row > GridHeight)
                    continue;
                else
                    sum += cells[coordinates.row][coordinates.column].Status;
            }
            return sum;
        }
    }
}
