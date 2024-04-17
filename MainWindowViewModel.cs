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

        
        public MainWindowViewModel()
        {
            ColorString = new ObservableCollection<string>();
            ColorTuple = new ObservableCollection<(string, int)>();
            Cells = new ObservableCollection<CellDto>();
            Generation = 0;
            ButtonsEnabled = true;
            
            Rows = 15;
            Columns = 15;
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


        private ObservableCollection<string> _colorString;
        public ObservableCollection<string> ColorString
        {
            get { return _colorString; }
            set { SetProperty(ref _colorString, value); }
        }

        private ObservableCollection<(string, int)> _colorTuple;
        public ObservableCollection<(string, int)> ColorTuple
        {
            get { return _colorTuple; }
            set { _colorTuple = value; }
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

            TransformGrid();
        }

        public void TransformGrid()
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
            Width = Columns * 20;
            Height= Rows * 20;
            Grid = new GridOfLife(Rows, Columns);

            var livingCells = Cells.Where(x => x.Status == "black").ToList();
            foreach (var cell in livingCells)
            {
                Grid.ToggleCellStatus(cell.Row, cell.Column);
            }

            TransformGrid();
        }

        private void dispatcherTimer_Tick(object? sender, EventArgs e)
        {
            TransformGrid();
            Generation++;

            if (!Grid.CalculateGrowth())
            {
                _dispatcherTimer.Stop();
                MessageBox.Show($"All Dead after {Generation} generations !");
                ClearGrid();
                ButtonsEnabled = true;
            }
            
        }

        private void ClearGrid()
        {
            Grid = new GridOfLife(Rows, Columns);
            Grid.PopulateGrid();
            Generation = 0;
            TransformGrid();
        }

        public void RandomizeCells()
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    var r = new Random();
                    if (r.Next(2) == 1)
                        Grid.ToggleCellStatus(i, j);
                }
            }
            TransformGrid();
        }

    }
}
