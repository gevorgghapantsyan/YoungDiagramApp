using System.Text;
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
        private int row = 0; 
        private List<List<int>> _diagram = new List<List<int>>(); 

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ClearDiagramButton_Click(object sender, RoutedEventArgs e)
        {
            DiagramCanvas.Children.Clear(); 
            _diagram.Clear(); 
            row = 0; 
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
                MessageBox.Show($"Please input correct number։\n {ex.Message}");
            }
        }

        private void AddNumber(List<List<int>> diagram, int number)
        {
            bool added = false;

            for (int i = 0; i < diagram.Count; i++)
            {
                int currentrow = 0;
                if (i+1<diagram.Count && diagram[i][currentrow] < diagram[i + 1][currentrow]) 
                {
                    if (diagram[i].Last() < number)
                    {
                        diagram[i].Add(number); 
                        added = true;
                        diagram[i].Sort(); 
                        break;
                    }
                    currentrow++;
                }
            }

            if (!added) 
                
            {
                List<int> newRow = new List<int> { number};
                if(diagram.Count == 0 || number>diagram.Last()[0])
                {
                    diagram.Add(newRow);
                }
                else
                {
                    MessageBox.Show("Please check number");
                }
               
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