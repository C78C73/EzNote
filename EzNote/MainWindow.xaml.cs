using System.Collections.ObjectModel;
using System.Windows;

namespace EzNote
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<Note> Notes;
        private ObservableCollection<Note> FilteredNotes = new();

        public MainWindow()
        {
            InitializeComponent();

            Notes = new ObservableCollection<Note>(NoteStorage.LoadAll());
            FilteredNotes = new ObservableCollection<Note>();
            NotesList.ItemsSource = FilteredNotes;
            RefreshFilteredNotes();
        }

        private void RefreshFilteredNotes(string query = "")
        {
            if (FilteredNotes.Count > 0)
            {
                FilteredNotes.Clear();
            }

            ObservableCollection<Note> tempList = new ObservableCollection<Note>();

            if (query != null && query.Trim() != "")
            {
                foreach (var n in Notes)
                {
                    if (n.Title.ToLower().Contains(query.ToLower()) || n.Body.ToLower().Contains(query.ToLower()))
                    {
                        tempList.Add(n);
                    }
                }
            }
            else
            {
                foreach (var n in Notes)
                {
                    tempList.Add(n);
                }
            }

            foreach (Note thing in tempList)
            {
                FilteredNotes.Add(thing);
            }
        }


        private void Click_to_Save(object sender, RoutedEventArgs e)
        {
            if (NotesList.SelectedItem is Note note)
            {
                note.Title = Title.Text;
                note.Body = Body.Text;
                note.LastModified = DateTime.Now;
                NotesList.Items.Refresh();
            }
            else
            {
                // duplicate note title check ** IF ONLY THEY MATCH 100% **
                if (Notes.Any(n => n.Title == Title.Text))
                {
                    // goes message body then message title
                    MessageBox.Show("Note with the same title already exists, please change.", "Error: Notes Title Duplicate");

                    // clears out the notes title for the user to change
                    Title.Text = "";
                    return;
                }

                var newNote = new Note
                {
                    Title = Title.Text,
                    Body = Body.Text
                };
                Notes.Add(newNote);
            }

            NoteStorage.SaveAll(Notes);

            RefreshFilteredNotes(SearchBox.Text);
        }

        private void Click_for_New_note(object sender, RoutedEventArgs e)
        {
            NotesList.SelectedItem = null;
            Title.Text = "";
            Body.Text = "";
        }

        private void Click_to_delete(object sender, RoutedEventArgs e)
        {
            if (NotesList.SelectedItem is Note note)
            {
                Notes.Remove(note);
                Title.Text = "";
                Body.Text = "";

                NoteStorage.SaveAll(Notes);

                RefreshFilteredNotes(SearchBox.Text);
            }
        }

        private void Note_selector(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (NotesList.SelectedItem is Note selectedNote)
            {
                Title.Text = selectedNote.Title;
                Body.Text = selectedNote.Body;
            }
            else
            {
                Title.Text = "";
                Body.Text = "";
            }
        }

        private void Searchbox(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            RefreshFilteredNotes(SearchBox.Text);
        }
    }
}