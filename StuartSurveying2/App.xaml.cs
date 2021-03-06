﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace StuartSurveying2
{
  	public partial class App : Application
	{
        public App()
        {
            InitializeComponent();

            var padding = new Thickness(20, Device.OnPlatform(40, 40, 0), 10, 10);

            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (sender, e) =>
            {
                ((Label)sender).BackgroundColor = Color.Red;
                ((Label)sender).FontAttributes = FontAttributes.Italic;
            };
            
            var Circle = new ContentPage
            {                
                Padding = padding,
                Title = "Who / What ADP Does",
                Content = new StackLayout
                {
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                    Children = {
                        new Image {
                            Aspect = Aspect.Fill,
                            Source = ImageSource.FromFile("circle.png"),
                            WidthRequest = 800,
                            HeightRequest = 500                         
                        }                      
                    }
                }
            };

            var testPage = new ContentPage();

            var Content = new StackLayout();
            Content.VerticalOptions = LayoutOptions.Start;

            var testLabel = new Label();
            testLabel.FontSize = 24;
            testLabel.FontAttributes = FontAttributes.Bold;
            testLabel.HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start;
            testLabel.Text = "Payroll Problems";
            testLabel.GestureRecognizers.Add(tapGestureRecognizer);
            Content.Children.Add(testLabel);
            testPage.Content = Content;


             var Problems = new ContentPage
            {
                Padding = padding,
                Title = "Problems And Concerns",
                Content = new StackLayout
                {
                    VerticalOptions = LayoutOptions.Start,
                    Children = {
                        new Label {
                            FontSize = 24,
                            FontAttributes = FontAttributes.Bold,
                            HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start,
                            Text = "Payroll Problems"                            
                        },
                        new Label {
                            FontSize = 20,
                            HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start,
                            Text = "Manual Process"
                        },
                        new Label {
                            FontSize = 16,
                            HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start,
                            Text = "Errors: Under / Over Paid Employees, Improper tax withholdings."
                        },
                         new Label {
                            FontSize = 16,
                            HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start,
                            Text = "Time Loss."
                        },
                          new Label {
                            FontSize = 20,
                            HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start,
                            Text = "Fraud on time reoprting."
                        },
                           new Label {
                            FontSize = 20,
                            HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start,
                            Text = "Lack of real time reporting capabilities"
                        },
                           new Label {
                            FontSize = 20,
                            HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start,
                            Text = "Locatin Based -- processed in office, punch in tied to office"
                        },
                           new Label {
                            FontSize = 20,
                            HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start,
                            Text = " "
                        },
                           new Label {
                            FontSize = 24,
                            FontAttributes = FontAttributes.Bold,
                            HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start,
                            Text = "HR Problems"
                        },
                           new Label {
                            FontSize = 20,
                            HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start,
                            Text = "New Recruits - background checks, recruting methods"
                        }
                           ,
                           new Label {
                            FontSize = 20,
                            HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start,
                            Text = "HR policies or handbooks, Laws like posters."
                        }
                    }
                }
            };

            var Conserns = new ContentPage
            {
                Padding = padding,
                Title = "Problems And Concerns",
                Content = new StackLayout
                {
                    VerticalOptions = LayoutOptions.Start,
                    Children = {
                        new Label {
                            FontSize = 24,
                            FontAttributes = FontAttributes.Bold,
                            HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start,
                            Text = "Objections"
                        },
                        new Label {
                            FontSize = 16,
                            HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start,
                            Text = "Time: Not now, tied up with huge project, new location, to long to implement."
                        },
                         new Label {
                            FontSize = 16,
                            HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start,
                            Text = "Customer Service: To young, too much risk of poor service."
                        },
                          new Label {
                            FontSize = 16,
                            HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start,
                            Text = "Size: ADP is to huge, we are too small for this."
                        },
                           new Label {
                            FontSize = 16,
                            HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start,
                            Text = "Cost: Looks expensive , long term contract."
                        },
                           new Label {
                            FontSize = 16,
                            HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start,
                            Text = "Security of sensitive info."
                        },
                           new Label {
                            FontSize = 16,
                            FontAttributes = FontAttributes.Bold,
                            HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start,
                            Text = "We are not technical."
                        },
                           new Label {
                            FontSize = 16,
                            HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start,
                            Text = "Do you have references?"
                        }
                    }
                }
            };

            // The root page of your application
            var Rapport = new ContentPage
            {
                Padding = padding,
                Title = "Problems And Concerns",
                Content = new StackLayout
                {
                    VerticalOptions = LayoutOptions.Start,
                    Children = {
                        new Label {
                            FontSize = 24,
                            FontAttributes = FontAttributes.Bold,
                            HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start,
                            Text = "Step 1"
                        },
                        new Label {
                            FontSize = 16,
                            HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start,
                            Text = "Rapport"
                        },
                        new Label {
                            FontSize = 16,
                            HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start,
                            Text = "Comment on the warm home feel of the loby decore"
                        }
                    }
                }
            };

            var Agenda = new ContentPage
            {
                Padding = padding,
                Title = "Problems And Concerns",
                Content = new StackLayout
                {
                    VerticalOptions = LayoutOptions.Start,
                    Children = {
                        new Label {
                            FontSize = 24,
                            FontAttributes = FontAttributes.Bold,
                            HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start,
                            Text = "Step 2"
                        },
                        new Label {
                            FontSize = 16,
                            HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start,
                            Text = "Agenda"
                        },
                        new Label {
                            FontSize = 16,
                            HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start,
                            Text = "talk thru it or show it on paper"
                        }
                    }
                }
            };

            var NewVendor = new ContentPage
            {
                Padding = padding,
                Title = "Problems And Concerns",
                Content = new StackLayout
                {
                    VerticalOptions = LayoutOptions.Start,
                    Children = {
                        new Label {
                            FontSize = 24,
                            FontAttributes = FontAttributes.Bold,
                            HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start,
                            Text = "Step 3"
                        },
                        new Label {
                            FontSize = 16,
                            HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start,
                            Text = "New Vendor Process"
                        },
                        new Label {
                            FontSize = 16,
                            HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start,
                            Text = "Who are the decision makers? Are they all here?"
                        }
                    }
                }
            };

            var AboutBusiness = new ContentPage
            {
                Padding = padding,
                Title = "Problems And Concerns",
                Content = new StackLayout
                {
                    VerticalOptions = LayoutOptions.Start,
                    Children = {
                        new Label {
                            FontSize = 24,
                            FontAttributes = FontAttributes.Bold,
                            HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start,
                            Text = "Step 4"
                        },
                        new Label {
                            FontSize = 16,
                            HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start,
                            Text = "Business Details"
                        },
                        new Label {
                            FontSize = 16,
                            HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start,
                            Text = "Talk to me about your business, where do you see it going ?"
                        }
                    }
                }
            };

            var PayrollProcess = new ContentPage
            {
                Padding = padding,
                Title = "Problems And Concerns",
                Content = new ScrollView()
                {
                    Content = new StackLayout
                    {
                        VerticalOptions = LayoutOptions.Start,
                        Children =
                        {
                            new Label {
                                FontSize = 24,
                                FontAttributes = FontAttributes.Bold,
                                HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start,
                                Text = "Step 5"
                            },
                            new Label {
                                FontSize = 16,
                                HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start,
                                Text = "Payroll Process and HR Process"
                            },
                            new Label {
                                FontSize = 16,
                                HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start,
                                Text = "Walk me thru your payroll process. Drill Down into problems - PROBE with questions"
                            },
                            new Label {
                                FontSize = 16,
                                HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start,
                                Text = "Describe to me a time you needed to process but cound't get to a computer."
                            },
                             new Label {
                                FontSize = 16,
                                HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start,
                                Text = "How do you get payroll processed?"
                            },
                            new Label {
                                FontSize = 16,
                                HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start,
                                Text = "How much time did that take you?"
                            },
                             new Label {
                                FontSize = 16,
                                HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start,
                                Text = "What else could you have been doing with that time?"
                            },
                             new Label {
                                FontSize = 16,
                                HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start,
                                Text = "If ADP could provide you with a platform that would prevent having to sit at a computer or dial a phone number to process your payroll each pay period, would that be a benefit to you?"
                            },
                              new Label {
                                FontSize = 16,
                                HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start,
                                Text = " "
                            },
                               new Label {
                                FontSize = 16,
                                HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start,
                                Text = " "
                            },
                            new Label {
                                FontSize = 16,
                                HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start,
                                Text = "Tell me about a time you needed to access your payroll reports or payroll data while on the road."
                            },
                            new Label {
                                FontSize = 16,
                                HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start,
                                Text = "How did you get the reports?"
                            },
                            new Label {
                                FontSize = 16,
                                HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start,
                                Text = "How much time did that take?"
                            },
                            new Label {
                                FontSize = 16,
                                HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start,
                                Text = "What else could you have done with that time?"
                            },
                              new Label {
                                FontSize = 16,
                                HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start,
                                Text = " "
                            },
                            new Label {
                                FontSize = 16,
                                HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start,
                                Text = "Do you always have your phone on you?"
                            }
                        }
                    }
                }
            };

            var SetNextMeeting = new ContentPage
            {
                Padding = padding,
                Title = "Problems And Concerns",
                Content = new StackLayout
                {
                    VerticalOptions = LayoutOptions.Start,
                    Children = {
                        new Label {
                            FontSize = 24,
                            FontAttributes = FontAttributes.Bold,
                            HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start,
                            Text = "Step 6"
                        },
                        new Label {
                            FontSize = 16,
                            HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start,
                            Text = "Set Next Meeting respond to Concerns"
                        },
                        new Label {
                            FontSize = 16,
                            HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Start,
                            Text = "Try setting up next meeting to closing the deal. Respond to Conerns"
                        }
                    }
                }
            };

            CarouselPage temp = new CarouselPage();
            temp.Children.Add(Circle);
            temp.Children.Add(testPage);
            temp.Children.Add(Problems);
            temp.Children.Add(Conserns);
            temp.Children.Add(Rapport);
            temp.Children.Add(Agenda);
            temp.Children.Add(NewVendor);
            temp.Children.Add(AboutBusiness);
            temp.Children.Add(PayrollProcess);
            temp.Children.Add(SetNextMeeting);

            MainPage = temp;
        }

       
        protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
