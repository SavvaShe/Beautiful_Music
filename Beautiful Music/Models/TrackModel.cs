
namespace BeautifulMusic.Models
{
    public class TrackModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int AlbumId { get; set; }

        public int GenreId { get; set; }

        public string Composer { get; set; }

        public int Milliseconds { get; set; }

    }
}
