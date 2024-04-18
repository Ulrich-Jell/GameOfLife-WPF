using ConwaysGameOfLife.Classes;
using Microsoft.Xaml.Behaviors.Core;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace ConwaysGameOfLife
{
    public class MainWindowViewModel : BindableBase
    {

        private int _lastRows;
        private int _lastColumns;
        private string _currentGen;
        private string _lastGen;
        private string _secondLastGen;
        public MainWindowViewModel()
        {
            
            Cells = new ObservableCollection<CellDto>();
            Generation = 0;
            ButtonsEnabled = true;
            
            Rows = 15;
            Columns = 15;
            _lastRows = 15;
            _lastColumns = 15;
            Width = Columns * 20;
            Height = Rows * 20;
            Grid = new GridOfLife(Rows, Columns);

            RandomizeCells();            

            _dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            _dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            _dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            
        }

        

        public DispatcherTimer _dispatcherTimer;

        private int _width;
        public int Width
        {
            get { return _width; }
            set { SetProperty (ref _width, value); }
        }

        private int _height;
        public int Height
        {
            get { return _height; }
            set { SetProperty(ref _height, value); }
        }

        private int _columns;
        public int Columns 
        {
            get { return _columns; } 
            set { SetProperty(ref _columns, value);
                AdjustGrid();
            } 
        }

        private int _rows;
        public int Rows 
        {
            get { return _rows; } 
            set { SetProperty(ref _rows, value);
                AdjustGrid();
            } 
        }     

        private GridOfLife _grid;
        public GridOfLife Grid
        {
            get { return _grid; }
            set { SetProperty(ref _grid, value); }
        }

        private ObservableCollection<CellDto> _cells;
        public ObservableCollection<CellDto> Cells
        {
            get { return _cells; }
            set { SetProperty(ref _cells, value); }
        }

        private int _generation;
        public int Generation
        {
            get { return _generation; }
            set { SetProperty(ref _generation , value); }
        }

        private bool _buttonsEnabled;

        public bool ButtonsEnabled
        {
            get { return _buttonsEnabled; }
            set { SetProperty(ref _buttonsEnabled, value); }
        }

        private void TimerTrigger()
        {
            if (_dispatcherTimer.IsEnabled)
                _dispatcherTimer.Stop();
            else
                _dispatcherTimer.Start();

            ButtonsEnabled = !_dispatcherTimer.IsEnabled;
        }

        public ICommand TriggerCommand => new ActionCommand(TimerTrigger);
        public ICommand ToggleStatusCommand => new ActionCommand(ToggleSatus);
        public ICommand ClearGridCommand => new ActionCommand(ClearGrid);
        public ICommand RandomCommand => new ActionCommand(RandomizeCells);

        private void ToggleSatus(object o)
        {
            var cell = (CellDto)o;
            var status = "";
            if (cell.Status == "white")
            { Grid.Cells[cell.Row][cell.Column].Status = 0; status = "black"; }
            else
            { Grid.Cells[cell.Row][cell.Column].Status = 1; status = "white"; }

            Grid.ToggleCellStatus(cell.Row, cell.Column);
            int index = cell.Row * Columns + cell.Column;
            Cells[index].Status = status;

            ReloadGrid();
        }

        public void ReloadGrid()
        {
            Cells.Clear();
            for (int row = 0; row < Grid.Rows; row++)
            {
                for (int col = 0; col < Grid.Columns; col++)
                {
                    
                    var status = Grid.Cells[row][col].Status;
                    if (status == 1)
                    { Cells.Add(new CellDto(row, col, "black")); }
                    else
                    { Cells.Add(new CellDto(row, col, "white")); }

                }
            }
        }

        public void AdjustGrid()
        {
            if (Columns > 50 || Rows > 50)
            {
                Rows = _lastRows;
                Columns = _lastColumns;
                MessageBox.Show("The maximum of rows and coluimns is 50 each.");
            }
            else
            {
                Width = Columns * 20;
                Height = Rows * 20;
                Grid = new GridOfLife(Rows, Columns);

                var livingCells = Cells.Where(x => x.Status == "black").ToList();


                foreach (var cell in livingCells)
                {
                    if (cell.Row < Rows && cell.Column < Columns)
                        Grid.ToggleCellStatus(cell.Row, cell.Column);
                }

                ReloadGrid();
            }
        }

        private void dispatcherTimer_Tick(object? sender, EventArgs e)
        {
            
            Generation++;
            if (!LifeIsEvolving())
            {
                _dispatcherTimer.Stop();
                MessageBox.Show($"After {Generation} generations nothing new will happen!");
                ClearGrid();
                ButtonsEnabled = true;
            }
            else if (!Grid.CalculateGrowth())
            {
                _dispatcherTimer.Stop();
                MessageBox.Show($"All Dead after {Generation} generations!");
                ClearGrid();
                ButtonsEnabled = true;
            }
            else
                ReloadGrid();
        }

        private void ClearGrid()
        {
            Grid = new GridOfLife(Rows, Columns);
            Grid.PopulateGrid();
            Generation = 0;
            ReloadGrid();
        }

        public void RandomizeCells()
        {
            ClearGrid();
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    var r = new Random();
                    if (r.Next(4) == 1)
                        Grid.ToggleCellStatus(i, j);
                }
            }
            ReloadGrid();
        }

        private bool LifeIsEvolving()
        {
            _secondLastGen = _lastGen;
            _lastGen = _currentGen;
            _currentGen = "";
            foreach (var cell in Cells)
            {
                if (cell.Status == "black")
                    _currentGen += "1";
                else
                    _currentGen += "0";
            }

            if (_currentGen == _lastGen || _currentGen == _secondLastGen)
                return false;
            else
                return true;
        }

    }
}
