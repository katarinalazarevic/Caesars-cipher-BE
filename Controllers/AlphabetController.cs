namespace WebTemplate.Controllers;

[ApiController]
[Route("[controller]")]
public class AlphabetController : ControllerBase
{
    public IspitContext Context { get; set; }

    public AlphabetController(IspitContext context)
    {
        Context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<string>>> GetAlphabets()
    {
        var alphabets = await Context.Alphabets.ToListAsync();

        return Ok(alphabets);
    }

    [Route("{language}")]
    [HttpGet]
    public async Task<ActionResult<Alphabet>> GetAlphabet(string language)
    {
        var alphabet = await Context.Alphabets.FindAsync(language);

        return Ok(alphabet);
    }

    [Route("languages")]
    [HttpGet]
    public async Task<ActionResult<Alphabet>> GetLanguages()
    {
        var languages = await Context.Alphabets.Select(a => a.Language).ToListAsync();

        return Ok(languages);
    }
}
