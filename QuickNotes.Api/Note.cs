// Note.cs
using System;

namespace QuickNotes.Api
{
    public class Note
    {
        public int Id { get; set; }
        public string Text { get; set; } = "";
        public DateTime Created { get; set; }
    }
}