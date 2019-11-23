using System;
using UIKit;
using CoreGraphics;
using System.Linq;
using SolitaireiOS.Lang;

namespace SolitaireiOS
{
    public static class Dialog 
    {
        private const int DIALOG_WIDTH = 320;
        private const int DIALOG_HEIGHT = 300;
        //static UIView parentView;
        static UIView dialogContainer;
        static UIViewController callerInstance;

        public enum CreateSolitiareType { CreateBoard, CreateDeck, CreateCard }
        
        public static UIView MakeDialog(string _type, UIViewController _callerInstance, CreateSolitiareType createSolitiareType)
        {
            callerInstance = _callerInstance;

            dialogContainer = new UIView(callerInstance.View.Bounds);
            dialogContainer.UserInteractionEnabled = true;
            dialogContainer.BackgroundColor = UIColor.Clear;

            var backgroundBtn = new UIButton()
            {
                Frame = callerInstance.View.Bounds,
                BackgroundColor = UIColor.Gray
            };
            backgroundBtn.BackgroundColor = UIColor.FromWhiteAlpha(0.1f, 0.9f);
            backgroundBtn.TouchUpInside += delegate {
                dialogContainer.RemoveFromSuperview();
            };
            var dialogContent = new UIView()
            {
                Frame = MakeDialogContentFrame()
            };
            var nameField = new UITextField()
            {
                Frame = new CGRect(dialogContent.Bounds.Width / 2 - 150, 50, 300, 40),
                BorderStyle = UITextBorderStyle.RoundedRect,
                Placeholder = _type + " Name",
                UserInteractionEnabled = true
            };
            var descriptionField = new UITextField()
            {
                Frame = new CGRect(dialogContent.Bounds.Width / 2 - 150, 110, 300, 40),
                BorderStyle = UITextBorderStyle.RoundedRect,
                Placeholder = _type + " Description"
            };
            var cancelBtn = new UIButton(dialogContent.Bounds)
            {
                Frame = new CGRect(descriptionField.Frame.X + 10, descriptionField.Frame.Y + 60, 70, 40)
            };
            cancelBtn.SetTitleColor(UIColor.White, UIControlState.Normal);
            cancelBtn.SetTitle("Cancel", UIControlState.Normal);
            cancelBtn.TouchUpInside += delegate {
                dialogContainer.RemoveFromSuperview();
            };
            var addBtn = new UIButton()
            {
                Frame = new CGRect(descriptionField.Frame.Width - 70, descriptionField.Frame.Y + 60, 70, 40)
            };
            addBtn.SetTitleColor(UIColor.White, UIControlState.Normal);
            addBtn.SetTitle("Add", UIControlState.Normal);
            addBtn.TouchUpInside += delegate {
                switch (createSolitiareType)
                {
                    case CreateSolitiareType.CreateBoard:
                        CreateBoard(nameField.Text.Trim(), descriptionField.Text);
                        break;
                    case CreateSolitiareType.CreateDeck:
                        CreateDeck(nameField.Text.Trim(), descriptionField.Text);
                        break;
                    case CreateSolitiareType.CreateCard:

                        

                        //CreateCard(nameField.Text.Trim(), descriptionField.Text.Trim(), );
                        break;
                }
                
            };

            if(createSolitiareType == CreateSolitiareType.CreateCard)
            {
                //Label to show what deck is selected
                UILabel deckLabel = new UILabel(
                    new CGRect(dialogContent.Bounds.Width / 2 - 150, descriptionField.Frame.Y + 60, 300, 40)
                    
                );
                deckLabel.Text = "testing one two three";
                deckLabel.Layer.MasksToBounds = true;
                deckLabel.Layer.CornerRadius = 4;
                deckLabel.BackgroundColor = UIColor.White;
                //Makes the picker view
                UIPickerView deckPicker = new UIPickerView(
                new CGRect(
                    dialogContent.Bounds.Width / 2 - 150, 170, 300, 40
                    )
                );
                //Has to get where its being called from and what board to view to look at
                //decks assigned to the board
                UseBoardViewController useboard = (UseBoardViewController)callerInstance;
                deckPicker.Model = new DeckPickerModel(deckLabel, useboard.thisBoard.Decks);

                //Add new views to the dialog content.
                dialogContent.AddSubviews(deckLabel, deckPicker);


                //Moves the buttons down due to deckLabel taking their previous place.
                addBtn.Frame = new CGRect(descriptionField.Frame.Width - 70, deckLabel.Frame.Y + 60, 70, 40);
                cancelBtn.Frame = new CGRect(descriptionField.Frame.X + 10, deckLabel.Frame.Y + 60, 70, 40);
            }

            var dialogLabel = new UILabel()
            {
                Frame = new CGRect(nameField.Frame.X, nameField.Frame.Y - 50, 100, 40),
                Text = "Add " + _type,
                TextColor = UIColor.White
            };
            dialogContent.AddSubviews(dialogLabel, nameField, descriptionField, cancelBtn, addBtn);
            dialogContainer.AddSubviews(backgroundBtn, dialogContent);

            return dialogContainer;
        }

        /// 
        ///
        ///     Creates a the content container for dialog
        /// 
        /// 
        private static CGRect MakeDialogContentFrame()
        {
            return new CGRect(callerInstance.View.Bounds.Width / 2 - 160, callerInstance.View.Bounds.Height / 2 - DIALOG_WIDTH / 2, DIALOG_WIDTH, DIALOG_HEIGHT);
        }


        private static void CreateCard(string _name, string _description, Deck _deck)
        {
            
        }

        private static void CreateDeck(string _name, string _description)
        {
            // Checks to make sure this name doesnt not already exist within the respective board, because we will be using the board's title as the category name
            if (_name != "" && _name != null && UseBoardViewController.thisKanban.Columns.All(deck => deck.Title != _name))
            {
                dialogContainer.RemoveFromSuperview();
                ((UseBoardViewController)callerInstance).AddDeck(_name, _description);
            }
            else
            {
                // UIAlertController alert = UIAlertController.Create("Login", "Enter your credentials", UIAlertControllerStyle.Alert);
                // alert.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Cancel, null));
                // alert.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, myCancel));
                
                // PresentViewController(alert, animated: true, completionHandler: null);
                return;
            }
        }

        private static void CreateBoard(string _name, string _description)
        {
            // Checks to make sure this name doesnt not already exist within the respective board, because we will be using the board's title as the category name
            if (_name != "" && _name != null && AssetManager.boards.All(board => board.Name != _name))
            { 
                dialogContainer.RemoveFromSuperview();
                AssetManager.boards.Add(new Lang.Board(_name, _description));
            }
            else
            {
                // UIAlertController alert = UIAlertController.Create("Login", "Enter your credentials", UIAlertControllerStyle.Alert);
                // alert.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Cancel, null));
                // alert.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, myCancel));

                // PresentViewController(alert, animated: true, completionHandler: null);
                return;
            }
        }
    }
}
