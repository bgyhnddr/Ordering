var CFileUpload = function (container, type, url, text) {
    this.container = container;
    this.type = type;
    this.url = url;
    this.label = document.createElement('label');
    var that = this;

    var fileInput = document.createElement('input');
    fileInput.style.display = "none";
    fileInput.type = "file";
    fileInput.accept = "." + type;
    fileInput.addEventListener("change", function (ev) {
        uploadButton.disabled = "";
        fileSelected(ev.currentTarget);
    });

    var selectButton = document.createElement('button');
    selectButton.innerHTML = "选择文件";
    selectButton.addEventListener("click", function (ev) {
        fileInput.click();
    });

    that.label.innerHTML = type;

    var uploadButton = document.createElement('button');
    uploadButton.innerHTML = text;
    uploadButton.addEventListener("click", function (ev) {
        uploadButton.disabled = "disabled";
        uploadFile(ev.currentTarget, fileInput.files[0]);
    });
    container.appendChild(fileInput);
    container.appendChild(selectButton);
    container.appendChild(uploadButton);
    container.appendChild(that.label);


    function fileSelected(el) {
        var file = el.files[0];
        if (file) {
            var fileSize = 0;
            if (file.size > 1024 * 1024)
                fileSize = (Math.round(file.size * 100 / (1024 * 1024)) / 100).toString() + 'MB';
            else
                fileSize = (Math.round(file.size * 100 / 1024) / 100).toString() + 'KB';

            that.label.innerHTML = 'Name: ' + file.name + ' ' + 'Size: ' + fileSize;
        }
    }

    function uploadFile(el, file) {
        var fd = new FormData();
        fd.append(that.type, file);
        var xhr = new XMLHttpRequest();
        xhr.upload.addEventListener("progress", uploadProgress, false);
        xhr.addEventListener("load", uploadComplete, false);
        xhr.addEventListener("error", uploadFailed, false);
        xhr.addEventListener("abort", uploadCanceled, false);
        xhr.open("POST", that.url);
        xhr.send(fd);
    }

    function uploadProgress(evt) {
        if (evt.lengthComputable) {
            var percentComplete = Math.round(evt.loaded * 100 / evt.total);
            that.label.innerHTML = percentComplete.toString() + '%';
        }
        else {
            that.label.innerHTML = 'unable to compute';
        }
    }

    function uploadComplete(evt) {
        /* This event is raised when the server send back a response */
        that.label.innerHTML = evt.target.responseText;
    }

    function uploadFailed(evt) {
        that.label.innerHTML = "There was an error attempting to upload the file.";
    }

    function uploadCanceled(evt) {
        that.label.innerHTML = "The upload has been canceled by the user or the browser dropped the connection.";
    }
}