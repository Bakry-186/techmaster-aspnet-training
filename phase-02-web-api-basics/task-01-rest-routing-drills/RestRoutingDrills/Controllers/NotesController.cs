using Microsoft.AspNetCore.Mvc;
using RestRoutingDrills.Interfaces;
using RestRoutingDrills.Models;

namespace RestRoutingDrills.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotesController(INoteStore noteStore) : ControllerBase
{
    [HttpGet("{id}")]
    public IActionResult GetNoteById(int id)
    {
        var note = noteStore.GetNoteById(id);
        if (note == null)
        {
            return NotFound(new { error = "Note not found" });
        }
        return Ok(note);
    }

    [HttpGet]
    public IActionResult GetAllNotes()
    {
        var notes = noteStore.GetAllNotes();
        return Ok(notes);
    }

    [HttpPost]
    public IActionResult CreateNote(CreateNoteDto createNoteDto)
    {
        var note = noteStore.CreateNote(createNoteDto);
        return CreatedAtAction(nameof(GetNoteById), new { id = note.Id }, note);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateNote(int id, UpdateNoteDto updateNoteDto)
    {
        var note = noteStore.UpdateNote(id, updateNoteDto);
        if (note == null)
        {
            return NotFound(new { error = "Note not found" });
        }
        return Ok(note);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteNote(int id)
    {
        if (!noteStore.DeleteNote(id))
        {
            return NotFound(new { error = "Note not found" });
        }

        return NoContent();
    }
}
