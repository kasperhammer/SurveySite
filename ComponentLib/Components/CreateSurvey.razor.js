export function resetAllRadioButtons() {
    $('.rdodel').each(function () {
        this.checked = false; // 'this' refers to the current radio button element
    });
}