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
using System.Windows.Media.Animation;
using System.Windows.Ink;

namespace YoungDiagramApp
{
    public partial class MainWindow : Window
    {
        private List<List<int>> _diagram = new List<List<int>>();
        private int lastRow;
        private int lastCol;
        private Storyboard storyboard;
        private void SpeedSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            storyboard = new Storyboard();
            if (storyboard != null)
            {
                storyboard.SpeedRatio = SpeedSlider.Value;
            }
        }

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
                if (number <= 0)
                {
                    MessageBox.Show("Please input a positive number.");
                    return; 
                }
                AddNumber(_diagram, number);
                DrawDiagram();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Please input integer:\n {ex.Message}");
            }
            finally { NumberInput.Clear(); }
           
        }

        private void AddNumber(List<List<int>> diagram, int number)
        {
            int i = 0;
            bool isadded = true; 

            while (true)
            {
                if (i >= diagram.Count)
                {
                    diagram.Add(new List<int> { number });
                    if (isadded)
                    {
                        lastRow = i;
                        lastCol = 0;
                    }
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
                    if (isadded)
                    {
                        lastRow = i;
                        lastCol = j;
                    }
                    break;
                }

                int current = row[j];
                row[j] = number;
                number = current;

                if (isadded)
                {
                    lastRow = i;
                    lastCol = j;
                    isadded = false; 
                }

                i++;
            }
        }



        public void DrawDiagram()
        {
            double rectSize = 50;
            DiagramCanvas.Children.Clear();
          
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
                        Stroke = new SolidColorBrush(Colors.Black),
                        StrokeThickness = 1,
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

                    double finalLeft = col * rectSize;
                    double finalTop = row * rectSize;

                    if (row == lastRow && col == lastCol)
                    {
                        Canvas.SetLeft(grid, 600);
                        Canvas.SetTop(grid, 150);
                        ColorAnimation colorAnimation = new ColorAnimation 
                        {
                            From = Colors.Black, 
                            To = Colors.Red, 
                            Duration = new Duration(TimeSpan.FromSeconds(2)), 
                            AutoReverse = true,
                            
                        };
                        Storyboard.SetTarget(colorAnimation, rect); 
                        Storyboard.SetTargetProperty(colorAnimation, new PropertyPath("Stroke.Color"));

                        DoubleAnimationUsingKeyFrames thicknessAnimation = new DoubleAnimationUsingKeyFrames(); 
                        
                        thicknessAnimation.KeyFrames.Add(new LinearDoubleKeyFrame(1, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0))));
                        thicknessAnimation.KeyFrames.Add(new LinearDoubleKeyFrame(5, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(2))));
                        thicknessAnimation.KeyFrames.Add(new LinearDoubleKeyFrame(1, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(3))));
                            
                            
                        
                        Storyboard.SetTarget(thicknessAnimation, rect); 
                        Storyboard.SetTargetProperty(thicknessAnimation, new PropertyPath(Rectangle.StrokeThicknessProperty));
                        
                        DoubleAnimation leftAnimation = new DoubleAnimation
                        {
                            From = 600,
                            To = finalLeft,
                            Duration = TimeSpan.FromSeconds(2),
                            BeginTime = TimeSpan.FromSeconds(4),
                        };

                        DoubleAnimation topAnimation = new DoubleAnimation
                        {
                            From = 150,
                            To = finalTop,
                            Duration = TimeSpan.FromSeconds(2),
                            BeginTime=TimeSpan.FromSeconds(4),
                        };

                        Storyboard.SetTarget(leftAnimation, grid);
                        Storyboard.SetTarget(topAnimation, grid);
                        Storyboard.SetTargetProperty(leftAnimation, new PropertyPath("(Canvas.Left)"));
                        Storyboard.SetTargetProperty(topAnimation, new PropertyPath("(Canvas.Top)"));

                        storyboard.Children.Add(thicknessAnimation);
                        storyboard.Children.Add(colorAnimation);
                        storyboard.Children.Add(leftAnimation);
                        storyboard.Children.Add(topAnimation);
                    }
                    else
                    {
                        Canvas.SetLeft(grid, finalLeft);
                        Canvas.SetTop(grid, finalTop);

                    }

                    DiagramCanvas.Children.Add(grid);
                }
            }

            storyboard.Begin();
        }


    }
}




