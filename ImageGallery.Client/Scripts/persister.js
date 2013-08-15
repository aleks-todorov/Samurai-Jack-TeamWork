/// <reference path="class.js" />
/// <reference path="http-requester.js" />
/// <reference path="jquery-2.0.2.js" />

var persisters = (function () {
    var MainPersister = Class.create({

        init: function (rootUrl) {
            this.rootUrl = rootUrl;
            this.album = new AlbumsPersister(this.rootUrl);
            this.comment = new CommentsPersister(this.rootUrl);
            this.image = new ImagesPersister(this.rootUrl);
            this.user = new UsersPersister(this.rootUrl);
        }
    });

    var AlbumsPersister = Class.create({
        init: function (rootUrl) {
            this.rootUrl = rootUrl + "albums";
        },

        getAlbums: function (userId, success, error) {
            var url = this.rootUrl + "?userId=" + userId;
            httpRequester.getJSON(url, function (data) {
                success(data);
            }, error);
        },

        getAlbum: function (userId, albumId, success, error) {
            var url = this.rootUrl + "?userId=" + userId + "&albumId=" + albumId;
            httpRequester.getJSON(url, function (data) { 
                success(data);
            }, error);
        },

        postAlbum:function (userId, albumId, album, success, error) {
            var url = this.rootUrl + "?userId=" + userId + "&albumId=" + albumId;
           
            var albumData = {
                AlbumId: album.AlbumId,
                AlbumName: album.AlbumName,
                UserId: album.UserId,
                Path: album.Path
            };

            httpRequester.postJSON(url, albumData, function (data) {
                success(data);
            }, error);
        },

        deleteAlbum: function (userId, albumId, success, error) {
            var url = this.rootUrl + "?userId=" + userId + "&albumId=" + albumId;
            httpRequester.deleteJSON(url, function (data) {
                success(data)
            }, error);
        }
    });

    var CommentsPersister = Class.create({
        init: function (rootUrl) {
            this.rootUrl = rootUrl + "comments";
        },

        getComments: function (userId, success, error) {
            var url = this.rootUrl + "?userId=" + userId;
            httpRequester.getJSON(url, function (data) {
                success(data);
            }, error);
        },

        getComment: function (userId, commentId, success, error) {
            var url = this.rootUrl + "?userId=" + userId + "&commentId=" + commentId;
            httpRequester.getJSON(url, function (data) {
                success(data);
            }, error);
        },

        postComment: function (userId, commentId, comment, success, error) {
            var url = this.rootUrl + "?userId=" + userId;
            var commentData = {
                CommentId: comment.CommentId,
                Content: comment.Content,
                AuthorId: comment.AuthorId,
                ImageId: comment.ImageId
            };
            httpRequester.postJSON(url, commentData, function (data) {
                success(data);
            }, error);
        },

        deleteComment: function (userId, commentId, success, error) {
            var url = this.rootUrl + "?userId=" + userId + "&commentId=" + commentId;
            httpRequester.deleteJSON(url, function (data) {
                success(data);
            }, error);
        }
    });

    var ImagesPersister = Class.create({
        init: function (rootUrl) {
            this.rootUrl = rootUrl + "images";
        },

        getImages: function (userId, success, error) {
            var url = this.rootUrl + "?userId=" + userId;
            httpRequester.getJSON(url, function (data) {
                success(data);
            }, error);
        },

        getImage: function (userId, imageId, success, error) {
            var url = this.rootUrl + "?userId=" + userId + "&imageId=" + imageId;
            httpRequester.getJSON(url, function (data) {
                success(data);
            }, error);
        },

        postImage: function (userId, albumId, image) {
            var url = this.rootUrl + "&userId=" + userId + "&albumId=" + albumId;
            var imageData = {
                ImageId: image.ImageId,
                ImageName: image.Name,
                AlbumId: image.AlbumId,
                Path: image.Path,
                URL: image.URL
            };

            httpRequester.postJSON(url, imageData, function (data) {
                success(data);
            }, error);
        },

        deleteImage: function (userId, albumId, imageId) {
            var url = this.rootUrl + "?userId=" + userId + +"&albumId=" + albumId + "&imageId=" + imageId;
            httpRequester.deleteJSON(url, function (data) {
                success(data);
            }, error);
        }

    });

    var UsersPersister = Class.create({
        init: function (rootUrl) {
            this.rootUrl = rootUrl + "users";
        },

        getUsers: function (url, success, error) {
            var url = this.rootUrl;
            httpRequester.getJSON(url, function (data) {
                success(data);
            }, error);
        },

        getUser: function (userId, success, error) {
            var url = this.rootUrl + "/" + userId;
            httpRequester.getJSON(url, function (data) {
                success(data);
            }, error);
        },

        postUser: function (user, success, error) {
            var url = this.rootUrl;
            var userData = {
                UserId: user.UserId,
                UserName: user.UserName
            };

            httpRequester.postJSON(url, userData, function (data) {
                success(data);
            }, error);
        },

        deleteUser: function (userId, success, error) {
            var url = this.rootUrl + "/" + userId;
            httpRequester.deleteJSON(url, function (data) {
                success(data);
            }, error);
        }
    });

    return {
        get: function (url) {
            return new MainPersister(url);
        }
    };
}());