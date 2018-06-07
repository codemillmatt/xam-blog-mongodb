using System;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;
using System.Collections.ObjectModel;

namespace MongoDemo
{
    public partial class ToDoItemListPage : ContentPage
    {
        public ObservableCollection<ToDoItem> ToDoItems { get; set; }

        public ToDoItemListPage()
        {
            InitializeComponent();

            ToDoItems = new ObservableCollection<ToDoItem>();

            todoList.ItemsSource = ToDoItems;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            var allItems = await MongoService.GetAllItems();

            //var allItems = await MongoService.SearchByName("first");

            foreach (var item in allItems)
            {
                if (!ToDoItems.Any((arg) => arg.Id == item.Id))
                    ToDoItems.Add(item);
            }

            if (allItems.Count == 0)
            {
                var newItem = new ToDoItem { Name = "The first item", Description = "Long description that's boring" };
                await MongoService.InsertItem(newItem);

                ToDoItems.Add(newItem);
            }
        }

        protected async void Add_Clicked(object sender, EventArgs eventArgs)
        {
            var detailPage = new NavigationPage(new ToDoItemDetailPage());

            await Navigation.PushModalAsync(detailPage, true);
        }

        protected async void Delete_Item(object sender, EventArgs eventArgs)
        {
            if (!(sender is MenuItem menuItem))
                return;

            if (!(menuItem.CommandParameter is ToDoItem toDoItem))
                return;

            var success = await MongoService.DeleteItem(toDoItem);

            if (success)
            {
                ToDoItems.Remove(toDoItem);
            }
        }
    }
}
