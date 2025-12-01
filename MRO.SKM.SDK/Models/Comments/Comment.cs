namespace MRO.SKM.SDK.Models.Comments;

public class Comment
{
    public Comment()
    {
        this.Exceptions = new();
        this.Params = new();
    }

    public string Summary { get; set; }
    public string Returns { get; set; }
    public List<CommentException> Exceptions { get; set; }
    public List<CommentParameter> Params { get; set; }
}