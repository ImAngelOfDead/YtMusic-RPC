namespace people2json.Models
{
    public class TrackInfo
    {
        public string Track{ get; set; }
        public string Artist{ get; set; }
        public string Cover{ get; set; }
        public int CurrentTime{ get; set; }
        public int TotalDuration{ get; set; }
        public bool IsPlaying{ get; set; }
        public string VideoId{ get; set; }
    }
}