

$(document).ready(function () {
    var passempty = false;
    var passlength = false;
    $("#sbmtNewpass").click(function (e) {

        var txt = $(".txtnewpass");
        if (txt.val() === "") {
            e.preventDefault();
            $.toast({
                heading: 'Error',
                text: 'Password is empty',
                showHideTransition: 'fade',
                icon: 'error'

            })
            passempty = true;
        }
        console.log(txt.val());
        if (txt.val() !== "" && txt.val().length < 8) {
            e.preventDefault();
            $.toast({
                heading: 'Error',
                text: 'Password must containt 8 caractere',
                showHideTransition: 'fade',
                icon: 'error'
            })
            passlength = true;
        }
        console.log(passlength);
        console.log(passempty);
        if (!passlength && !passempty) {
            $.toast({
                heading: 'Success',
                text: 'Your Have been saved',
                showHideTransition: 'fade',
                icon: 'success'
            });
           
        }
    });

});