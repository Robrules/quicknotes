// immutable data carrier
record Note(int Id, string Text, DateTime Created);

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// in-memory store (lives only while app runs)
var notes = new List<Note>();

// READ all notes
app.MapGet("/notes", () => notes);

// CREATE a note
app.MapPost("/notes", CreateNote); 

IResult CreateNote(Note n)
{
    int nextId = notes.Count + 1;
    DateTime now = DateTime.UtcNow;

    var newNote = new Note(nextId, n.Text, now);
    notes.Add(newNote);

    return Results.Created($"/notes/{newNote.Id}", newNote);
}


// DELETE a note
app.MapDelete("/notes/{id:int}", DeleteNote);

IResult DeleteNote(int id) 
{
    int removed = notes.RemoveAll(note => note.Id == id);

    if (removed > 0) {
        return Results.NoContent();  //204 = success
    } else {
        return Results.NotFound();  //404 = couldnt find it
    }

}

app.Run();
