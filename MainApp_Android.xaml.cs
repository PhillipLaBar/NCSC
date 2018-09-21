// Copyright Phillip Labar 2017
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Bluetooth;
using Android.Content; 
using Android.OS; 
using Android.Util; 
using Android.Views; 
using Android.Views.InputMethods; 
using Android.Widget; 
using Java.Lang;
using Java.Lang.Reflect;
using Java.IO;

using Xamarin.Forms;
using UIKit;
using System.Threading;

namespace StuartSurveying2.Android
{
    public partial class MainApp_Android : Xamarin.Forms.Application
    {
        // Local Bluetooth adapter 
        public BluetoothAdapter _bluetoothAdapter = null;

        public BluetoothServerSocket _bluetoothServerSocket = null;
        public BluetoothDevice _bluetoothDevice = null;
        public BluetoothSocket _bluetoothSocket = null;

        public BufferedReader _readerBuffer = null;
        public System.IO.Stream _inStream = null;
        public InputStreamReader _inStreamReader = null;

        public BufferedWriter _writeBuffer = null;
        public System.IO.Stream _outStream = null;
        public OutputStreamWriter _outStreamWriter = null;

        public OutputStream _outputStream = null;
        public InputStream _inputStream = null;
        
        private const int REQUEST_CONNECT_DEVICE = 1; 
 		private const int REQUEST_ENABLE_BT = 2;

        protected int _state; 
 	 
 		// Constants that indicate the current connection state 
 		// TODO: Convert to Enums 
 		public const int STATE_NONE = 0;       // we're doing nothing 
 		public const int STATE_LISTEN = 1;     // now listening for incoming connections 
 		public const int STATE_CONNECTING = 2; // now initiating an outgoing connection 
 		public const int STATE_CONNECTED = 3;  // now connected to a remote device 


        List<KeyValuePair<string, string>> _bluetoothDevices = new List<KeyValuePair<string, string>>();

        bool _clientMode = false;
        Xamarin.Forms.Button _startServerButton = new Xamarin.Forms.Button();

        public Java.Util.UUID _myUUID;
        public string _myName;

        Label _testLabel = new Label();

        CarouselPage _carouselPage = null;

        int _previousPageIndex = 0;        

        public BluetoothServer _serverThread = null;

        public double _screenHeight;
        public double _screenWidth;

        public double _fontSizeMicro = 0;
        public double _fontSizeSmall = 0;
        public double _fontSizeMed = 0;
        public double _fontSizeLarge = 0;

        List<KeyValuePair<string, Label>> _labelCollection = new List<KeyValuePair<string, Label>>();
        List<KeyValuePair<string, string>> _labelCollectionSummaryText = new List<KeyValuePair<string, string>>();

       
        TapGestureRecognizer _tapConnect = new TapGestureRecognizer();
        TapGestureRecognizer _tapLabel = new TapGestureRecognizer();

        StackLayout _summaryStackLayout = new StackLayout();
        List<string> _summaryListItems = new List<string>();
        Label _summaryLabel = new Label();

        ContentPage _navigationPage = null;


        public MainApp_Android()
        {
            // InitializeComponent();

            var screen = DependencyService.Get<UIScreen>();
            var padding = new Thickness(20, Device.OnPlatform(40, 40, 0), 10, 10);

            SetFontSize();

            SetupBlueTooth();

            DisplayListOfBluetoothDevices();

            SetTapGestures();

            //Common Lables
            Label Spacer = new Label();
            Spacer.FontSize = _fontSizeMicro;
            Spacer.HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start;
            Spacer.Text = " ";

            #region ServerPage
            var ServerPage = new ContentPage();
            var Content = new StackLayout();
            Content.VerticalOptions = LayoutOptions.Start;
            // Label
            _testLabel = new Label();
            _testLabel.FontSize = 24;
            _testLabel.FontAttributes = FontAttributes.Bold;
            _testLabel.HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start;
            _testLabel.Text = "Bluetooth setup :";
            _testLabel.GestureRecognizers.Add(_tapConnect);
            //ListView
            var listView = new Xamarin.Forms.ListView();
            listView.ItemsSource = _bluetoothDevices;
            listView.ItemSelected += OnSelectedBluetoothDevice;

            _startServerButton.Text = "Start Server";
            _startServerButton.Clicked += StartServer_Clicked;

            Content.Children.Add(_testLabel);
            Content.Children.Add(listView);
            Content.Children.Add(_startServerButton);
            ServerPage.Content = Content;
            ServerPage.AnchorX = 0;
            #endregion

            #region Gartner Label
            var Circle = new ContentPage
            {
                //Padding = padding,
                Title = "Circle",
                Content = new StackLayout
                {
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                    Children = {
                        new Image {
                            Aspect = Aspect.Fill,
                            Source = ImageSource.FromFile("circle.png"),
                            WidthRequest = 675,
                            HeightRequest = 500
                        }
                    }
                }
            };
            Circle.AnchorX = 1;
            #endregion

            #region Gartner Basic Info
            var ScalePage = new ContentPage();
            var ContentScalePage = new StackLayout();
            ContentScalePage.VerticalOptions = LayoutOptions.Start;

            // Title
            Label SP1 = new Label();
            SP1.FontSize = _fontSizeMed;
            SP1.FontAttributes = FontAttributes.Italic;
            SP1.HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start;
            SP1.TextColor = Color.Teal;
            SP1.BackgroundColor = Color.Silver;
            SP1.Text = "  Gartner";

            // Blank Line
            Label SP2 = new Label();
            SP2.FontSize = _fontSizeSmall;
            SP2.HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start;
            SP2.Text = " ";

            // World Leading...
            Label SP3 = new Label();
            SP3.FontSize = _fontSizeLarge;
            SP3.HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start;

            FormattedString fpScale1 = new FormattedString();
            Span fpScale1_Span1 = new Span();
            fpScale1_Span1.Text = "       - ";
            fpScale1_Span1.FontSize = _fontSizeLarge;

            Span fpScale1_Span2 = new Span();
            fpScale1_Span2.Text = "World leading tech research and advising company.";
            fpScale1_Span2.FontSize = _fontSizeLarge;
            fpScale1_Span2.FontAttributes = FontAttributes.Bold;

            fpScale1.Spans.Add(fpScale1_Span1);
            fpScale1.Spans.Add(fpScale1_Span2);
            SP3.FormattedText = fpScale1;

            // Blank Line
            Label SP4 = new Label();
            SP4.FontSize = _fontSizeMicro;
            SP4.HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start;
            SP4.Text = " ";

            // Help people...
            Label SP5 = new Label();
            SP5.FontSize = _fontSizeLarge;
            SP5.HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start;

            FormattedString fpScale2 = new FormattedString();
            Span fpScale1_Span3 = new Span();
            fpScale1_Span3.Text = "       - ";
            fpScale1_Span3.FontSize = _fontSizeLarge;

            Span fpScale1_Span4 = new Span();
            fpScale1_Span4.Text = "Help clients select right technologies, using the right vendors at the right time.";
            fpScale1_Span4.FontSize = _fontSizeLarge;
            fpScale1_Span4.FontAttributes = FontAttributes.Bold;

            fpScale2.Spans.Add(fpScale1_Span3);
            fpScale2.Spans.Add(fpScale1_Span4);
            SP5.FormattedText = fpScale2;

            // Blank Line
            Label SP6 = new Label();
            SP6.FontSize = _fontSizeMicro;
            SP6.HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start;
            SP6.Text = " ";

            // Over 11,000...
            Label SP7 = new Label();
            SP7.FontSize = _fontSizeLarge;
            SP7.HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start;

            FormattedString fpScale3 = new FormattedString();
            Span fpScale1_Span5 = new Span();
            fpScale1_Span5.Text = "       - ";
            fpScale1_Span5.FontSize = _fontSizeLarge;

            Span fpScale1_Span6 = new Span();
            fpScale1_Span6.Text = "Over 11,000 client companies w/ 87% retention rate!";
            fpScale1_Span6.FontSize = _fontSizeLarge;
            fpScale1_Span6.FontAttributes = FontAttributes.Bold;

            fpScale3.Spans.Add(fpScale1_Span5);
            fpScale3.Spans.Add(fpScale1_Span6);
            SP7.FormattedText = fpScale3;


            ContentScalePage.Children.Add(SP1);
            ContentScalePage.Children.Add(SP2);
            ContentScalePage.Children.Add(SP3);
            ContentScalePage.Children.Add(SP4);
            ContentScalePage.Children.Add(SP5);
            ContentScalePage.Children.Add(SP6);
            ContentScalePage.Children.Add(SP7);


            ScalePage.Content = ContentScalePage;
            ScalePage.AnchorX = 2;
            #endregion 

            #region Gartner For CIO's ROI
            var GartnerROIInfo = new ContentPage();
            var ContentGartnerROIInfo = new StackLayout();
            ContentGartnerROIInfo.VerticalOptions = LayoutOptions.Start;

            Label PP1 = new Label();
            PP1.FontSize = _fontSizeMed;
            PP1.FontAttributes = FontAttributes.Italic;
            PP1.HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start;
            PP1.TextColor = Color.Teal;
            PP1.BackgroundColor = Color.Silver;
            PP1.Text = "  Gartner for CIO's - Hearth";

            Label PP2 = new Label();
            PP2.FontSize = _fontSizeSmall;
            PP2.HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start;
            PP2.Text = " ";

            Label PP3 = new Label();
            PP3.FontSize = _fontSizeLarge;            
            PP3.HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start;
            PP3.Text = "   Cost Optimization";
                        
            Label PP4 = new Label();
            PP4.FontSize = _fontSizeLarge;            
            PP4.HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start;
            PP4.Text = "   Research";

            Label PP5 = new Label();
            PP5.FontSize = _fontSizeLarge;
            PP5.HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start;
            PP5.Text = "   Executive Partner";

            Label SpacerPay2 = new Label();
            SpacerPay2.FontSize = _fontSizeMicro;
            SpacerPay2.HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start;
            SpacerPay2.Text = " ";

            Label PP6 = new Label();
            PP6.FontSize = _fontSizeMed;           
            PP6.HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start;
            PP6.Text = "       - Save 15% on IT Spend";

            Label PP7 = new Label();
            PP7.FontSize = _fontSizeMed;
            PP7.HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start;
            PP7.Text = "       - Hearth Reduce 1.3M on IT Spend - 1200% ROI";

            Label PP8 = new Label();
            PP8.FontSize = _fontSizeMed;
            PP8.HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start;
            PP8.Text = "       - Avoid Errors, Save Time, Better Outcomes";           

            Label PP9 = new Label();
            PP9.FontSize = _fontSizeLarge;
            PP9.HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start;
            PP9.Text = "       - Increase Confidence in Decisions";

            ContentGartnerROIInfo.Children.Add(PP1);
            ContentGartnerROIInfo.Children.Add(PP2);
            ContentGartnerROIInfo.Children.Add(PP3);
            ContentGartnerROIInfo.Children.Add(PP4);            
            ContentGartnerROIInfo.Children.Add(PP5);            
            ContentGartnerROIInfo.Children.Add(PP6);
            ContentGartnerROIInfo.Children.Add(SpacerPay2);
            ContentGartnerROIInfo.Children.Add(PP7);           
            ContentGartnerROIInfo.Children.Add(PP8);
            ContentGartnerROIInfo.Children.Add(PP9);
            GartnerROIInfo.Content = ContentGartnerROIInfo;
            GartnerROIInfo.AnchorX = 3;
            #endregion

            #region Gartner Selection
            var SelectionPage = new ContentPage();
            var ScrollView = new Xamarin.Forms.ScrollView();
            var mainGrid = new Grid();

            mainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });


            mainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            mainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });


            // Title
            Label SP0 = new Label();
            SP0.FontSize = _fontSizeMed;
            SP0.FontAttributes = FontAttributes.Italic;
            SP0.HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start;
            SP0.TextColor = Color.Teal;
            SP0.BackgroundColor = Color.Silver;
            SP0.Text = "  Gartner";


            //////////////////////////////////
            Label LabelPH1 = new Label();
            LabelPH1.FontSize = _fontSizeMed;
            LabelPH1.HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start;
            LabelPH1.Text = "Cost Optimization Tool";
            var PH1 = new Image();
            PH1.Aspect = Aspect.Fill;
            PH1.Source = ImageSource.FromFile("SMcostop.png");
            PH1.GestureRecognizers.Add(new TapGestureRecognizer(OnPH1));


            //////////////////////////////////
            Label LabelPH2 = new Label();
            LabelPH2.FontSize = _fontSizeMed;
            LabelPH2.HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start;
            LabelPH2.Text = "Magic Quadriant";
            var PH2 = new Image();
            PH2.Aspect = Aspect.Fill;
            PH2.Source = ImageSource.FromFile("SMmagicq.png");
            PH2.GestureRecognizers.Add(new TapGestureRecognizer(OnPH2));

            //////////////////////////////////
            Label LabelPH3 = new Label();
            LabelPH3.FontSize = _fontSizeMed;
            LabelPH3.HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start;
            LabelPH3.Text = "Hype Cycle";
            var PH3 = new Image();
            PH3.Aspect = Aspect.Fill;
            PH3.Source = ImageSource.FromFile("SMhype.png");
            PH3.GestureRecognizers.Add(new TapGestureRecognizer(OnPH3));


            //////////////////////////////////
            Label LabelPH4 = new Label();
            LabelPH4.FontSize = _fontSizeMed;
            LabelPH4.HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start;
            LabelPH4.Text = "Vendor Rating";
            var PH4 = new Image();
            PH4.Aspect = Aspect.Fill;
            PH4.Source = ImageSource.FromFile("SMvenrate.png");
            PH4.GestureRecognizers.Add(new TapGestureRecognizer(OnPH4));

            //////////////////////////////////
            Label LabelPH5 = new Label();
            LabelPH5.FontSize = _fontSizeMed;
            LabelPH5.HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start;
            LabelPH5.Text = "Critical Capabilities";
            var PH5 = new Image();
            PH5.Aspect = Aspect.Fill;
            PH5.Source = ImageSource.FromFile("SMcritcap.png");
            PH5.GestureRecognizers.Add(new TapGestureRecognizer(OnPH5));

            //////////////////////////////////
            Label LabelPH6 = new Label();
            LabelPH6.FontSize = _fontSizeMed;
            LabelPH6.HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start;
            LabelPH6.Text = "IT Scorecard";
            var PH6 = new Image();
            PH6.Aspect = Aspect.Fill;
            PH6.Source = ImageSource.FromFile("SMitscore.png");
            PH6.GestureRecognizers.Add(new TapGestureRecognizer(OnPH6));

            //////////////////////////////////
            Label LabelPH7 = new Label();
            LabelPH7.FontSize = _fontSizeMed;
            LabelPH7.HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start;
            LabelPH7.Text = "Market Clock";
            var PH7 = new Image();
            PH7.Aspect = Aspect.Fill;
            PH7.Source = ImageSource.FromFile("SMmarkclk.png");
            PH7.GestureRecognizers.Add(new TapGestureRecognizer(OnPH7));

            //////////////////////////////////
            Label LabelPH8 = new Label();
            LabelPH8.FontSize = _fontSizeMed;
            LabelPH8.HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start;
            LabelPH8.Text = "Market Guide";
            var PH8 = new Image();
            PH8.Aspect = Aspect.Fill;
            PH8.Source = ImageSource.FromFile("SMmrktgid.png");
            PH8.GestureRecognizers.Add(new TapGestureRecognizer(OnPH8));


            mainGrid.Children.Add(SP0, 0, 0);

            mainGrid.Children.Add(LabelPH1, 0, 2);
            mainGrid.Children.Add(PH1, 0, 3);
            mainGrid.Children.Add(LabelPH2, 1, 2);
            mainGrid.Children.Add(PH2, 1, 3);

            mainGrid.Children.Add(LabelPH3, 0, 5);
            mainGrid.Children.Add(PH3, 0, 6);
            mainGrid.Children.Add(LabelPH4, 1, 5);
            mainGrid.Children.Add(PH4, 1, 6);

            mainGrid.Children.Add(LabelPH5, 0, 8);
            mainGrid.Children.Add(PH5, 0, 9);
            mainGrid.Children.Add(LabelPH6, 1, 8);
            mainGrid.Children.Add(PH6, 1, 9);

            mainGrid.Children.Add(LabelPH7, 0, 11);
            mainGrid.Children.Add(PH7, 0, 12);
            mainGrid.Children.Add(LabelPH8, 1, 11);
            mainGrid.Children.Add(PH8, 1, 12);



            ScrollView.Content = mainGrid;
            SelectionPage.Content = ScrollView;
            SelectionPage.AnchorX = 4;
            _navigationPage = SelectionPage;
            #endregion 

            #region Cost Optimization.png
            var CostOptimization = new ContentPage()
            {
                //Padding = padding,
                Title = "CostOptimization",
                Content = new StackLayout
                {
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                    Children = {
                        new Label {
                            FontSize = _fontSizeMed,
                            HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start,
                            TextColor = Color.Teal,
                            BackgroundColor = Color.Silver,
                            Text = "  Cost Optimization Tool"
                        },
                        new Image {
                            Aspect = Aspect.Fill,
                            Source = ImageSource.FromFile("costop.png"),
                            WidthRequest = 675,
                            HeightRequest = 449
                        }
                    }
                }
            };
            CostOptimization.AnchorX = 5;
          

            #endregion


            #region MagicQuad
            var MagicQuad = new ContentPage()
            {
                //Padding = padding,
                Title = "MagicQuad",
                Content = new StackLayout
                {
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                    Children = { new Label {
                            FontSize = _fontSizeMed,
                            HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start,
                            TextColor = Color.Teal,
                            BackgroundColor = Color.Silver,
                            Text = "  Magic Quadrant (Who - Key Players)"
                        },
                        new Image {
                            Aspect = Aspect.Fill,
                            Source = ImageSource.FromFile("magicq.png"),
                            WidthRequest = 675,
                            HeightRequest = 449
                        }
                    }
                }
            };
            MagicQuad.AnchorX = 6;


            #endregion

            #region Hype Cycle
            var HypeCycle = new ContentPage()
            {
                //Padding = padding,
                Title = "HypeCycle",
                Content = new StackLayout
                {
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                    Children = { new Label {
                            FontSize = _fontSizeMed,
                            HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start,
                            TextColor = Color.Teal,
                            BackgroundColor = Color.Silver,
                            Text = "  Hype Cycle (When - Maturity)"
                        },
                        new Image {
                            Aspect = Aspect.Fill,
                            Source = ImageSource.FromFile("hype.png"),
                            WidthRequest = 675,
                            HeightRequest = 449
                        }
                    }
                }
            };
            HypeCycle.AnchorX = 7;

            #endregion

            #region VendorRate
            var VendorRate = new ContentPage()
            {
                //Padding = padding,
                Title = "VendorRate",
                Content = new StackLayout
                {
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                    Children = { new Label {
                            FontSize = _fontSizeMed,
                            HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start,
                            TextColor = Color.Teal,
                            BackgroundColor = Color.Silver,
                            Text = "  Vendor Rating (Price, Service, Financials)"
                        },
                        new Image {
                            Aspect = Aspect.Fill,
                            Source = ImageSource.FromFile("venrate.png"),
                            WidthRequest = 675,
                            HeightRequest = 449
                        }
                    }
                }
            };
            VendorRate.AnchorX = 8;
            #endregion

            #region Crit capabilities
            var CritCap = new ContentPage()
            {
                //Padding = padding,
                Title = "CritCap",
                Content = new StackLayout
                {
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                    Children = { new Label {
                            FontSize = _fontSizeMed,
                            HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start,
                            TextColor = Color.Teal,
                            BackgroundColor = Color.Silver,
                            Text = "  Critical Capabilities (What - Deep Dive)"
                        },
                        new Image {
                            Aspect = Aspect.Fill,
                            Source = ImageSource.FromFile("critcap.png"),
                            WidthRequest = 675,
                            HeightRequest = 449
                        }
                    }
                }
            };
            CritCap.AnchorX = 9;
            #endregion
            
            #region IT Score Card
            var ITScore = new ContentPage()
            {
                //Padding = padding,
                Title = "ITScore",
                Content = new StackLayout
                {
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                    Children = { new Label {
                            FontSize = _fontSizeMed,
                            HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start,
                            TextColor = Color.Teal,
                            BackgroundColor = Color.Silver,
                            Text = "  IT Score Card (IT & Enterprise)"
                        },
                        new Image {
                            Aspect = Aspect.Fill,
                            Source = ImageSource.FromFile("itscore.png"),
                            WidthRequest = 675,
                            HeightRequest = 449
                        }
                    }
                }
            };
            ITScore.AnchorX = 10;
            #endregion
            
            #region Market Clock
            var MarketClock = new ContentPage()
            {
                //Padding = padding,
                Title = "MarketClock",
                Content = new StackLayout
                {
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                    Children = { new Label {
                            FontSize = _fontSizeMed,
                            HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start,
                            TextColor = Color.Teal,
                            BackgroundColor = Color.Silver,
                            Text = "  Market Clock (Updating - Obsolescence)"
                        },
                        new Image {
                            Aspect = Aspect.Fill,
                            Source = ImageSource.FromFile("markclk.png"),
                            WidthRequest = 675,
                            HeightRequest = 449
                        }
                    }
                }
            };
            MarketClock.AnchorX = 11;
            #endregion
            
            #region Market Guide
            var MarketGuide = new ContentPage()
            {
                //Padding = padding,
                Title = "MarketGuide",
                Content = new StackLayout
                {
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                    Children = { new Label {
                            FontSize = _fontSizeMed,
                            HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start,
                            TextColor = Color.Teal,
                            BackgroundColor = Color.Silver,
                            Text = "  Market Guide (Edge Players)"
                        },
                        new Image {
                            Aspect = Aspect.Fill,
                            Source = ImageSource.FromFile("mrktgid.png"),
                            WidthRequest = 675,
                            HeightRequest = 449
                        }
                    }
                }
            };
            MarketGuide.AnchorX = 12;
            #endregion
                          

            #region Summary 
            //var SummaryPage = new ContentPage();

            //var _summaryLabelHeader = new Label();
            //_summaryLabelHeader.FontSize = _fontSizeMed;
            //_summaryLabelHeader.FontAttributes = FontAttributes.Italic;
            //_summaryLabelHeader.BackgroundColor = Color.Silver;
            //_summaryLabelHeader.TextColor = Color.Teal;
            //_summaryLabelHeader.HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start;
            //_summaryLabelHeader.Text = "  ADP RUN - Solution for Stuart Surveying";

            //Label SH3 = new Label();
            //SH3.FontSize = _fontSizeSmall;
            //SH3.HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start;
            //SH3.Text = " ";

            //var _summaryLabelHeader2 = new Label();
            //_summaryLabelHeader2.FontSize = _fontSizeLarge;
            //_summaryLabelHeader2.HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start;
            //_summaryLabelHeader2.Text = "   RUN can help with...";

            //Label SummarySpacer = new Label();
            //SummarySpacer.FontSize = _fontSizeMicro;
            //SummarySpacer.HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start;
            //SummarySpacer.Text = " ";

            //_summaryLabel.FontSize = _fontSizeMed;

            //_summaryStackLayout.Children.Add(_summaryLabelHeader);
            //_summaryStackLayout.Children.Add(SH3);
            //_summaryStackLayout.Children.Add(_summaryLabelHeader2);
            //_summaryStackLayout.Children.Add(SummarySpacer);



            //_summaryStackLayout.Children.Add(_summaryLabel);

            //SummaryPage.Content = _summaryStackLayout;
            //SummaryPage.AnchorX = 7;
            #endregion


            _carouselPage = new CarouselPage();            
            _carouselPage.Children.Add(ServerPage);
            _carouselPage.Children.Add(Circle);
            _carouselPage.Children.Add(ScalePage);
            _carouselPage.Children.Add(GartnerROIInfo);

            _carouselPage.Children.Add(SelectionPage);            

            // Images
            _carouselPage.Children.Add(CostOptimization);
            _carouselPage.Children.Add(MagicQuad);
            _carouselPage.Children.Add(HypeCycle);
            _carouselPage.Children.Add(VendorRate);
            _carouselPage.Children.Add(CritCap);
            _carouselPage.Children.Add(ITScore);
            _carouselPage.Children.Add(MarketClock);
            _carouselPage.Children.Add(MarketGuide);


            MainPage = _carouselPage;
            _carouselPage.PropertyChanged += _carouselPage_PropertyChanged;            
        }

        private void ImageSelect(int pageNum)
        {
            byte[] array = new byte[3];
            array[0] = (byte)'n';
            array[1] = (byte)'p';
            array[2] = (byte)pageNum;
            // Send new page info
            if (_bluetoothSocket != null)
                _bluetoothSocket.OutputStream.WriteAsync(array, 0, array.Length);

            _carouselPage.CurrentPage = _carouselPage.Children[pageNum];

        }

        private void OnPH1(Xamarin.Forms.View arg1, object arg2)
        {
            ImageSelect(5); 
        }

        private void OnPH2(Xamarin.Forms.View arg1, object arg2)
        {
            ImageSelect(6);
        }

        private void OnPH3(Xamarin.Forms.View arg1, object arg2)
        {
            ImageSelect(7);
        }

        private void OnPH4(Xamarin.Forms.View arg1, object arg2)
        {
            ImageSelect(8);
        }

        private void OnPH5(Xamarin.Forms.View arg1, object arg2)
        {
            ImageSelect(9);
        }

        private void OnPH6(Xamarin.Forms.View arg1, object arg2)
        {
            ImageSelect(10);
        }

        private void OnPH7(Xamarin.Forms.View arg1, object arg2)
        {
            ImageSelect(11);
        }

        private void OnPH8(Xamarin.Forms.View arg1, object arg2)
        {
            ImageSelect(12);
        }


        private void SetFontSize()
        {
            if (Device.Idiom == TargetIdiom.Phone)
            {
                _fontSizeMicro = Device.GetNamedSize(NamedSize.Micro, typeof(Label));
                _fontSizeSmall = Device.GetNamedSize(NamedSize.Small, typeof(Label));
                _fontSizeMed = Device.GetNamedSize(NamedSize.Medium, typeof(Label)) + Device.GetNamedSize(NamedSize.Micro, typeof(Label)); ;
                _fontSizeLarge = Device.GetNamedSize(NamedSize.Large, typeof(Label)) + Device.GetNamedSize(NamedSize.Micro, typeof(Label));
            }
            else
            {
                _fontSizeMicro = 10;
                _fontSizeSmall = 20;
                _fontSizeMed = 36;
                _fontSizeLarge = 48;
            }
        }

        private bool SetupBlueTooth()
        {
            _bluetoothAdapter = BluetoothAdapter.DefaultAdapter;

            // If the adapter is null, then Bluetooth is not supported 
            if (_bluetoothAdapter == null)
            {
                _testLabel.Text = "Bluetooth: Failed Adapter";
                return false;
            }

            _bluetoothAdapter.Enable();

            _myUUID = Java.Util.UUID.FromString("ec79da00-853f-11e4-b4a9-0800200c9a69");
            _myName = _myUUID.ToString();
            return true;
        }

        private void DisplayListOfBluetoothDevices()
        {
            foreach (BluetoothDevice device in _bluetoothAdapter.BondedDevices)
            {
                _bluetoothDevices.Add(new KeyValuePair<string, string>(device.Name, device.Address));
            }

            // If the adapter is null, then Bluetooth is not supported 
            if (_bluetoothDevices == null)
            {
                _testLabel.Text = "Bluetooth: Failed Device";
            }
            else
            {
                _testLabel.Text = "Bluetooth: Ready";
            }
        }

        // Setup server
        private void StartServer_Clicked(object sender, EventArgs e)
        {
            _clientMode = false; 

            if (_bluetoothAdapter != null)
            {
                _bluetoothServerSocket = _bluetoothAdapter.ListenUsingRfcommWithServiceRecord(_myName, _myUUID);

                _serverThread = new BluetoothServer(this,_bluetoothServerSocket, _carouselPage);

                ThreadStart WaitForConnectionFunc = new ThreadStart(_serverThread.WaitForConnection);
                System.Threading.Thread waitForConnectThread = new System.Threading.Thread(WaitForConnectionFunc);
                waitForConnectThread.IsBackground = true;
                waitForConnectThread.Start();

                ThreadStart myThreadDelegate = new ThreadStart(_serverThread.ListenForBTData);
                System.Threading.Thread myThread = new System.Threading.Thread(myThreadDelegate);
                myThread.IsBackground = true;
                myThread.Start();

                #region Gartner For CIO's
                var GartnerForCIOs = new ContentPage();
                var ContentGartnerForCIOs = new StackLayout();
                ContentGartnerForCIOs.VerticalOptions = LayoutOptions.Start;

                Label PP1 = new Label();
                PP1.FontSize = _fontSizeMed;
                PP1.FontAttributes = FontAttributes.Italic;
                PP1.HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start;
                PP1.TextColor = Color.Teal;
                PP1.BackgroundColor = Color.Silver;
                PP1.Text = "  Gartner for CIO's";

                Label PP2 = new Label();
                PP2.FontSize = _fontSizeSmall;
                PP2.HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start;
                PP2.Text = " ";

                Label PP3 = new Label();
                PP3.FontSize = _fontSizeLarge;
                PP3.HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start;
                PP3.Text = "   Cost Optimization Tools";

                Label SpacerPay2 = new Label();
                SpacerPay2.FontSize = _fontSizeMicro;
                SpacerPay2.HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start;
                SpacerPay2.Text = " ";

                Label PP4 = new Label();
                PP4.FontSize = _fontSizeLarge;
                PP4.HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start;
                PP4.Text = "   Contract Review Service";

                Label SpacerPay3 = new Label();
                SpacerPay3.FontSize = _fontSizeMicro;
                SpacerPay3.HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start;
                SpacerPay3.Text = " ";

                Label PP5 = new Label();
                PP5.FontSize = _fontSizeLarge;
                PP5.HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start;
                PP5.Text = "   Research Tools";

                Label SpacerPay4 = new Label();
                SpacerPay4.FontSize = _fontSizeMicro;
                SpacerPay4.HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start;
                SpacerPay4.Text = " ";

                Label PP6 = new Label();
                PP6.FontSize = _fontSizeLarge;
                PP6.HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start;
                PP6.Text = "   Executive Partner";

                ContentGartnerForCIOs.Children.Add(PP1);
                ContentGartnerForCIOs.Children.Add(PP2);
                ContentGartnerForCIOs.Children.Add(PP3);
                ContentGartnerForCIOs.Children.Add(SpacerPay2);
                ContentGartnerForCIOs.Children.Add(PP4);
                ContentGartnerForCIOs.Children.Add(SpacerPay3);
                ContentGartnerForCIOs.Children.Add(PP5);
                ContentGartnerForCIOs.Children.Add(SpacerPay4);
                ContentGartnerForCIOs.Children.Add(PP6);
                GartnerForCIOs.Content = ContentGartnerForCIOs;
                GartnerForCIOs.AnchorX = 4;

                int indexNavPage = _carouselPage.Children.IndexOf(_navigationPage);
                _carouselPage.Children.Remove(_navigationPage);
                _carouselPage.Children.Insert(indexNavPage, GartnerForCIOs);                
                #endregion
            }
        }

        private void OnSelectedBluetoothDevice(object sender, SelectedItemChangedEventArgs e)
        {
            _clientMode = true;

            KeyValuePair<string, string> selectedBTDevice = (KeyValuePair<string, string>)(((Xamarin.Forms.ListView)sender).SelectedItem);

            ParcelUuid[] guids = null;
            if (_bluetoothAdapter != null)
                _bluetoothAdapter.Enable();

            if (selectedBTDevice.Value != null && _bluetoothAdapter != null)
            {
                _bluetoothDevice = _bluetoothAdapter.GetRemoteDevice(selectedBTDevice.Value);
                guids = _bluetoothDevice.GetUuids();
                _state = STATE_CONNECTED;
            }

            System.Console.WriteLine(guids[3].Uuid);

            //foreach(ParcelUuid uuid in guids)
            //{
            //    System.Console.WriteLine(uuid.Uuid);
            //}
            if (_bluetoothDevice != null)
            {
                _bluetoothSocket = _bluetoothDevice.CreateRfcommSocketToServiceRecord(_myUUID);

                if (_bluetoothSocket != null)
                {
                    _bluetoothSocket.Connect();
                    _outStream = _bluetoothSocket.OutputStream;
                    _outStreamWriter = new OutputStreamWriter(_outStream);
                    _writeBuffer = new BufferedWriter(_outStreamWriter);

                    _testLabel.Text = "Bluetooth: Connected";
                }
                else
                {
                    _testLabel.Text = "Bluetooth: Failed Socket";
                }
            }             
        }

        private void _carouselPage_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            CarouselPage cp = (CarouselPage)sender;
            bool HomePage = false;

            int CurrentPageIndex = (int)cp.CurrentPage.AnchorX;

            if (_clientMode)
            {
                // if the page changed, send the info to other devices
                if (CurrentPageIndex == _previousPageIndex)
                {
                    // do nothing
                }                
                else
                {
                    //string message = "np" + NewPageIndex.ToString();
                   // byte[] array = Encoding.ASCII.GetBytes(message);

                    // _writeBuffer.Write(message);

                    byte[] array = new byte[3];
                    array[0] = (byte)'n';
                    array[1] = (byte)'p';

                    if (_previousPageIndex > 4)
                    {
                        CurrentPageIndex = 4;
                        HomePage = true;                       
                    }
                                       
                    array[2] = (byte)CurrentPageIndex;
                    _previousPageIndex = CurrentPageIndex;

                    // Send new page info
                    if (_bluetoothSocket != null)
                        _bluetoothSocket.OutputStream.WriteAsync(array, 0, array.Length);

                    if (HomePage)
                    {
                        _carouselPage.CurrentPage = _carouselPage.Children[4];
                    }
                }    
            }   
        }

        private void SetTapGestures()
        {
            _tapConnect = new TapGestureRecognizer();
            _tapConnect.Tapped += (sender, e) =>
            {
                ((Label)sender).BackgroundColor = Color.Red;
                ((Label)sender).FontAttributes = FontAttributes.Italic;
            };

            _tapLabel = new TapGestureRecognizer();
            _tapLabel.Tapped += (sender, e) =>
            {
                if (_clientMode)
                {
                    ((Label)sender).BackgroundColor = Color.Lime;

                    
                    foreach(KeyValuePair<string,string> kvp in _labelCollectionSummaryText)
                    {
                        if(kvp.Key.Equals( ((Label)sender).Text) )
                        {
                            _summaryListItems.Add(kvp.Value);
                            SetSummaryLabelText();
                        }

                    }

                    //if (!_summaryListItems.Contains<string>(((Label)sender).Text))
                    //{
                    //    _summaryListItems.Add(((Label)sender).Text);
                    //    SetSummaryLabelText();                       
                    //}

                    bool foundItem = false;
                    string CharsToSend = "";

                    foreach (KeyValuePair<string,Label> item in _labelCollection)
                    {
                        if(item.Value.Text == ((Label)sender).Text)
                        {
                            CharsToSend = item.Key;
                            foundItem = true;
                            break;
                        }
                    }

                    if(foundItem && (CharsToSend.Length == 3))
                    {      
                        byte[] array = new byte[3];
                        array[0] = (byte)CharsToSend[0];
                        array[1] = (byte)CharsToSend[1];
                        array[2] = (byte)CharsToSend[2];
                        // Send new page info
                        if (_bluetoothSocket != null)
                            _bluetoothSocket.OutputStream.WriteAsync(array, 0, array.Length);
                    }
                }
            };
        }

        private void SetSummaryLabelText()
        {
            var fs = new FormattedString();
            bool LineBreak = false;

            foreach (string s in _summaryListItems)
            {
                if(!LineBreak)
                {
                    LineBreak = true;
                    Span tempSpan = new Span();
                    tempSpan.FontFamily = "monospace";
                    tempSpan.Text = "   " + s + "   ";
                    tempSpan.FontSize = 32;
                    tempSpan.ForegroundColor = Color.Black;
                    fs.Spans.Add(tempSpan);                  
                }
                else
                {
                    LineBreak = false;
                    Span tempSpan = new Span();
                    tempSpan.FontFamily = "monospace";
                    tempSpan.Text = s + "\n";
                    tempSpan.ForegroundColor = Color.Black;
                    tempSpan.FontSize = 32;
                    fs.Spans.Add(tempSpan);                  
                }
            }

            _summaryLabel.FormattedText = fs;
        }

        public void UpdateLabel(string LabelID)
        { 
            foreach (KeyValuePair<string,Label> item in _labelCollection)
            {
                if(item.Key.Equals(LabelID))
                {
                    item.Value.BackgroundColor = Color.Lime;

                    foreach (KeyValuePair<string, string> kvp in _labelCollectionSummaryText)
                    {
                        if (kvp.Key.Equals(item.Value.Text))
                        {
                            _summaryListItems.Add(kvp.Value);
                            SetSummaryLabelText();
                        }

                    }

                    //if (!_summaryListItems.Contains<string>(item.Value.Text))
                    //{
                    //    _summaryListItems.Add(item.Value.Text);
                    //    SetSummaryLabelText();
                    //}
                }
            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}