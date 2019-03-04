$(document).ready(function () {

    EnableDisableControls()
});
}
function EnableDisableControls() {
    $("#checkID").change(function () {
        if ($("#checkID").is(":checked")) {
            $("#myOwner").find('*').prop("disabled", true);
        }
        else {
            $("#myOwner").find('*').prop("disabled", false);
        }
    });
}