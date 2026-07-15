using Microsoft.AspNetCore.Mvc;
using RestRoutingDrills.Interfaces;
using RestRoutingDrills.Models;

namespace RestRoutingDrills.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotesController(INoteStore noteStore) : ControllerBase
{
    [HttpGet("search")]
    public IActionResult GetAllNotesByKeyword([FromQuery] string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
        {
            return BadRequest(new { error = "Keyword is required" });
        }

        var notes = noteStore.GetAllNotesByKeyword(keyword);
        return Ok(notes);
    }

    [HttpGet]
    public IActionResult GetAllNotes([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        if (pageNumber < 1)
        {
            return BadRequest(new { error = "Page number must be greater than 0" });
        }
        if (pageSize < 1 || pageSize > 50)
        {
            return BadRequest(new { error = "Page size must be greater than 0 and less than 50" });
        }

        var notes = noteStore.GetAllNotes(pageNumber, pageSize);
        return Ok(notes);
    }

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
