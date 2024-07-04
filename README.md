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
