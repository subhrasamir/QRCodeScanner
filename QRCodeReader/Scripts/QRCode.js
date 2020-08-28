
var appUrl;
$(document).ready(function (event) {
    $("#divQRCode").hide();
    appUrl = $("#ApplicationPath").val();
    $('input[type="file"]').change(function (e) {
        var fileName = e.target.files[0].name;
        var fileSize = e.target.files[0].size;
        var fileData = new FormData();
        fileData.append(e.target.files[0].name, e.target.files[0]);
        var fileExtension = ['png', 'gif', 'jpeg', 'jpg'];

        if ($.inArray(fileName.split('.').pop().toLowerCase(), fileExtension) != -1) {

            if (fileSize <= 1048576) {
                $.ajax({
                    url: appUrl + 'Home/UploadQRCode',
                    type: "POST",
                    contentType: false,
                    processData: false,
                    data: fileData,
                    success: function (result) {
                        $("#tdQRcode").show();
                        $("#lblErrorMsg").text('');
                        var imgPath = "../QRCodeFiles/" + e.target.files[0].name
                        $("#imgQRCode").attr('src', imgPath);
                        $.each(JSON.parse(JSON.parse(result)), function (index, jsonObject) {
                            $.each(jsonObject, function (key, val) {
                                if (key.trim() == "type") {
                                    $("#lblType").text(val);
                                }
                                if (key.trim() == "symbol") {
                                    $.each(JSON.parse(JSON.parse(result))[0].symbol, function (index1, jsonObject1) {
                                        $.each(jsonObject1, function (key1, val1) {
                                            switch (key1.trim()) {
                                                case "seq":
                                                    $("#lblSeq").text(val1);
                                                    break;
                                                case "data":
                                                    $("#lblData").text(val1);
                                                    $("#QrData").text(val1);

                                                    break;
                                                case "error":
                                                    $("#lblError").text(val1);
                                                    break;
                                                default:
                                            }

                                        });
                                    });
                                }
                            });
                        });

                        $("#divQRCode").show();
                    },
                    error: function (err) {
                        $("#tdQRcode").hide();
                        $("#divQRCode").hide();
                        alert(err.statusText);
                    }
                });
                $(this).attr("value", "");
            }
            else {
                $("#divQRCode").hide();
                $("#tdQRcode").hide();
                $("#lblErrorMsg").text('* Max 1 MB file size is allowed to upload ! ');
                $("#txtUploadQRcode").replaceWith($("#txtUploadQRcode").val('').clone(true));
            }

        }
        else {
            $("#divQRCode").hide();
            $("#tdQRcode").hide();
            $("#lblErrorMsg").text('* Invalid file uploaded. Please upload files with valid format:  ' + fileExtension.join(', '));
            $("#txtUploadQRcode").replaceWith($("#txtUploadQRcode").val('').clone(true));
        }

    });
});
