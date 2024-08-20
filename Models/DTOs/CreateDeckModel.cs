namespace Quiz_API.Models.DTOs
{
  public class CreateDeckModel
  {
    public string DeckName { get; set; }
    public string Description { get; set; }
    public List<CreateCardModel>? Cards { get; set; }

  }
}
