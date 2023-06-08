/// <summary>
/// Interface that defines the methods related to file uploading
/// </summary>
public interface IFileUpload
{
    /// <summary>
    /// Method that sets the file into the presentation gameObject
    /// </summary>
    /// <param name="bytes"></param>
    /// <param name="fileExtension"></param>
    /// <returns></returns>
    public void FileUpload(byte[] bytes, string fileExtension);
}
