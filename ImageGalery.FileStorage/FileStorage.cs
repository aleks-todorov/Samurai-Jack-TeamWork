using ImageGallery.Model;
using Spring.IO;
using Spring.Social.Dropbox.Api;
using Spring.Social.Dropbox.Connect;
using Spring.Social.OAuth1;
using System;
using System.IO;
using System.Linq;
using System.Net;
using ImageGallery.Data;

public static class FileStorageAPI
{
    private const string DropboxAppKey = "nlosf6upb1xeqon";
    private const string DropboxAppSecret = "3tuhgfnhfbqdls6";
    private static OAuthToken oauthAccessToken = new OAuthToken("tljikhg4l311qwlf", "d66t9r2yhvo39ai");

    static DropboxServiceProvider dropboxServiceProvider =
         new DropboxServiceProvider(DropboxAppKey, DropboxAppSecret, AccessLevel.AppFolder);

    // Login in Dropbox
    static IDropbox dropbox = dropboxServiceProvider.GetApi(oauthAccessToken.Value, oauthAccessToken.Secret);

    public static void CreateFolder(Album album)
    {
        Entry createFolderEntry = dropbox.CreateFolder(album.Path + "\\" + album.AlbumName);
    }

    public static string UploadFile(Image image, string path)
    {
        WebClient webClient = new WebClient();
        webClient.DownloadFile(image.URL, path + "\\" + image.ImageName);

        Entry uploadFileEntry = dropbox.UploadFile(
            new FileResource(path + "\\" + image.ImageName), image.Path + "/" + image.ImageName);

        DropboxLink sharedUrl = dropbox.GetShareableLink(uploadFileEntry.Path);
        // File.Delete(image.ImageName);
        return sharedUrl.Url;
    }
}

