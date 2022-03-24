using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using ToDo.Models;

namespace ToDo.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        string description = "";
        ToDoList current = null;
        bool Editing = false;

        string heading = "";
        public string Heading
        {
            get => heading;
            set => this.RaiseAndSetIfChanged(ref heading, value);
        }
        public string Description
        {
            get => description;
            set => this.RaiseAndSetIfChanged(ref description, value);
        }

        DateTimeOffset date = DateTimeOffset.Now.Date;
        public DateTimeOffset Date
        {
            set
            {
                this.RaiseAndSetIfChanged(ref date, value);
                this.ChangeObservableCollection(date);
            }
            get => date;
        }
        public ObservableCollection<ToDoList> Items { get; set; }


        ViewModelBase content;
        public ViewModelBase Content
        {
            get => content;
            private set => this.RaiseAndSetIfChanged(ref content, value);
        }

        public MainWindowViewModel()
        {
            ListByDays = new Dictionary<DateTimeOffset, List<ToDoList>>();
            Items = new ObservableCollection<ToDoList>();
            Content = new ToDoListViewModel();
        }

        private Dictionary<DateTimeOffset, List<ToDoList>> ListByDays;
        private void InitToDoList()
        {
            ListByDays = new Dictionary<DateTimeOffset, List<ToDoList>>();
            ListByDays.Add(date, new List<ToDoList>());
        }

        public void AppendAction(DateTimeOffset date, ToDoList item)
        {
            if (!ListByDays.ContainsKey(date))
                ListByDays.Add(date, new List<ToDoList>());
            ListByDays[date].Add(item);
            this.ChangeObservableCollection(Date);
        }


        public void ChangeView()
        {
            if (this.Content is ToDoListViewModel)
                this.Content = new NoteViewModel();
            else
            {
                Heading = "";
                Description = "";
                current = null;
                Editing = false;
                Content = new ToDoListViewModel();
            }
        }

        public void ChangeObservableCollection(DateTimeOffset date)
        {
            if (!ListByDays.ContainsKey(date))
                Items.Clear();
            else
            {
                Items.Clear();
                foreach (var item in ListByDays[date])
                    Items.Add(item);
            }
        }

        public void Save()
        {
            if (Heading != "")
            {
                if (Editing)
                {
                    var item = ListByDays[date].Find(x => x.Equals(current));
                    item.Heading = this.Heading;
                    item.Description = this.Description;
                    Editing = false;
                }
                else
                    AppendAction(Date, new ToDoList(Heading, Description));
                ChangeView();
            }
        }

        public void Delete(ToDoList item)
        {
            ListByDays[date].Remove(item);
            ChangeObservableCollection(date);
        }

        public void ViewExisting(ToDoList item)
        {
            Editing = true;
            current = item;
            Heading = current.Heading;
            Description = current.Description;
            ChangeView();
        }
    }
}