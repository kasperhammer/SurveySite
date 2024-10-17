export function CheckBtn(i, id) {
    console.log("#input_" + i + "_" + id);
    var obj = $("#input_" + i + "_" + id);
    console.log(obj);
    // Toggle the 'checked' state using jQuery's prop method
    if (obj.prop('checked') === true) {
        obj.prop('checked', false);
    }
}
