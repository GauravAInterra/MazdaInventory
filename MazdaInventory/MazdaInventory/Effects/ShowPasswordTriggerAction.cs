using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Xamarin.Forms;

namespace MazdaInventory.Effects
{
    class ShowPasswordTriggerAction : TriggerAction<ImageButton>
    {
        public string IconImageName { get; set; }

        public string EntryPasswordName { get; set; }

        public string EntryTextName { get; set; }

        protected override void Invoke(ImageButton sender)
        {
            Console.WriteLine("here in invoke");
            // get the runtime references 
            // for our Elements from our custom control
            //var imageIconView = ((Grid)sender.Parent).FindByName<Image>(IconImageName);
            var entryPasswordView = ((Grid)sender.Parent).FindByName<Entry>(EntryPasswordName);
            var entryTextView = ((Grid)sender.Parent).FindByName<Entry>(EntryTextName);

            // Switch visibility of Password 
            // Entry field and Text Entry fields
            entryPasswordView.IsVisible =
                           !entryPasswordView.IsVisible;
            entryTextView.IsVisible =
                           !entryTextView.IsVisible;

            // update the Show/Hide button Icon states 
            if (entryPasswordView.IsVisible)
            {
                // Password is not Visible state
                //imageIconView.Source = ImageSource.FromResource("showhide.png",Assembly.GetExecutingAssembly());

                // Setting up Entry curser focus
                entryPasswordView.Focus();
                entryPasswordView.Text = entryTextView.Text;
            }
            else
            {
                // Password is Visible state
                //imageIconView.Source = ImageSource.FromResource("showhide.png",Assembly.GetExecutingAssembly());

                // Setting up Entry curser focus
                entryTextView.Focus();
                entryTextView.Text = entryPasswordView.Text;
            }
        }
        }
}
