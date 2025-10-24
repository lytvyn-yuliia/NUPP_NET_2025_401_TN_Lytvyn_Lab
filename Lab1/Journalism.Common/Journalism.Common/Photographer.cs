using System;


namespace Journalism.Common
{
    public class Photographer : Person
    {
        public string CameraModel { get; set; }
        public int PhotosTaken { get; set; }

        public Photographer(string name, int experience, string city, string camera)
            : base(name, experience, city)
        {
            CameraModel = camera;
        }

        public void TakePhoto(string topic)
        {
            PhotosTaken++;
            Console.WriteLine($"{Name} took a photo for: {topic}");
        }
    }
}

