namespace EzNote
{
    public class Note
    {

        private string title;
        private string body;
        private DateTime lastModified;

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public string Body
        {
            get { return body; }
            set { body = value; }
        }

        public DateTime LastModified
        {
            get { return lastModified; }
            set { lastModified = value; }
        }

        public Note()
        {
            title = "Untitled";
            body = "";
            lastModified = DateTime.Now;
        }

        public override string ToString()
        {
            return Title;
        }
    }
}