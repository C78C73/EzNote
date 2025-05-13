using System.Diagnostics;
using System.IO;
using System.Text.Json;

namespace EzNote
{
    // handles saving and loading notes
    public static class NoteStorage
    {
        private const string NotesDirectory = "Notes"; // saves in bin folder
        private const string IndexFile = "allNoteTitles.json"; //saved in bin folder

        // checks that file names are valid to be saved
        private static string ChecksFileNameValidity(string title)
        {
            foreach (var c in Path.GetInvalidFileNameChars())
                title = title.Replace(c, '_');
            return title;
        }

        // saves files in their own file, in the Notes folder, also saving the file names in a json file
        public static List<Note> LoadAll()
        {
            List<Note> notes = new();

            if (!Directory.Exists(NotesDirectory))
                Directory.CreateDirectory(NotesDirectory);

            if (File.Exists(IndexFile))
            {
                try
                {
                    var titles = JsonSerializer.Deserialize<List<string>>(File.ReadAllText(IndexFile));
                    if (titles != null)
                    {
                        foreach (var title in titles)
                        {
                            var filePath = Path.Combine(NotesDirectory, ChecksFileNameValidity(title) + ".json");
                            if (File.Exists(filePath))
                            {
                                var note = JsonSerializer.Deserialize<Note>(File.ReadAllText(filePath));
                                if (note != null)
                                    notes.Add(note);
                            }
                        }
                    }
                }
                catch (Exception error)
                {
                    Debug.WriteLine($"\nError: Type-{error.GetType().Name} | Message-{error.Message}");
                }
            }

            return notes;
        }

        public static void SaveAll(IEnumerable<Note> noteList)
        {
            try
            {
                if (!Directory.Exists(NotesDirectory))
                    Directory.CreateDirectory(NotesDirectory);

                var titles = new List<string>();

                foreach (var note in noteList)
                {
                    var fileName = ChecksFileNameValidity(note.Title) + ".json";
                    var filePath = Path.Combine(NotesDirectory, fileName);
                    File.WriteAllText(filePath, JsonSerializer.Serialize(note));
                    titles.Add(note.Title);
                }

                File.WriteAllText(IndexFile, JsonSerializer.Serialize(titles));
                Debug.WriteLine($"\nNote saved.\nPath: {Path.GetFullPath(NotesDirectory)}\n");
            }

            // catch for errors
            catch (Exception error)
            {
                Debug.WriteLine($"\nError: Type-{error.GetType().Name} | Message-{error.Message}");
            }
        }
    }
}