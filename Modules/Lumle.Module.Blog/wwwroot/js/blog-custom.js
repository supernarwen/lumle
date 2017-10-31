﻿(function () {

    $('#mainForm').on('submit', function () {
        var form = $(this);
        form.parsley().validate();
        var isValidForm = form.parsley().isValid();
        if (isValidForm) {
            form.find('button[type=submit]').prop('disabled', true);
            $("#loadingImg").show();
            form.get(0).submit();
        }
        return false;
    });

    var edit = document.getElementById("mainForm");

    if (edit) {
        // Setup editor
        var editPost = document.getElementById("Content");

        tinymce.init({
            selector: '#Content',
            autoresize_min_height: 200,
            plugins: 'autosave preview searchreplace visualchars image link media fullscreen code table hr pagebreak autoresize nonbreaking anchor insertdatetime advlist lists textcolor wordcount imagetools colorpicker',
            menubar: "edit view format insert table",
            toolbar1: 'formatselect | bold italic blockquote forecolor backcolor | imageupload link | alignleft aligncenter alignright  | numlist bullist outdent indent | fullscreen',
            selection_toolbar: 'bold italic | quicklink h2 h3 blockquote',
            autoresize_bottom_margin: 0,
            paste_data_images: true,
            image_advtab: true,
            file_picker_types: 'image',
            relative_urls: false,
            convert_urls: false,
            branding: false,

            setup: function (editor) {
                editor.addButton('imageupload', {
                    icon: "image",
                    onclick: function () {
                        var fileInput = document.createElement("input");
                        fileInput.type = "file";
                        fileInput.multiple = true;
                        fileInput.addEventListener("change", handleFileSelect, false);
                        fileInput.click();
                    }
                });

            }
        });

        // File upload
        function handleFileSelect(event) {
            if (window.File && window.FileList && window.FileReader) {

                var files = event.target.files;

                for (var i = 0; i < files.length; i++) {
                    var file = files[i];

                    // Only image uploads supported
                    if (!file.type.match('image'))
                        continue;

                    var reader = new FileReader();
                    reader.addEventListener("load", function (event) {
                        var image = new Image();
                        image.alt = file.name;
                        image.onload = function (e) {
                            image.setAttribute("data-filename", file.name);
                            image.setAttribute("width", image.width);
                            image.setAttribute("height", image.height);
                            tinymce.activeEditor.execCommand('mceInsertContent', false, image.outerHTML);
                        };
                        image.src = this.result;

                    });

                    reader.readAsDataURL(file);
                }

                document.body.removeChild(event.target);
            }
            else {
                console.log("Your browser does not support File API");
            }
        }
    }
})();