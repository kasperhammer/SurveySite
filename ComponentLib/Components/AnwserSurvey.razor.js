export function CheckBtn(id) {
    var obj = $("#input_" + id);
    // Toggle the 'checked' state using jQuery's prop method
    if (obj.prop('checked') === true) {
        obj.prop('checked', false);
    } 
}
