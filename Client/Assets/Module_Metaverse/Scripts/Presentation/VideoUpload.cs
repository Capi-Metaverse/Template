using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Video;

public class VideoUpload : IFileUpload
{
    private string _path;//File path
    private Presentation _presentation;
    private TMP_Text _loadingPressCanvas;

    public VideoUpload(string Path, Presentation Presentation, TMP_Text LoadingPressCanvas)
    {
        _path = Path;
        _presentation = Presentation;
        _loadingPressCanvas = LoadingPressCanvas;
    }


    public void FileUpload(byte[] bytes, string fileExtension)
    {
        string filename = _path.Split("\\").Last().Split('.')[0].Trim();

        //Create VideoFile
        Debug.Log(_path);
        string PostPath = Application.persistentDataPath + $"/{filename}.{fileExtension}";
        if (!System.IO.File.Exists(PostPath))
            System.IO.File.WriteAllBytes(PostPath, bytes);

        //Set VideoClip
        _loadingPressCanvas.enabled = false;
        _presentation.transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
        VideoPlayer videoPlayer = _presentation.transform.GetChild(0).GetComponent<VideoPlayer>();
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = PostPath;
    }
}
