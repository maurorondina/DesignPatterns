public static class VisitorGoodExample
{
    public static void Run()
    {
        // Object structure (composed of elements)
        var document = new IDocumentElement[]
        {
            new Heading("Authors", 5),
            new Paragraph("...."),
            new Heading("Visitor Pattern", 1),
            new Paragraph("A behavioral design pattern."),
            new Heading("Introduction", 3),
            new Paragraph("...."),
            new Heading("Example", 4),
            new Paragraph("...."),
            new Heading("Structure", 3),
            new Image("visitor.jpg", "Visitor UML"),
            new Heading("Implementation", 3),
            new Paragraph("...."),
        };

        // HTML Export
        var htmlVisitor = new HtmlExportVisitor();
        var htmlOutput = document.Select(e => e.Accept(htmlVisitor));
        Console.WriteLine("HTML:\n" + string.Join("\n", htmlOutput));

        // Generate TOC
        var tocVisitor = new TocVisitor(2); // Only include the 2 lowest levels
        document.ToList().ForEach(e => e.Accept(tocVisitor));
        Console.WriteLine("Table of Contents:\n" +
            string.Join("\n", tocVisitor.Entries.Select(e => $"Level {e.Level}: {e.Text}")));
    }

    // ELEMENT
    public interface IDocumentElement
    {
        T Accept<T>(IVisitor<T> visitor);
    }

    // CONCRETE ELEMENTS
    public record Paragraph(string Text) : IDocumentElement
    {
        public T Accept<T>(IVisitor<T> visitor) => visitor.Visit(this);
    }

    public record Heading(string Text, int Level) : IDocumentElement
    {
        public T Accept<T>(IVisitor<T> visitor) => visitor.Visit(this);
    }

    public record Image(string Source, string? AltText = null) : IDocumentElement
    {
        public T Accept<T>(IVisitor<T> visitor) => visitor.Visit(this);
    }

    // VISITOR
    public interface IVisitor<out T>
    {
        T Visit(Paragraph paragraph);
        T Visit(Heading heading);
        T Visit(Image image);
    }

    // CONCRETE VISITORS
    public class HtmlExportVisitor : IVisitor<string>   // stateless visitor
    {
        public string Visit(Paragraph paragraph) => $"<p>{paragraph.Text}</p>";
        public string Visit(Heading heading) => $"<h{heading.Level}>{heading.Text}</h{heading.Level}>";
        public string Visit(Image image) => $"<img src=\"{image.Source}\" alt=\"{image.AltText}\" />";
    }

    public record TocEntry(string Text, int Level);
    public class TocVisitor(int tocLevels) : IVisitor<IEnumerable<TocEntry>>  // stateful visitor
    {
        private readonly SortedSet<int> _encounteredLevels = new();
        private readonly List<TocEntry> _allEntries = new();
        
        public IEnumerable<TocEntry> Entries => FilterLowestLevels();
    
        public IEnumerable<TocEntry> Visit(Paragraph paragraph) => Entries;
        
        public IEnumerable<TocEntry> Visit(Heading heading)
        {
            _encounteredLevels.Add(heading.Level);
            
            _allEntries.Add(new TocEntry(heading.Text, heading.Level));
            return Entries;
        }
        
        public IEnumerable<TocEntry> Visit(Image image) => Entries;
        
        // Filter to include only entries with the N lowest levels
        private IEnumerable<TocEntry> FilterLowestLevels()
        {
            if (_encounteredLevels.Count <= tocLevels)
                return _allEntries;
                
            var lowestLevels = _encounteredLevels.Take(tocLevels).ToHashSet();
            return _allEntries.Where(entry => lowestLevels.Contains(entry.Level));
        }
    }
}