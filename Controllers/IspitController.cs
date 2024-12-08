namespace WebTemplate.Controllers;

[ApiController]
[Route("[controller]")]
public class IspitController : ControllerBase
{
    public IspitContext Context { get; set; }

    public IspitController(IspitContext context)
    {
        Context = context;
    }

    [Route("Encode")]
    [HttpGet]
    public async Task<ActionResult<string>> GetEncoded([FromQuery] string sentence, [FromQuery] int shift, [FromQuery] string language)
    {
        if (string.IsNullOrWhiteSpace(language))
            return BadRequest("Potrebno je uneti jezik.");

        var alphabet = await Context.Alphabets.FindAsync(language);
        if (alphabet is null)
            return BadRequest("Ne postoji specificirani jezik.");

        var letters = alphabet.Letters.Replace(" ", "");

        if (shift < 0 || shift > letters.Length - 1)
            return BadRequest("Pomak ne moze biti manji od 0.");

        if (shift > letters.Length - 1)
            return BadRequest($"Pomak ne moze biti veci od {letters.Length - 1}.");

        StringBuilder cipheredText = new StringBuilder();
        foreach (char c in sentence)
        {
            if (char.IsLetter(c))
            {
                bool isUpper = char.IsUpper(c);
                int letterIndex = letters.IndexOf(char.ToLower(c));
                if (letterIndex == -1)
                    return BadRequest($"Slovo {c} nije iz specificiranog jezika.");
                int newLetterIndex = (letterIndex + shift) % letters.Length;
                char cipheredChar = letters[newLetterIndex];
                cipheredText.Append(isUpper ? char.ToUpper(cipheredChar) : cipheredChar);
            }
            else
            {
                cipheredText.Append(c);
            }
        }

        return cipheredText.ToString();
    }

    [Route("Decode")]
    [HttpGet]
    public async Task<ActionResult<string>> GetDecoded([FromQuery] string sentence, [FromQuery] int shift, [FromQuery] string language)
    {
        if (string.IsNullOrWhiteSpace(language))
            return BadRequest("Potrebno je uneti jezik.");

        var alphabet = await Context.Alphabets.FindAsync(language);
        if (alphabet is null)
            return BadRequest("Ne postoji specificirani jezik.");

        var letters = alphabet.Letters.Replace(" ", "");

        if (shift < 0 || shift > letters.Length - 1)
            return BadRequest("Pomak mora ne moze biti manji od 0.");

        if (shift > letters.Length - 1)
            return BadRequest($"Pomak mora ne moze biti veci od {letters.Length - 1}.");

        StringBuilder cipheredText = new StringBuilder();
        foreach (char c in sentence)
        {
            if (char.IsLetter(c))
            {
                bool isUpper = char.IsUpper(c);
                int letterIndex = letters.IndexOf(char.ToLower(c));
                if (letterIndex == -1)
                    return BadRequest($"Slovo {c} nije iz specificiranog jezika.");
                int newLetterIndex = (letterIndex + letters.Length - shift ) % letters.Length;
                char cipheredChar = letters[newLetterIndex];
                cipheredText.Append(isUpper ? char.ToUpper(cipheredChar) : cipheredChar);
            }
            else
            {
                cipheredText.Append(c);
            }
        }

        return cipheredText.ToString();
    }

    [Route("All-encoded")]
    [HttpGet]
    public async Task<ActionResult<List<string>>> GetAllEncoded([FromQuery] string sentence, [FromQuery] string language)
    {
        if (string.IsNullOrWhiteSpace(language))
            return BadRequest("Potrebno je uneti jezik.");

        var alphabet = await Context.Alphabets.FindAsync(language);
        if (alphabet is null)
            return BadRequest("Ne postoji specificirani jezik.");

        var letters = alphabet.Letters.Replace(" ", "");

        List<string> allEncoded = new List<string>();
        for (int shift = 0; shift < letters.Length; shift++)
        {
            StringBuilder cipheredText = new StringBuilder();
            foreach (char c in sentence)
            {
                if (char.IsLetter(c))
                {
                    bool isUpper = char.IsUpper(c);
                    int letterIndex = letters.IndexOf(char.ToLower(c));
                    if (letterIndex == -1)
                        return BadRequest($"Slovo {c} nije iz specificiranog jezika.");
                    int newLetterIndex = (letterIndex + shift) % letters.Length;
                    char cipheredChar = letters[newLetterIndex];
                    cipheredText.Append(isUpper ? char.ToUpper(cipheredChar) : cipheredChar);
                }
                else
                {
                    cipheredText.Append(c);
                }
            }
            allEncoded.Add(cipheredText.ToString());
        }

        return allEncoded;
    }
}
