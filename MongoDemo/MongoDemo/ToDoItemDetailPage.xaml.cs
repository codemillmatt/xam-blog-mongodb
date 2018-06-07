using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace MongoDemo
{
    public partial class ToDoItemDetailPage : ContentPage
    {
        public ToDoItemDetailPage()
        {
            InitializeComponent();
        }

        protected async void Cancel_Clicked(object sender, EventArgs eventArgs)
        {
            await Navigation.PopModalAsync(true);
        }

        protected async void Save_Clicked(object sender, EventArgs eventArgs)
        {
            var name = nameCell.Text;
            var descr = descriptionCell.Text;

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(descr))
                return;

            var newToDo = new ToDoItem { Name = name, Description = descr };

            await MongoService.InsertItem(newToDo);

            await Navigation.PopModalAsync(true);
        }
    }
}
