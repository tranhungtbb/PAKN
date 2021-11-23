


var imageFilePath = "test.jpg";

var json = File.ReadAllText("fake-image.json");
var sampleData = JsonConvert.DeserializeObject<Dictionary<long, FakeResult>>(json);
var fakeImage = new FakeImageDetection(sampleData);

if (fakeImage.IsFake(imageFilePath)) // or fakeImage.IsFake(new Bitmap(imageFilePath))
{
    // The image is faked
}