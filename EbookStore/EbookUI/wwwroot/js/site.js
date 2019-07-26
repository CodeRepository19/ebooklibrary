
// Script for Automatic scroll div tag in index page.
$(document).ready(function () {
    $('#demo2').scrollbox({
        linear: true,
        step: 1,
        delay: 0,
        speed: 100
    });
});

//Script for Scroll top event
//$(document).ready(function () {
    var btn = $('#button');
    $(window).scroll(function () {
        if ($(window).scrollTop() > 400) {
            btn.addClass('show');
        } else {
            btn.removeClass('show');
        }
    });
    btn.on('click', function (e) {
        e.preventDefault();
        $('html, body').animate({ scrollTop: 0 }, '500');
    });

//});

// Script for Details Page
//$(document).ready(function () {
    $("#rejected").click(function () {
        $("#remarks").show();
        $("#btnreviewadd").addClass("mt-1");
    });
    $("#approved").click(function () {
        $("#remarks").hide();
        $("#btnreviewadd").removeClass("mt-1");
    });
//});

//Script for Create Form Page
$(document).ready(function () {
    if (document.querySelector("#pdf-loader") != null && document.querySelector("#changePDFLoader") != null) {
        document.querySelector("#pdf-loader").style.display = 'none';
        document.querySelector("#changePDFLoader").style.display = 'none';
        $("#addbtn").attr("disabled", "disabled");

        var _PDF_DOC,
            _CANVAS = document.querySelector('#pdf-preview'),
            _OBJECT_URL;

        function showPDF(pdf_url) {
            PDFJS.getDocument({ url: pdf_url }).then(function (pdf_doc) {

                _PDF_DOC = pdf_doc;

                // Show the first page
                showPage(1);

                // destroy previous object url
                URL.revokeObjectURL(_OBJECT_URL);
            }).catch(function (error) {
                // trigger Cancel on error
                //document.querySelector("#cancel-pdf").click();

                // error reason
                alert(error.message);
            });;
        }

        function showPage(page_no) {
            // fetch the page
            _PDF_DOC.getPage(page_no).then(function (page) {
                // set the scale of viewport
                var scale_required = _CANVAS.width / page.getViewport(1).width;

                // get viewport of the page at required scale
                var viewport = page.getViewport(scale_required);

                // set canvas height
                _CANVAS.height = viewport.height;

                var renderContext = {
                    canvasContext: _CANVAS.getContext('2d'),
                    viewport: viewport
                };

                // render the page contents in the canvas
                page.render(renderContext).then(function () {
                    document.querySelector("#pdf-preview").style.display = 'inline-block';
                    document.querySelector("#pdf-loader").style.display = 'none';
                    document.querySelector("#changePDFLoader").style.display = 'inline-block';
                    //document.querySelector("#changeBookLbl").style.display = 'inline-block';
                    var image = document.getElementById("pdf-preview").toDataURL("image/png");

                    //get raw image data
                    image = image.replace('data:image/png;base64,', '');

                    document.querySelector("#txtImgUrl").setAttribute("value", image);
                    $("#addbtn").removeAttr("disabled");
                });
            });
        }

        /* Selected File has changed */
        document.querySelector("#upload-dialog").addEventListener('change', function () {
            document.querySelector("#changePDFLoader").style.display = 'inline-block';
            // user selected file
            var file = this.files[0];

            // allowed MIME types
            var mime_types = ['application/pdf'];

            // Validate whether PDF
            if (mime_types.indexOf(file.type) == -1) {
                alert('Error : Incorrect file type');
                return;
            }

            // validate file size
            if (file.size > 10 * 1024 * 1024) {
                alert('Error : Exceeded size 10MB');
                return;
            }

            // Show the PDF preview loader
            document.querySelector("#pdf-loader").style.display = 'inline-block';
            //document.querySelector("#changeBookLbl").style.display = 'inline-block';
            // object url of PDF
            _OBJECT_URL = URL.createObjectURL(file)

            // send the object url of the pdf to the PDF preview function
            showPDF(_OBJECT_URL);
        });

        $('.custom-file-input').on("change", function () {
            var fileLabel = $(this).next('.custom-file-label');
            var files = $(this)[0].files;
            if (files.length > 1) {
                fileLabel.html(files.length + ' files selected');
            }
            else if (files.length == 1) {
                fileLabel.html(files[0].name);
            }
        });
    }
});


//Script for Edit Form Page
$(document).ready(function () {
    if (document.querySelector("#pdf-loader") != null && document.querySelector("#changeBookLbl") != null && document.querySelector("#changePDFLoader") != null) {
        document.querySelector("#pdf-loader").style.display = 'none';
        document.querySelector("#changePDFLoader").style.display = 'none';
        document.querySelector("#changeBookLbl").style.display = 'none';


        //$("#updbtn").attr("disabled","disabled");
        var _PDF_DOC,
            _CANVAS = document.querySelector('#pdf-preview'),
            _OBJECT_URL;

        function showPDF(pdf_url) {
            PDFJS.getDocument({ url: pdf_url }).then(function (pdf_doc) {

                _PDF_DOC = pdf_doc;

                // Show the first page
                showPage(1);

                // destroy previous object url
                URL.revokeObjectURL(_OBJECT_URL);
            }).catch(function (error) {
                // trigger Cancel on error
                //document.querySelector("#cancel-pdf").click();

                // error reason
                alert(error.message);
            });;
        }

        function showPage(page_no) {
            // fetch the page
            _PDF_DOC.getPage(page_no).then(function (page) {
                // set the scale of viewport
                var scale_required = _CANVAS.width / page.getViewport(1).width;

                // get viewport of the page at required scale
                var viewport = page.getViewport(scale_required);

                // set canvas height
                _CANVAS.height = viewport.height;

                var renderContext = {
                    canvasContext: _CANVAS.getContext('2d'),
                    viewport: viewport
                };

                // render the page contents in the canvas
                page.render(renderContext).then(function () {
                    document.querySelector("#pdf-preview").style.display = 'inline-block';
                    document.querySelector("#pdf-loader").style.display = 'none';
                    document.querySelector("#changePDFLoader").style.display = 'inline-block';
                    document.querySelector("#changeBookLbl").style.display = 'inline-block';
                    var image = document.getElementById("pdf-preview").toDataURL("image/png");

                    //get raw image data
                    image = image.replace('data:image/png;base64,', '');

                    document.querySelector("#txtImgUrl").setAttribute("value", image);
                    $("#updbtn").removeAttr("disabled");
                });
            });
        }

        /* Selected File has changed */
        document.querySelector("#upload-dialog").addEventListener('change', function () {
            document.querySelector("#changePDFLoader").style.display = 'inline-block';
            // user selected file
            var file = this.files[0];

            // allowed MIME types
            var mime_types = ['application/pdf'];

            // Validate whether PDF
            if (mime_types.indexOf(file.type) == -1) {
                alert('Error : Incorrect file type');
                return;
            }

            // validate file size
            if (file.size > 10 * 1024 * 1024) {
                alert('Error : Exceeded size 10MB');
                return;
            }

            // Show the PDF preview loader
            document.querySelector("#pdf-loader").style.display = 'inline-block';
            document.querySelector("#changeBookLbl").style.display = 'inline-block';
            // object url of PDF
            _OBJECT_URL = URL.createObjectURL(file)

            // send the object url of the pdf to the PDF preview function
            showPDF(_OBJECT_URL);
        });

        $('.custom-file-input').on("change", function () {
            var fileLabel = $(this).next('.custom-file-label');
            var files = $(this)[0].files;
            if (files.length > 1) {
                fileLabel.html(files.length + ' files selected');
            }
            else if (files.length == 1) {
                fileLabel.html(files[0].name);
            }
        });
    }
});
