using Foundation;
using System;
using UIKit;
using Syncfusion.SfNavigationDrawer.iOS;
using CoreGraphics;
using SolitaireiOS.Lang;

namespace SolitaireiOS
{
    public partial class ViewController : UIViewController
    {
        public object FrameLayout { get; private set; }
        // UITableView tableView;

        public ViewController(IntPtr handle) : base(handle) {

            AssetManager.InitBoards();
        }
       

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            //Register Syncfusion license
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTcwMjM0QDMxMzcyZTMzMmUzME5tUkxGQldvU2Voek1QQUJST2hzUmttb2pzZ1RJeHRVak1TTlRTajVGZnc9");

            // Perform any additional setup after loading the view, typically from a nib.
            SFNavigationDrawer navigation = new SFNavigationDrawer();
            this.Add(navigation);
            navigation.Frame = new CGRect(0, 40, this.View.Frame.Width, this.View.Frame.Height);
            navigation.DrawerHeaderHeight = 200;
            navigation.DrawerWidth = 200;

            // Acts like a android toolbar
            var toolbar = new UIView()
            {
                Frame = new CGRect(0, 0, View.Bounds.Width, 80),
                BackgroundColor = UIColor.FromRGB(139, 166, 244)
            };
            var addBoardBtn = new UIButton()
            {
                Frame = new CGRect(View.Bounds.Width - 130, 40, 150, 40)
            };
            addBoardBtn.SetTitle("Add Board", UIControlState.Normal);
            addBoardBtn.SetTitleColor(UIColor.White, UIControlState.Normal);
            addBoardBtn.TouchUpInside += delegate {
                View.AddSubview(Dialog.MakeDialog(nameof(Board), this, Dialog.CreateSolitiareType.CreateBoard));
	        };
            toolbar.AddSubview(addBoardBtn);


            // Home menu for toggling the navigation
            UIButton menubutton = new UIButton();
            menubutton.Frame = new CGRect(10, 45, 25, 25);
            menubutton.SetBackgroundImage(new UIImage("Images/menu.png"), UIControlState.Normal);

            // When the home button is pressed we will toggle the navigation
            menubutton.TouchUpInside += (object sender, EventArgs e) =>
            {
                navigation.ToggleDrawer();
            };
            toolbar.AddSubview(menubutton);

            // TODO: Make another UIVIEW that will act like the framelayout for the "fragments"

            // Main view that acts as the FrameLayout for fragments
            UIView frameLayout = new UIView(new CGRect(0, toolbar.Bounds.Height, this.View.Frame.Width, this.View.Frame.Height));
            frameLayout.BackgroundColor = UIColor.White;
            // Creating the listview

            // UITableView is not populating the screen with data
            UITableView boardTableView = new UITableView(frameLayout.Bounds);
            boardTableView.Source = new BoardDataSource(this);

            frameLayout.AddSubview(boardTableView);
            // Attaching the home button to the navigation
            // frameLayout.AddSubview(toolbar);
            // Attaching the contentview to the navigation ContentView (toolbar)
            UIView MainUIContainer = new UIView(new CGRect(0, 0,View.Bounds.Width, View.Bounds.Height));
            MainUIContainer.AddSubviews(toolbar, frameLayout);
            navigation.ContentView = MainUIContainer;

            // Acts Acts like the frameLayout for the headerView
            UIView headerView = new UIView(new CGRect(0, 0, navigation.DrawerWidth, 0));

            // Acts like a container for the user's image and name
            UIView HeaderView = new UIView()
            {
                Frame = new CGRect(0, 70, navigation.DrawerWidth, 0),
                BackgroundColor = UIColor.White
            };
            // Shows the name to the user
            UILabel usernameLabel = new UILabel()
            {
                Frame = new CGRect(10, 80, navigation.DrawerWidth, 30),
                Text = (NSString)"Name",
                TextColor = UIColor.DarkTextColor,
                TextAlignment = UITextAlignment.Left
            };
            // Shows the name to the user
            UILabel userEmail = new UILabel()
            {
                Frame = new CGRect(10, 100, navigation.DrawerWidth, 30),
                Text = (NSString)"Email",
                TextColor = UIColor.Gray,
                TextAlignment = UITextAlignment.Left
            };

            // Adding the user's image and name label to the HeaderView Container UIVIEW
            HeaderView.AddSubview(new UIImageView(UIImage.FromBundle("Images/default_avatar.png"))
            {
                Frame = new CGRect((navigation.DrawerWidth / 2) - 25, 15, 50, 50)
            });
            HeaderView.AddSubview(usernameLabel);
            HeaderView.AddSubview(userEmail);

            // Adding the main header container to the headerView
            headerView.AddSubview(HeaderView);

            // Acts like the FrameLayout for the contents of the drawer layout
            UIView drawerContentView = new UIView(new CGRect(0, 0, navigation.DrawerWidth, 0));

            // Is the container for the contents of the drawer layout
            UIView contentContainer = new UIView(new CGRect(0, 0, navigation.DrawerWidth, View.Bounds.Height));
            contentContainer.BackgroundColor = UIColor.FromRGB(139, 166, 244);


            
         

            InitDrawerContents(contentContainer, navigation);

            
            drawerContentView.AddSubview(contentContainer);
            navigation.DrawerContentView = drawerContentView;
            navigation.DrawerHeaderView = headerView;
            this.Add(navigation);
        }

        /// 
        ///
        ///     Initializes the content for the drawer view
        /// 
        ///
        public void InitDrawerContents(UIView _contentContainer, SfNavigationDrawer _navigation)
        {
            UIButton homeButton = new UIButton(new CGRect(0, 0, _navigation.DrawerWidth, 50));
            homeButton.SetTitle("Boards", UIControlState.Normal);
            homeButton.SetTitleColor(UIColor.Black, UIControlState.Normal);
            homeButton.HorizontalAlignment = UIControlContentHorizontalAlignment.Center;
            _contentContainer.AddSubview(homeButton);

            UIButton profileButton = new UIButton(new CGRect(0, 50, _navigation.DrawerWidth, 50));
            profileButton.SetTitle("Contributors", UIControlState.Normal);
            profileButton.SetTitleColor(UIColor.Black, UIControlState.Normal);
            profileButton.HorizontalAlignment = UIControlContentHorizontalAlignment.Center;
            profileButton.Layer.CornerRadius = 0;
            profileButton.Layer.BorderColor = UIColor.FromRGB(0, 0, 0).CGColor;
            _contentContainer.AddSubview(profileButton);
        }
            
        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        public class BoardDataSource : UITableViewSource
        {
            ViewController parent;
            const string CellId = "BoardCell";

            public BoardDataSource(ViewController _parent)
            {
                this.parent = _parent;
            }

            public override nint RowsInSection(UITableView tableview, nint section)
            {
                return AssetManager.boards.Count;
            }

            public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
            {
                // Getting our view
                UITableViewCell cell = tableView.DequeueReusableCell(CellId);
                if (cell == null)
                {
                    cell = new UITableViewCell(UITableViewCellStyle.Subtitle, CellId);
                    cell.TextLabel.Font = UIFont.FromName("Helvetica Light", 14);
                    cell.DetailTextLabel.Font = UIFont.FromName("Helvetica Light", 12);
                    cell.DetailTextLabel.TextColor = UIColor.LightGray;
                    cell.Accessory = UITableViewCellAccessory.DetailDisclosureButton;

                }
                else if (cell.ImageView.Image != null)
                {
                    cell.ImageView.Image.Dispose();
                }

                var item = AssetManager.boards[indexPath.Row];

                cell.TextLabel.Text = item.Name;
                // Getting the default board image from file
                cell.ImageView.Image = UIImage.FromFile("Images/default_board");
                cell.DetailTextLabel.Text = item.Description;

                return cell;
            }

            ///
            /// 
            ///     Handles a board being clicked
            /// 
            /// 
            public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
            {
                //// base.RowSelected(tableView, indexPath);

                //// Create the View Controller in the Main storyboard.
                var storyboard = UIStoryboard.FromName("Main", null);
                
                var useBoardActivity = (UseBoardViewController)storyboard.InstantiateViewController("UseBoardViewController");
                
                parent.PresentViewController(useBoardActivity, true, null);
            }

            // When tapped we will show a alert
            public override void AccessoryButtonTapped(UITableView tableView,
                                           NSIndexPath indexPath)
            {
                //var emailItem = emailServer.Email[indexPath.Row];

                //var controller = UIAlertController.Create("Email Details",
                //                     emailItem.ToString(), UIAlertControllerStyle.Alert);
                //controller.AddAction(UIAlertAction.Create("OK",
                //                     UIAlertActionStyle.Default, null));

                //owner.PresentViewController(controller, true, null);
            }
        }
    }
}

