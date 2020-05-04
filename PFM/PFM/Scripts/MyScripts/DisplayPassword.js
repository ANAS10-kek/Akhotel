$(document).ready(function () {
    var clicked = false, clicked2 = false, clicked3 = false;
    $(".icon-con").click(function () {
        if (!clicked) {
            $(".inputCon").attr("type", "text");
            $(".icon-con").css("background-image", 'url(../pic/eye-slash-solid.svg)');
            clicked = true;
        }
        else if (clicked) {
            $(".inputCon").attr("type", "Password");
            $(".icon-con").css("background-image", 'url(../pic/eye-solid.svg)');
            clicked = false;
        }
        return;
    });
    $(".icon-pass").click(function () {
        if (!clicked2) {
            $(".passinput").attr("type", "text");
            $(".icon-pass").css("background-image", 'url(../pic/eye-slash-solid.svg)');
            clicked2 = true;
        }
        else if (clicked2) {
            $(".passinput").attr("type", "Password");
            $(".icon-pass").css("background-image", 'url(../pic/eye-solid.svg)');
            clicked2 = false;
        }
        return;
    });
    
    
});