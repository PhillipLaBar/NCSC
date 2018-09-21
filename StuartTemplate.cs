using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;

namespace StuartSurveying2.Android
{
    public class StuartTemplate : Grid
    {
        public StuartTemplate()
        {

            RowDefinitions = new RowDefinitionCollection();
            RowDefinition rd0 = new RowDefinition();
            rd0.Height = 30;
            RowDefinitions.Add(rd0);

            RowDefinition rd1 = new RowDefinition();
            rd1.Height = GridLength.Auto;
            RowDefinitions.Add(rd1);

            RowDefinition rd2 = new RowDefinition();
            rd2.Height = 30;
            RowDefinitions.Add(rd2);            

            //var topBox = new BoxView();
            //topBox.Color = Color.Teal;
            //Children.Add(topBox, 0, 0);

            var topLabel = new Label();
            topLabel.TextColor = Color.White;
            topLabel.Text = "ADP Solutons";
            topLabel.FontSize = 24;
            topLabel.BackgroundColor = Color.Teal;
            Children.Add(topLabel, 0, 0);

            var contentPresenter = new ContentPresenter();
            Children.Add(contentPresenter, 0, 1);            

            //var bottomBox = new BoxView();
            //bottomBox.Color = Color.Teal;
            //Children.Add(bottomBox, 0, 2);

            var bottomLabel = new Label();
            bottomLabel.TextColor = Color.White;
            bottomLabel.Text = "Stuart Servey ...";
            bottomLabel.FontSize = 32;
            bottomLabel.BackgroundColor = Color.Teal;
            Children.Add(bottomLabel, 0, 2);            
        }
    }
}