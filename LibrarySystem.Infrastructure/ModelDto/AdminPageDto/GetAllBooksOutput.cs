namespace LibrarySystem.Infrastructure.ModelDto.AdminPageDto
{
    public sealed class GetAllBooksOutput
    {
        public int Id { get; set; }
        public string Author { get; set; }
        public bool IsAvailable { get; set; } = true;
        public string Title { get; set; }

    }
}
