public static class VisitorBadExample
{
    public static void Run()
    {
        var document = new object[]
        {
            new Heading("Visitor Pattern", 1),
            new Paragraph("A behavioral design pattern."),
            new Image("visitor.jpg", "Visitor UML"),
            new Heading("Implementation", 2)
        };

        var html = document.Select(e => 
                e is Paragraph p ? p.ToHtml() : 
                e is Heading h ? h.ToHtml() :
                e is Image i ? i.ToHtml() : "")
            .Where(html => !string.IsNullOrEmpty(html))
            .ToList();
        Console.WriteLine("HTML:\n" + string.Join("\n", html));

        var toc = document.Select(e =>
                e is Paragraph p ? p.ToTocEntry() : 
                e is Heading h ? h.ToTocEntry() :
                e is Image i ? i.ToTocEntry() : "")
            .Where(entry => !string.IsNullOrEmpty(entry))
            .ToList();
        Console.WriteLine("Table of Contents:\n" + string.Join("\n", toc));
    }

    public record Paragraph(string Text)
    {
        public string ToHtml() => $"<p>{Text}</p>";
        public string ToTocEntry() => ""; // Irrelevant for paragraphs
    }

    public record Heading(string Text, int Level)
    {
        public string ToHtml() => $"<h{Level}>{Text}</h{Level}>";
        public string ToTocEntry() => $"Level {Level}: {Text}";
    }

    public record Image(string Source, string? AltText = null)
    {
        public string ToHtml() => $"<img src=\"{Source}\" alt=\"{AltText}\" />";
        public string ToTocEntry() => ""; // Irrelevant for images
    }
}