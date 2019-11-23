using System;
using System.Collections.Generic;
using SolitaireiOS.Lang;
using UIKit;

namespace SolitaireiOS
{
    public class DeckPickerModel : UIPickerViewModel
    {
        private List<Deck> decks;
        private UILabel deckLabel;
        public DeckPickerModel(UILabel _deckLabel, List<Deck> _decks)
        {
            deckLabel = _deckLabel;
            decks = _decks;
        }

        public override nint GetComponentCount(UIPickerView pickerView)
        {
            return 2;
        }

        public override nint GetRowsInComponent(UIPickerView pickerView, nint component)
        {
            return decks.Count;
        }

        public Deck GetDeck(UIPickerView pickerView)
        {

            return decks[(int)pickerView.SelectedRowInComponent(1)];


        }

        public override void Selected(UIPickerView pickerView, nint row, nint component)
        {

            deckLabel.Text = decks[(int)pickerView.SelectedRowInComponent(1)].Name;

        }

        public override nfloat GetComponentWidth(UIPickerView picker, nint component)
        {
            if (component == 0)
                return 240f;
            else
                return 0f;
        }

        public override nfloat GetRowHeight(UIPickerView picker, nint component)
        {
            return 40f;
        }



    }
}
