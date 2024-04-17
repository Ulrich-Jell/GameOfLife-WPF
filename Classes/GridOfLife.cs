using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConwaysGameOfLife.Classes
{
    public class GridOfLife
    {
        public int Rows { get; set; }
        public int Columns { get; set; }
        public List<List<CellOfLife>> Cells { get; set; }

        public GridOfLife(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            Cells = new List<List<CellOfLife>>();
            PopulateGrid();
        }

        public void PopulateGrid()
        {
            for (int r = 0; r < Rows; r++)
            {
                var row = new List<CellOfLife>();
                for (int c = 0; c < Columns; c++)
                    row.Add(new CellOfLife(r, c, Rows, Columns));
                Cells.Add(row);
            }
        }

        public void PrintGrid()
        {
            foreach (var row in Cells)
            {
                var rowstring = "";
                foreach (var cell in row)
                    rowstring += cell.Status.ToString();
                Console.WriteLine(rowstring);
            }
            Console.WriteLine();
        }

        public bool CalculateGrowth()
        {
            var temp = new List<List<int>>();
            int sum = 0;

            foreach (var row in Cells)
            {
                var newRow = new List<int>();
                foreach (var cell in row)
                {
                    int n = cell.GetNeighboursValue(Cells);
                    if (cell.Status == 0 && n == 3)
                    {
                        sum++;
                        newRow.Add(1);
                    }
                    else if (cell.Status == 1 && n < 2)
                        newRow.Add(0);
                    else if (cell.Status == 1 && (n == 3 || n == 2))
                    {
                        sum++;
                        newRow.Add(1);
                    }
                    else if (cell.Status == 1 && n > 3)
                        newRow.Add(0);
                    else
                        newRow.Add(0);
                }
                temp.Add(newRow);
            }
            return ApplyGrowth(temp, sum);
        }

        public bool ApplyGrowth(List<List<int>> growth, int sum)
        {
            if (sum != 0)
            {
                for (int row = 0; row < Rows; row++)
                {
                    for (int cell = 0; cell < Columns; cell++)
                    {
                        Cells[row][cell].Status = growth[row][cell];
                    }
                }
                //PrintGrid();
                return true;
            }
            else
                return false;
        }

        public void PrintNeighbours()
        {
            var temp = new List<List<CellOfLife>>(Cells);

            foreach (var row in Cells)
            {
                string rowstring = string.Empty;
                foreach (var cell in row)
                {
                    int n = cell.GetNeighboursValue(temp);
                    rowstring += n.ToString();
                }
                Console.WriteLine(rowstring);

            }
            Console.WriteLine();

        }

        public void ToggleCellStatus(int row, int column)
        {
            var status = Cells[row][column].Status;

            if (status == 1)
                Cells[row][column].Status = 0;
            else
                Cells[row][column].Status = 1;
        }
    }
}
