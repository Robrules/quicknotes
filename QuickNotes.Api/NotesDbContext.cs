
// NotesDbContext.cs
using Microsoft.EntityFrameworkCore;

namespace QuickNotes.Api
{
    public class NotesDbContext : DbContext
    {
        public NotesDbContext(DbContextOptions<NotesDbContext> options)
            : base(options)
        {
        }

        // this will now find QuickNotes.Api.Note
        public DbSet<Note> Notes => Set<Note>();
    }
}