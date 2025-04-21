using Microsoft.EntityFrameworkCore;
using QuickNotes.Api;

var builder = WebApplication.CreateBuilder(args);

//Register SQLite-backed EF Core DbContext
builder.Services.AddDbContext<NotesDbContext>(options => options.UseSqlite("Data Source=notes.db"));

// Add Swagger/OpenAPI Support
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// READ all notes
app.MapGet("/notes", async (NotesDbContext db) =>
    await db.Notes.ToListAsync());

// POST /notes  → create a new note (body = plain string text)
app.MapPost("/notes", CreateNote);

// DELETE /notes/{id} → delete by ID
app.MapDelete("/notes/{id:int}", DeleteNote);

app.Run();

//Handlers

// Creates a note with the given text
async Task<IResult> CreateNote(NotesDbContext db, string text)
{
    // Build the Note entity
    var note = new Note
    {
        Text    = text,
        Created = DateTime.UtcNow
    };

    // Add + save
    db.Notes.Add(note);
    await db.SaveChangesAsync();

    // Return 201 Created with location header
    return Results.Created($"/notes/{note.Id}", note);
}

// Deletes a note by its ID
async Task<IResult> DeleteNote(NotesDbContext db, int id)
{
    // Look up the entity
    var note = await db.Notes.FindAsync(id);
    if (note == null)
    {
        // 404 if it wasn't found
        return Results.NotFound();
    }

    // Remove + save
    db.Notes.Remove(note);
    await db.SaveChangesAsync();

    // 204 No Content on success
    return Results.NoContent();
}
