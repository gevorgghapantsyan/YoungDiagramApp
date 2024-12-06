﻿using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace YoungDiagramApp
{
    public partial class MainWindow : Window
    {
        private List<List<int>> _diagram = new List<List<int>>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ClearDiagramButton_Click(object sender, RoutedEventArgs e)
        {
            DiagramCanvas.Children.Clear();
            _diagram.Clear();
        }

        private void AddNumberButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int number = Convert.ToInt32(NumberInput.Text);
                AddNumber(_diagram, number);
                DrawDiagram();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Please input correct number:\n {ex.Message}");
            }
        }

        private void AddNumber(List<List<int>> diagram, int number)
        {
            int i = 0;
            while (true)
            {
                if (i >= diagram.Count)
                {
                    diagram.Add(new List<int> { number });
                    break;
                }

                List<int> row = diagram[i];
                int j = 0;

                while (j < row.Count && number >= row[j])
                {
                    
                     j++; 
                   
                }

                if (j == row.Count)
                {
                    row.Add(number);
                    break;
                }

                int current = row[j];
                row[j] = number;
                number = current;

                i++;
            }
        }

        private void DrawDiagram()
        {
            DiagramCanvas.Children.Clear();
            double rectSize = 30;

            for (int row = 0; row < _diagram.Count; row++)
            {
                for (int col = 0; col < _diagram[row].Count; col++)
                {
                    Grid grid = new Grid
                    {
                        Width = rectSize,
                        Height = rectSize
                    };

                    Rectangle rect = new Rectangle
                    {
                        Width = rectSize,
                        Height = rectSize,
                        Stroke = Brushes.Black,
                        Fill = Brushes.Aqua
                    };

                    TextBlock text = new TextBlock
                    {
                        Text = _diagram[row][col].ToString(),
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        FontWeight = FontWeights.Bold
                    };

                    grid.Children.Add(rect);
                    grid.Children.Add(text);

                    Canvas.SetLeft(grid, col * rectSize);
                    Canvas.SetTop(grid, row * rectSize);
                    DiagramCanvas.Children.Add(grid);
                }
            }
        }
    }
}
