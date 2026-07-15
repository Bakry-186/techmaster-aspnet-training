using RestRoutingDrills.Models;

namespace RestRoutingDrills.Interfaces;

public interface INoteStore
{
    Note CreateNote(CreateNoteDto createNoteDto);
    Note? GetNoteById(int id);
    List<Note> GetAllNotes();
    Note? UpdateNote(int id, UpdateNoteDto updateNoteDto);
    bool DeleteNote(int id);
}
