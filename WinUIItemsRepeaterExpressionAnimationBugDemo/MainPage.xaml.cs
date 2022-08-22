using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Media;

namespace WinUIItemsRepeaterExpressionAnimationBugDemo
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent(); 
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            var scrollProperties = ElementCompositionPreview.GetScrollViewerManipulationPropertySet(MyScrollViewer);

            var headerVisual = ElementCompositionPreview.GetElementVisual(Header);

            var compositor = headerVisual.Compositor;

            var expressionAnimation = compositor.CreateExpressionAnimation();
            expressionAnimation.Expression = "Max(-scroll.Translation.Y, 0)";
            expressionAnimation.SetReferenceParameter("scroll", scrollProperties);

            UIElement parent = Header;
            var index = 0;
            while (true)
            {
                parent = VisualTreeHelper.GetParent(parent) as UIElement;
                if (parent == null || parent == MyScrollViewer)
                {
                    break;
                }

                expressionAnimation.SetExpressionReferenceParameter($"ignore{index}", ElementCompositionPreview.GetElementVisual(parent)); // Comment this line can display items immediately
                index++;
            }

            headerVisual.StartAnimation("Offset.Y", expressionAnimation);
        }

        private void LongButton_Click(object sender, RoutedEventArgs e)
        {
            Repeater.ItemsSource = Enumerable.Range(0, 100);
            MyScrollViewer.ChangeView(null, 0, null, true);
        }

        private void ShortButton_Click(object sender, RoutedEventArgs e)
        {
            Repeater.ItemsSource = Enumerable.Range(0, 20);
            MyScrollViewer.ChangeView(null, 0, null, true);
        }
    }
}