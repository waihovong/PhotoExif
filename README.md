# PhotoExif
PhotoExif is a fast console application that allows you to view your images exif data all in tables.
It also gives data on the most commonly used settings such as ISO, Shutter Speed and Aperature.
</br>
This is most useful to give a quick analysis of your photography settings and usage.

## Getting Started
Ensure the [MetadataExtractor](https://github.com/drewnoakes/metadata-extractor-dotnet/tree/main) and [ConsoleTables](https://github.com/khalidabuhakmeh/ConsoleTables) nuget package are installed
```
  <ItemGroup>
    <PackageReference Include="ConsoleTables" Version="2.4.2" />
    <PackageReference Include="MetadataExtractor" Version="2.8.1" />
  </ItemGroup>
```
* Set `Program.cs` to startup project
* Find your file path from file explorer
```
public static void Main(string[] args)
{
  List<ImageExif> images = Program.GetImagesFromPath(""); // Update This to your image path
}
```
* Click `PhotoExif` or `dotnet run` in the Developer PowerShell

### Example output
`Exif Stats`

| F-Stop        | Count      | ISO | Count | Focal Length | Count | Shutter Speed | Count |
| ----------- | -------------- | ------|------|------|------|------| ------- |
| f/5.0  | 523       | 200 | 336 | 35 mm | 412 | 1/250 sec | 178 |
| f/16.0 | 18       | 1250 | 36 | 55 mm | 312 | 1/80 sec | 32 |
```
`ordered by most used`
```
`Individual Image Info`

| File        | F-Stop      | ISO | Shutter Speed | Focal Length | Exposure Bias | Date Time | White Balance | Camera|
| ----------- | -------------- | ------|------|------|------|------| ------- | ------- |
| IMG2912.JPG | f/4.0       | ISO-200 | 1/500 sec | 35 mm | 0 EV | 2024:01:02 09:00:00 | Auto white balance | FUJIFILM X-T3 |
| IMG5412.JPG | f/2.8       | ISO-1600 | 1/75 sec | 55 mm | 0 EV | 2024:06:02 19:00:00 | Manual white balance | FUJIFILM X-T3 |

## Usage
* If you wish to update any file extentions, they can be updated under `ImageFileExtention`
```
public enum ImageFileExtention
{
    RAF,
    JPG,
    PNG,
    //add more as needed
}
```
## Notes
Only been tested on windows pc
