using RestRoutingDrills.Interfaces;
using RestRoutingDrills.Models;

namespace RestRoutingDrills.Services;

public class NoteStore : INoteStore
{
    private readonly List<Note> _notes = new();

    public Note CreateNote(CreateNoteDto createNoteDto)
    {
        var note = new Note { Id = _notes.Count + 1, Title = createNoteDto.Title, Content = createNoteDto.Content };
        _notes.Add(note);
        return note;
    }

    public Note? GetNoteById(int id)
    {
        return _notes.FirstOrDefault(n => n.Id == id);
    }

    public List<Note> GetAllNotes()
    {
        return _notes;
    }

    public Note? UpdateNote(int id, UpdateNoteDto updateNoteDto)
    {
        var note = GetNoteById(id);
        if (note == null)
        {
            return null;
        }
        note.Title = updateNoteDto.Title;
        note.Content = updateNoteDto.Content;
        return note;
    }

    public bool DeleteNote(int id)
    {
        var note = GetNoteById(id);
        if (note == null)
        {
            return false;
        }

        _notes.Remove(note);
        return true;
    }
}
